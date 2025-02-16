using DaftAppleGames.SubnauticaPets.Pets;
using HarmonyLib;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    /// <summary>
    /// Patches for the Trivalve pet
    /// </summary>
    [HarmonyPatch(typeof(TrivalvePlayerInteraction))]
    internal class TrivalvePlayerInteractionPatches
    {
        [HarmonyPatch(nameof(TrivalvePlayerInteraction.AllowedToInteract))]
        [HarmonyPrefix]
        public static bool AllowedToInteract_Prefix(TrivalvePlayerInteraction __instance, ref bool __result,
            SwimWalkCreatureController.State swimWalkState)
        {
            if (__instance.GetComponentInParent<Pet>())
            {
                __result = true;
                return false;
            }
            return true;
        }

        [HarmonyPatch(nameof(TrivalvePlayerInteraction.OnHandHover))]
        [HarmonyPostfix]
        public static void OnHandHover_Postfix(TrivalvePlayerInteraction __instance, GUIHand hand)
        {
            Pet pet = __instance.GetComponentInParent<Pet>();
            if (pet)
            {
                HandReticle.main.SetText(HandReticle.TextType.Hand, $"Play with {pet.PetName}", false, GameInput.Button.LeftHand);
            }
        }
    }
}
