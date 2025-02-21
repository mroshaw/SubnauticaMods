using HarmonyLib;
using static DaftAppleGames.SeaglideSpeedMod_BZ.SeaglideSpeedModPluginBz;

namespace DaftAppleGames.SeaglideSpeedMod_BZ
{
    /// <summary>
    /// Class to patch in game classes and methods
    /// </summary>
    internal class SeaglideSpeedModPatches
    {
        /// <summary>
        /// Seaglide patches
        /// </summary>
        [HarmonyPatch(typeof(Seaglide))]
        internal class SeaglidePatches
        {
            /// <summary>
            /// Patch the Seaglide properties on start
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPatch(nameof(Seaglide.Start))]
            [HarmonyPostfix]
            public static void Start_Postfix(Seaglide __instance)
            {
                SeaglideHistory.AddSeaglide(__instance);
            }
        }
    }
}