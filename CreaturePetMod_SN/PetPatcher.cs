using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using UnityEngine;


namespace CreaturePetMod_SN
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
                        main.SetInteractText(techType.AsString());
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

                    // Walk towards the player
                    MoveOnSurface moveOnSurface = __instance.GetComponentInParent<MoveOnSurface>();
                    moveOnSurface.walkBehaviour.WalkTo(Player.main.transform.position, moveOnSurface.moveVelocity);
                    Logger.Log(Logger.Level.Debug, $"Walking towards player...");
                    return false;
                }
                return true;
            }
        }

    }
}
