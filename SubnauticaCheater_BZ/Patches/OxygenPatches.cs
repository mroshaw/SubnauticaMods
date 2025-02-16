﻿using HarmonyLib;

namespace DaftAppleGames.SubnauticaCheater_BZ.Patches
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required

    /// <summary>
    /// Sample Harmony Patch class. Suggestion is to use one file per patched class
    /// though you can include multiple patch classes in one file.
    /// Below is included as an example, and should be replaced by classes and methods
    /// for your mod.
    /// </summary>
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
            __result = __instance.oxygenAvailable;
            return false;
        }
    }
}