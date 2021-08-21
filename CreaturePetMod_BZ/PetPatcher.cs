using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;
using System.Linq;


namespace CreaturePetMod_BZ
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
        internal class PetInfoPatch
        {
            [HarmonyPrefix]
            public static bool ShowPetInfo(Pickupable __instance, GUIHand hand)
            {
                HandReticle main = HandReticle.main;
                if (hand.IsFreeToInteract())
                {
                    CreaturePet creaturePet = __instance.GetComponentInParent<CreaturePet>();
                    if (creaturePet)
                    {
                        TechType techType = __instance.GetTechType();
                        main.SetIcon(HandReticle.IconType.Hand, 1f);
                        main.SetText(HandReticle.TextType.Hand, $"Pet {creaturePet.GetPetName()}", true, GameInput.Button.LeftHand);
                        HandReticle.main.SetText(HandReticle.TextType.HandSubscript, techType.AsString(false), false, GameInput.Button.None);
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Handle clicking or "Petting" of Pets
        /// </summary>
        [HarmonyPatch(typeof(Pickupable))]
        [HarmonyPatch("OnHandClick")]
        internal class PetPlayPatch
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
                        TechType techType = __instance.GetTechType();
                        string petAnimation;
                        // Pick a random animations and play it
                        switch (techType)
                        {
                            case TechType.SnowStalkerBaby:
                                SnowStalkerBaby snowStalkerBaby = __instance.GetComponentInParent<SnowStalkerBaby>();
                                petAnimation = PetUtils.GetRandomAnim(PetCreatureType.SnowstalkerBaby);
                                Logger.Log(Logger.Level.Debug, $"Random animation: {petAnimation}");
                                snowStalkerBaby.GetAnimator().SetTrigger(petAnimation);
                                break;
                            case TechType.PenguinBaby:
                                PenguinBaby penguinBaby = __instance.GetComponentInParent<PenguinBaby>();
                                petAnimation = PetUtils.GetRandomAnim(PetCreatureType.PenglingBaby);
                                Logger.Log(Logger.Level.Debug, $"Random animation: {petAnimation}");
                                penguinBaby.GetAnimator().SetTrigger(petAnimation);
                                break;

                            case TechType.Penguin:
                                Penguin penguin = __instance.GetComponentInParent<Penguin>();
                                petAnimation = PetUtils.GetRandomAnim(PetCreatureType.PenglingAdult);
                                penguin.GetAnimator().SetTrigger(petAnimation);
                                Logger.Log(Logger.Level.Debug, $"Random animation: {petAnimation}");
                                break;
                        }
                        main.SetText(HandReticle.TextType.Hand, $"Pet: {techType.AsString(false)}", true, GameInput.Button.None);
                        HandReticle.main.SetText(HandReticle.TextType.HandSubscript, creaturePet.GetPetName(), false, GameInput.Button.None);

                        // Walk towards the player
                        MoveOnSurface moveOnSurface = __instance.GetComponentInParent<MoveOnSurface>();
                        moveOnSurface.walkBehaviour.GoToInternal(Player.main.transform.position, (Player.main.transform.position - creaturePet.transform.position).normalized, moveOnSurface.moveVelocity);
                        Logger.Log(Logger.Level.Debug, $"Walking towards player...");
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
        internal class PlatformInteractPatch
        {
            [HarmonyPostfix]
            public static void OverrideIsPlatform(GroundMotor __instance, GameObject go, ref bool __result)
            {
                Creature creature = go.GetComponentInParent<Creature>();
                if (creature)
                {
                    // Logger.Log(Logger.Level.Debug, $"Preventing platform behaviour: {creature.GetType()}");
                    __result = false;
                }

            }
        }

        /// <summary>
        /// Prevents traits (e.g. happiness, agressiveness) being updated for Pet creatures
        /// </summary>
        [HarmonyPatch(typeof(Creature))]
        [HarmonyPatch("UpdateBehaviour")]
        internal class CreatureTraitPatch
        {
            [HarmonyPrefix]
            public static bool OverrideUpdateTrait(Creature __instance, float time, float deltaTime)
            {
                if (PetUtils.IsCreaturePet(__instance))
                {
                    // Logger.Log(Logger.Level.Debug, $"Preventing trait update behaviour: {__instance.GetType()}");
                    StopwatchProfiler instance = StopwatchProfiler.Instance;
                    CreatureAction creatureAction = __instance.ChooseBestAction(time);
                    if (__instance.prevBestAction != creatureAction)
                    {
                        if (__instance.prevBestAction)
                        {
                            __instance.prevBestAction.StopPerform(time);
                        }
                        if (creatureAction)
                        {
                            creatureAction.StartPerform(time);
                        }
                        __instance.prevBestAction = creatureAction;
                    }
                    if (creatureAction)
                    {
                        creatureAction.Perform(time, deltaTime);
                        __instance.lastAction = creatureAction;
                    }
                    if (__instance.traitsAnimator && __instance.traitsAnimator.isActiveAndEnabled)
                    {
                        __instance.traitsAnimator.SetFloat(Creature.animAggressive, __instance.Aggression.Value);
                        __instance.traitsAnimator.SetFloat(Creature.animScared, __instance.Scared.Value);
                        __instance.traitsAnimator.SetFloat(Creature.animTired, __instance.Tired.Value);
                        __instance.traitsAnimator.SetFloat(Creature.animHappy, __instance.Happy.Value);
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
