using HarmonyLib;

namespace DaftAppleGames.SubnauticaCheater.Patches
{
    [HarmonyPatch(typeof(Oxygen))]
    internal class OxygenPatches
    {
        /// <summary>
        /// Patches the Player Awake method with prefix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(Oxygen.RemoveOxygen))]
        [HarmonyPrefix]
        public static bool RemoveOxygen_Prefix(Oxygen __instance)
        {
            return false;
        }
    }
}