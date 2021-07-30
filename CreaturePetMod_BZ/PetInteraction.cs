using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;
using System.Linq;


namespace CreaturePetMod_BZ
{
    class PetInteraction
    {
        /// <summary>
        /// Show pet info on hand over
        /// </summary>
        [HarmonyPatch(typeof(Pickupable))]
        [HarmonyPatch("OnHandHover")]
        internal class PetInfo
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
                        main.SetText(HandReticle.TextType.Hand, $"Pet {creaturePet.GetPetName()}", true, GameInput.Button.LeftHand);
                        HandReticle.main.SetText(HandReticle.TextType.HandSubscript, techType.AsString(false), false, GameInput.Button.None);
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Show pet info on hand over
        /// </summary>
        [HarmonyPatch(typeof(Pickupable))]
        [HarmonyPatch("OnHandClick")]
        internal class PetPlay
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
                        switch (techType)
                        {
                            case TechType.SnowStalkerBaby:
                                SnowStalkerBaby snowStalkerBaby = __instance.GetComponentInParent<SnowStalkerBaby>();
                                snowStalkerBaby.GetAnimator().SetTrigger("dryFur");

                                break;
                            case TechType.PenguinBaby:
                                PenguinBaby penguinBaby = __instance.GetComponentInParent<PenguinBaby>();
                                penguinBaby.GetAnimator().SetTrigger("call");
                                break;

                            case TechType.Penguin:
                                Penguin penguin = __instance.GetComponentInParent<Penguin>();
                                penguin.GetAnimator().SetTrigger("peck");
                                break;
                        }
                        main.SetText(HandReticle.TextType.Hand, $"Pet: {techType.AsString(false)}", true, GameInput.Button.None);
                        HandReticle.main.SetText(HandReticle.TextType.HandSubscript, creaturePet.GetPetName(), false, GameInput.Button.None);
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
        internal class PlatformInteract
        {
            [HarmonyPostfix]
            public static void OverrideIsPlatform(GroundMotor __instance, GameObject go, ref bool __result)
            {
                Creature creature = go.GetComponentInParent<Creature>();
                if (creature)
                {
                    // Not allowed to stand on your pet!
                    __result = false;
                }
            }
        }
    }
}
