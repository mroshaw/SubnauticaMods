using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Logger = QModManager.Utility.Logger;

namespace MroshawMods.CreaturePetMod_BZ
{
    /// <summary>
    /// Provides patched methods for Pet specific behaviour
    /// </summary>
    class PetPatcher
    {
        /// <summary>
        /// Show pet info on hand over
        /// </summary>
        [HarmonyPatch(typeof(Pickupable))]
        [HarmonyPatch("OnHandHover")]
        internal class CursorHoverOverCreature
        {
            [HarmonyPrefix]
            public static bool ShowPetInfo(Pickupable __instance, GUIHand hand)
            {
                HandReticle main = HandReticle.main;
                if (!hand.IsFreeToInteract()) return true;
                CreaturePet creaturePet = __instance.GetComponentInParent<CreaturePet>();
                if (!creaturePet) return true;
                TechType techType = __instance.GetTechType();
                main.SetIcon(HandReticle.IconType.Hand, 1f);
                main.SetText(HandReticle.TextType.Hand, $"Pet {creaturePet.GetPetName()}", true, GameInput.Button.LeftHand);
                HandReticle.main.SetText(HandReticle.TextType.HandSubscript, creaturePet.petCreatureType.ToString(), false, GameInput.Button.None);
                return false;
            }
        }

        /// <summary>
        /// Handle clicking or "Petting" of Pets
        /// </summary>
        [HarmonyPatch(typeof(Pickupable))]
        [HarmonyPatch("OnHandClick")]
        internal class PlayerClickedOnCreature
        {
            [HarmonyPrefix]
            public static bool PlayWithPet(Pickupable __instance, GUIHand hand)
            {
                HandReticle main = HandReticle.main;
                if (hand.IsFreeToInteract())
                {
                    CreaturePet creaturePet = __instance.GetComponentInParent<CreaturePet>();
                    if (creaturePet)
                    {

                        // Hold down CTRL key and click, to beckon pet towards player position
                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            // Walk towards the player
                            creaturePet.WalkToPlayerWithDelay();
                            return false;
                        }

                        // Perform random animation
                        creaturePet.PetWithAnimation();
                        // main.SetText(HandReticle.TextType.Hand, $"Pet: {creaturePet.petTechType.AsString(false)}", true, GameInput.Button.None);
                        // HandReticle.main.SetText(HandReticle.TextType.HandSubscript, creaturePet.GetPetName(), false, GameInput.Button.None);

                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Override interaction with Player as a platform
        /// </summary>
        [HarmonyPatch(typeof(GroundMotor))]
        [HarmonyPatch("IsValidPlatform")]
        internal class CheckForValidPlatform
        {
            [HarmonyPostfix]
            public static void OverrideIsPlatform(GroundMotor __instance, GameObject go, ref bool __result)
            {
                Creature creature = go.GetComponentInParent<Creature>();
                if (creature)
                {
                    __result = false;
                }
            }
        }

        /// <summary>
        /// Patches surface movement
        /// </summary>
        [HarmonyPatch(typeof(MoveOnSurface))]
        [HarmonyPatch("Perform")]
        private class MoveOnSurfacePatch
        {
            /// <summary>
            /// Overrides the Perform method specifically for SnowStalkerBaby pets, forcing the GoToInternal behaviour
            /// </summary>
            /// <param name="__instance"></param>
            /// <param name="time"></param>
            /// <param name="deltaTime"></param>
            /// <returns></returns>
            [HarmonyPrefix]
            private static bool PerformOverride(MoveOnSurface __instance, float time, float deltaTime)
            {
                // Get parent Creature and check that it's a Pet
                Creature creature = __instance.creature;
                CreaturePet creaturePet = __instance.gameObject.GetComponent<CreaturePet>();
                if (!creaturePet || creaturePet.petCreatureType != PetCreatureType.SnowstalkerBaby)
                {
                    // Invoke the "unpatched" method
                    return true;
                }

                if (!(__instance.timeNextTarget <= time)) return false;
                __instance.desiredPosition = __instance.FindRandomPosition();
                __instance.timeNextTarget = time + __instance.updateTargetInterval + __instance.updateTargetRandomInterval * Random.value;
                // Enforce the "GoToInternal" behaviour
                __instance.walkBehaviour.GoToInternal(__instance.desiredPosition, (__instance.desiredPosition - creature.transform.position).normalized, __instance.moveVelocity);
                Logger.Log(Logger.Level.Debug, $"{creature.GetType()} is walking to random target");
                return false;
            }
        }

        /// Class to manage "spawning" of a new pet
        /// </summary>
        [HarmonyPatch(typeof(Player))]
        [HarmonyPatch("Update")]
        internal class SpawnKeyPress
        {
            [HarmonyPostfix]
            public static void OnSpawn()
            {
                // Check for "Spawn Pet" keypress
                if (Input.GetKeyUp(QMod.Config.SpawnPetKey))
                {
                    Logger.Log(Logger.Level.Debug, "Spawn keypress detected");
                    PetSpawner.SpawnCreaturePet();
                    Logger.Log(Logger.Level.Debug, "Pet spawned!");
                    ErrorMessage.AddMessage($"Pet spawned! Welcome, {QMod.Config.PetName}!");
                }
            }
        }

        /// <summary>
        /// Class to manage the death of a pet
        /// </summary>
        [HarmonyPatch(typeof(Creature))]
        [HarmonyPatch("OnKill")]
        internal class CreatureKilled
        {
            [HarmonyPostfix]
            public static void OnKill(Creature __instance)
            {
                CreaturePet creaturePet = __instance.gameObject.GetComponent<CreaturePet>();
                // Check to see if the creature is a Pet
                if (creaturePet)
                {
                    Logger.Log(Logger.Level.Debug, "Pet death detected");
                    creaturePet.Dead();
                }
            }
        }

        [HarmonyPatch(typeof(Creature))]
        [HarmonyPatch("Start")]
        internal class CreatureCreated
        {
            /// <summary>
            /// This fixes an issue where loading a save game "undoes" our GameObject changes
            /// We lose our "creaturePet", so need another way to identify the loaded creature.
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPostfix]
            public static void ConfigureExistingPets(Creature __instance)
            {
                // First up, is this creature a pet? We lose our "creaturePet" component
                // to the save game, so need to check another way
                CreaturePet creaturePet = __instance.gameObject.GetComponent<CreaturePet>();
                if (creaturePet)
                {
                    Logger.Log(Logger.Level.Debug, "Creature Start: Already a pet, no need to reconfigure.");
                    return;
                }

                // We check against prefab Id to find pets to reconfigure
                string loadedPrefabId = __instance.gameObject.GetComponent<PrefabIdentifier>().Id;

                // If the GUID is in the list, let's reconfigure
                PetDetails petDetails = GetPetDetailsWithPrefabId(loadedPrefabId);
                if (petDetails == null) return;
                Logger.Log(Logger.Level.Debug, $"ConfigureExistingPets: Loading Creature Prefab Id: {loadedPrefabId} ({petDetails.PetName}");
                Logger.Log(Logger.Level.Debug, $"ConfigureExistingPets: Adding CreaturePet component...");
                creaturePet = __instance.gameObject.AddComponent<CreaturePet>();
                Logger.Log(Logger.Level.Debug, $"ConfigureExistingPets: Adding CreaturePet component... Done");
                Logger.Log(Logger.Level.Debug, $"ConfigureExistingPetsConfiguring Pet...");
                petDetails = creaturePet.ConfigurePet(petDetails.PetType, petDetails.PetName);
                Logger.Log(Logger.Level.Debug, $"ConfigureExistingPetsConfiguring Pet... Done.");
                Logger.Log(Logger.Level.Debug, $"Finished loading CreaturePet Prefab Id: {loadedPrefabId}");
            }

            /// <summary>
            /// Get the Pet Details from internal storage
            /// </summary>
            /// <param name="prefabid"></param>
            /// <returns></returns>
            internal static PetDetails GetPetDetailsWithPrefabId(string prefabid)
            {
                foreach (PetDetails petDetails in QMod.PetDetailsHashSet)
                {
                    if (petDetails.PrefabId == prefabid)
                    {
                        return petDetails;
                    }
                }
                return null;
            }
        }
    }
}
