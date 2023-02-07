using HarmonyLib;

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
                SeaglideSpeedModPluginBz.Log.LogInfo($"Seaglide_Start: ({__instance.name})");

                // Get current modifier
                float forceModifier = SeaglideSpeedModPluginBz.SeaglideModifier.Value;

                // Add to our list of instances to allow ad-hoc changes to the config settings
                SeaglideHistoryItem newSeaGlide = new SeaglideHistoryItem(__instance);
                SeaglideSpeedModPluginBz.SeaglideHistory.Add(newSeaGlide);

                // Update the Seaglide force
                SeaglideUtils.UpdateSeaglide(__instance, forceModifier);
            }
        }
    }
}
