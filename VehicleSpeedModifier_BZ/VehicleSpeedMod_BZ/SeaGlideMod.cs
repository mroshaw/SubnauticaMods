using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using System.Collections.Generic;

namespace VehicleSpeedMod_BZ
{
    class SeaGlideMod
    {
        /// <summary>
        /// Harmony hooks to modify the SeaTruck drag coefficient, reducing it to increase maximum speed
        /// </summary>
        /// 
        [HarmonyPatch(typeof(Seaglide))]
        [HarmonyPatch("Start")]
        internal class SeaGlideSpeed
        {
            [HarmonyPostfix]
            public static void Postfix(Seaglide __instance)
            {
                // Grab the modifier value from Config and apply to the drag coefficient
                Logger.Log(Logger.Level.Info, $"In post fix - SeaGlideStart");

                // Get current value
                float currentValue = __instance.maxSpinSpeed;
                Logger.Log(Logger.Level.Info, $"Current value: ");

                // Add to our list of instances to allow ad-hoc change to the value
                SeaGlideHistoryItem newSeaGlide = new SeaGlideHistoryItem(__instance, currentValue);
                QMod.SeaGlideHistory.Add(newSeaGlide);

                // Get current modifier
                float modifier = QMod.Config.SeaGlideSpeedModifier;
                Logger.Log(Logger.Level.Info, $"Modifier: {modifier}");

                // Apply modifier
                float newValue = currentValue * modifier;
                __instance.maxSpinSpeed = newValue;
                Logger.Log(Logger.Level.Info, $"New value: {newValue}");

            }
        }
    }
}
