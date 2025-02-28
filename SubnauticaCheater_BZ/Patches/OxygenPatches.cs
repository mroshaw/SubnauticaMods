using HarmonyLib;
using static DaftAppleGames.SubnauticaCheater_BZ.SubnauticaCheater_BZPlugin;

namespace DaftAppleGames.SubnauticaCheater_BZ.Patches
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
        public static bool RemoveOxygen_Prefix(Oxygen __instance, ref float __result)
        {
            if (ConfigFile.OxygenCheat)
            {
                __result = __instance.oxygenAvailable;
                return false;
            }
            return true;
        }
    }
}