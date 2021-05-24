using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using System.Collections.Generic;

namespace VehicleSpeedMod_BZ
{
    class SnowFoxMod
    {
        /// <summary>
        /// Harmony hooks to modify the SeaTruck drag coefficient, reducing it to increase maximum speed
        /// </summary>
        /// 
        [HarmonyPatch(typeof(Hoverbike))]
        [HarmonyPatch("Start")]
        internal class SnowFoxSpeed
        {
            [HarmonyPostfix]
            public static void Postfix(Hoverbike __instance)
            {
                // Grab the modifier value from Config and apply to the drag coefficient
                Logger.Log(Logger.Level.Debug, $"In post fix");

                // Get current value
                float currentValue = 0.0F;
                // Add to our list of instances to allow ad-hoc change to the value
                SnowFoxHistoryItem newSnowFox = new SnowFoxHistoryItem(__instance, currentValue);
                QMod.SnowFoxHistory.Add(newSnowFox);

                // Get current modifier
                float modifier = QMod.Config.SnowFoxSpeedModifier;
                Logger.Log(Logger.Level.Debug, $"Modifier: {modifier}");

                // Apply modifier
                float newValue = 0.0F;
                Logger.Log(Logger.Level.Debug, $"New value: {newValue}");

            }
        }
    }
}
