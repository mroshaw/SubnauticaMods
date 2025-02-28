using static DaftAppleGames.SubnauticaCheater_BZ.SubnauticaCheater_BZPlugin;
using HarmonyLib;

namespace DaftAppleGames.SubnauticaCheater_BZ.Patches
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required

    /// <summary>
    /// Sample Harmony Patch class. Suggestion is to use one file per patched class
    /// though you can include multiple patch classes in one file.
    /// Below is included as an example, and should be replaced by classes and methods
    /// for your mod.
    /// </summary>
    [HarmonyPatch(typeof(BodyTemperature))]
    internal class BodyTemperaturePatches
    {
        /// <summary>
        /// Patches the Player Awake method with prefix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(BodyTemperature.AddCold))]
        [HarmonyPrefix]
        public static bool AddCold_Prefix(Player __instance)
        {
            return !ConfigFile.OxygenCheat;
        }
    }
}