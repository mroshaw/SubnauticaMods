using HarmonyLib;
using mset;
using UnityEngine;

namespace DaftAppleGames.CreaturePetMod_BZ
{
    /// <summary>
    /// Provides patched methods for Pet specific behaviour
    /// </summary>
    public class CreaturePetPatches
    {

        [HarmonyPatch(typeof(SkyApplier))]
        internal class SkyApplierPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(SkyApplier.SetSky))]
            public static void SetSky_Prefix(SkyApplier __instance, Skies skyMode)
            {
                if (__instance.gameObject.GetComponent<Trivalve>())
                {
                    CreaturePetPluginBz.Log.LogDebug("Trivalve: In SetSky");
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(SkyApplier.SetCustomSky))]
            public static void SetCustomSky_Prefix(SkyApplier __instance, Sky customSky)
            {
                if (__instance.gameObject.GetComponent<Trivalve>())
                {
                    CreaturePetPluginBz.Log.LogDebug("Trivalve: In SetCustomSky");
                }
            }

        }

        /// <summary>
        /// Show pet info on hand over
        /// </summary>
        [HarmonyPatch(typeof(Pickupable))]
        internal class PickupablePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(Pickupable.OnHandHover))]
            public static bool OnHandOver_Prefix(Pickupable __instance, GUIHand hand)
            {
                HandReticle main = HandReticle.main;
                if (!hand.IsFreeToInteract()) return true;
                CreaturePet creaturePet = __instance.GetComponentInParent<CreaturePet>();
                if (!creaturePet) return true;

                // Check for right mouse click
                if (Player.main.GetRightHandDown())
                {
                    // Walk towards the player
                    creaturePet.WalkToPlayerWithDelay();
                    return false;
                }

                main.SetIcon(HandReticle.IconType.Hand);
                main.SetText(HandReticle.TextType.Hand, $"Pet {creaturePet.GetPetName()}", false, GameInput.Button.LeftHand);
                main.SetText(HandReticle.TextType.HandSubscript, $"Beckon {creaturePet.GetPetName()}", false, GameInput.Button.RightHand);
                return false;
            }

            /// <summary>
            /// Patch in the Click Pet behaviour
            /// </summary>
            /// <param name="__instance"></param>
            /// <param name="hand"></param>
            /// <returns></returns>
            [HarmonyPrefix]
            [HarmonyPatch(nameof(Pickupable.OnHandClick))]
            public static bool PlayWithPet(Pickupable __instance, GUIHand hand)
            {
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
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Patch the "Player Hover Over" method for the Trivalve Pet
        /// </summary>
        [HarmonyPatch(typeof(TrivalvePlayerInteraction))]
        internal class TrivalvePlayerInteractionPatch
        {
            [HarmonyPatch(nameof(TrivalvePlayerInteraction.OnHandHover))]
            [HarmonyPrefix]
            public static bool OnHandHover_Prefix(TrivalvePlayerInteraction __instance, GUIHand hand)
            {
                // CreaturePetPlugin_BZ.Log.LogDebug("In TrivalvePlayerInteraction.OnHandOver");
                CreaturePet creaturePet = __instance.GetComponentInParent<CreaturePet>();
                // Call original class method if not dealing with a Pet
                if (!creaturePet)
                {
                    return true;
                }
                if (!creaturePet.AllowedToInteract(__instance.trivalve.swimWalkController.state))
                {
                    return false;
                }
                if (Player.main.GetRightHandDown())
                {
                    __instance.Invoke("PlayCommandAnimation", 0f);
                    return false;
                }

                // Configure cursor
                string text = __instance.trivalve.followingPlayer ? "FishStopFollow" : "FishStartFollow";
                HandReticle.main.SetText(HandReticle.TextType.Hand, $"Play with {creaturePet.GetPetName()}", false, GameInput.Button.LeftHand);
                HandReticle.main.SetText(HandReticle.TextType.HandSubscript, text, true, GameInput.Button.RightHand);
                HandReticle.main.SetIcon(HandReticle.IconType.Hand);
                return false;
            }

            [HarmonyPatch(nameof(TrivalvePlayerInteraction.OnHandClick))]
            [HarmonyPrefix]
            public static bool OnHandClick_Prefix(TrivalvePlayerInteraction __instance, GUIHand hand)
            {
                CreaturePet creaturePet = __instance.GetComponentInParent<CreaturePet>();
                // Call original class method if not dealing with a Pet
                if (!creaturePet)
                {
                    CreaturePetPluginBz.Log.LogDebug("TrivalvePlayerInteraction.OnHandClick: not a pet Trivalve!");
                    return true;
                }

                SwimWalkCreatureController.State state = __instance.trivalve.swimWalkController.state;
                if (!creaturePet.AllowedToInteract(state))
                {
                    CreaturePetPluginBz.Log.LogDebug("TrivalvePlayerInteraction.OnHandClick: Pet Trivalve, not allowed to interact!");
                    return false;
                }
                if (state == SwimWalkCreatureController.State.Swim)
                {
                    CreaturePetPluginBz.Log.LogDebug("TrivalvePlayerInteraction.OnHandClick: Swimming state!");
                    __instance.PrepareWaterCinematic(hand.player);
                    return false;
                }
                CreaturePetPluginBz.Log.LogDebug("TrivalvePlayerInteraction.OnHandClick: Land state!");
                __instance.PrepareLandCinematic(hand.player);
                return false;
            }


        }

        /// <summary>
        /// Override interaction with Player as a platform
        /// </summary>
        [HarmonyPatch(typeof(GroundMotor))]
        internal class GroundMotorPatch
        {
            [HarmonyPatch(nameof(GroundMotor.IsValidPlatform))]
            [HarmonyPostfix]
            public static void IsValidPlatform_Postfix(GroundMotor __instance, GameObject go, ref bool __result)
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
        internal class MoveOnSurfacePatch
        {
            /// <summary>
            /// Overrides the Perform method specifically for SnowStalkerBaby pets, forcing the GoToInternal behaviour
            /// </summary>
            /// <param name="__instance"></param>
            /// <param name="time"></param>
            /// <param name="deltaTime"></param>
            /// <returns></returns>
            [HarmonyPatch(nameof(MoveOnSurface.Perform))]
            [HarmonyPrefix]
            private static bool Perform_Prefix(MoveOnSurface __instance, float time, float deltaTime)
            {
                // Get parent Creature and check that it's a Pet
                Creature creature = __instance.creature;
                CreaturePet creaturePet = __instance.gameObject.GetComponent<CreaturePet>();
                if (!creaturePet || creaturePet.PetCreatureType != PetCreatureType.SnowstalkerBaby)
                {
                    // Invoke the "unpatched" method
                    return true;
                }

                if (!(__instance.timeNextTarget <= time)) return false;
                __instance.desiredPosition = __instance.FindRandomPosition();
                __instance.timeNextTarget = time + __instance.updateTargetInterval + __instance.updateTargetRandomInterval * Random.value;
                // Enforce the "GoToInternal" behaviour
                __instance.walkBehaviour.GoToInternal(__instance.desiredPosition, (__instance.desiredPosition - creature.transform.position).normalized, __instance.moveVelocity);
                CreaturePetPluginBz.Log.LogDebug( $"{creature.GetType()} is walking to random target");
                return false;
            }
        }


        /// <summary>
        /// Patch in the Player methods
        /// </summary>
        [HarmonyPatch(typeof(Player))]

        internal class PlayerPatch
        {
            [HarmonyPatch(nameof(Player.Awake))]
            [HarmonyPostfix]
            public static void Awake_Postfix(Player __instance)
            {
                // Add the Mod Input Manager to the Player GameObject.
                // Ensures there is only one component, monitoring keyboard input.
                __instance.gameObject.AddComponent<ModInputManager>();
                CreaturePetPluginBz.Log.LogDebug("Added ModInputManager component.");
            }
        }

        /// <summary>
        /// Patch Creature Death, to register as a Pet death and handle appropriately
        /// </summary>
        [HarmonyPatch(typeof(Creature))]
        internal class CreaturePatch
        {
            [HarmonyPatch(nameof(Creature.OnKill))]
            [HarmonyPostfix]
            public static void OnKill_Postfix (Creature __instance)
            {
                CreaturePet creaturePet = __instance.gameObject.GetComponent<CreaturePet>();
                // Check to see if the creature is a Pet
                if (creaturePet)
                {
                    CreaturePetPluginBz.Log.LogDebug("Pet death detected");
                    creaturePet.Dead();
                }
            }

            /// <summary>
            /// This fixes an issue where loading a save game "undoes" our GameObject changes
            /// We lose our "creaturePet", so need another way to identify the loaded creature.
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPatch(nameof(Creature.Start))]
            [HarmonyPostfix]
            public static void ConfigureExistingPets(Creature __instance)
            {
                // First up, is this creature a pet? We lose our "creaturePet" component
                // to the save game, so need to check another way
                CreaturePet creaturePet = __instance.gameObject.GetComponent<CreaturePet>();
                if (creaturePet)
                {
                    CreaturePetPluginBz.Log.LogDebug("Creature Start: Already a pet, no need to reconfigure.");
                    return;
                }

                // We check against prefab Id to find pets to reconfigure
                string loadedPrefabId = __instance.gameObject.GetComponent<PrefabIdentifier>().Id;

                // If the GUID is in the list, let's reconfigure
                PetDetails petDetails = PetUtils.GetPetDetailsWithPrefabId(loadedPrefabId);
                if (petDetails == null) return;
                CreaturePetPluginBz.Log.LogDebug( $"ConfigureExistingPets: Loading Creature Prefab Id: {loadedPrefabId} ({petDetails.PetName})");
                CreaturePetPluginBz.Log.LogDebug( "ConfigureExistingPets: Adding CreaturePet component...");
                creaturePet = __instance.gameObject.AddComponent<CreaturePet>();
                CreaturePetPluginBz.Log.LogDebug( "ConfigureExistingPets: Adding CreaturePet component... Done");
                CreaturePetPluginBz.Log.LogDebug( "ConfigureExistingPetsConfiguring Pet...");
                petDetails = creaturePet.ConfigurePet(petDetails.PetType, petDetails.PetName);
                CreaturePetPluginBz.Log.LogDebug( "ConfigureExistingPetsConfiguring Pet... Done.");
                CreaturePetPluginBz.Log.LogDebug( $"Finished loading CreaturePet Prefab Id: {loadedPrefabId}");
            }
        }
    }
}
