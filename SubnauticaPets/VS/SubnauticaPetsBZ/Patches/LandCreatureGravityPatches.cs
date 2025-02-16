using DaftAppleGames.SubnauticaPets.Pets;
using HarmonyLib;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    [HarmonyPatch(typeof(LandCreatureGravity))]
    internal class LandCreatureGravityPatches
    {
        /// <summary>
        /// Patches the Player Awake method with prefix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(LandCreatureGravity.IsUnderwater))]
        [HarmonyPrefix]
        public static bool IsUnderwater_Prefix(Player __instance, float waterLevelOffset, bool __result)
        {
            if (__instance.TryGetComponent<Pet>(out Pet pet))
            {
                // LogUtils.LogDebug(LogArea.Patches, "Overriding Pet IsUnderwater check...");
                __result = false;
                return false;
            }
            return true;
        }
    }
}