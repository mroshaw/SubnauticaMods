using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using System.Collections.Generic;

namespace VehicleSpeedMod_BZ
{
    class PrawnSuitMod
    {
        /// <summary>
        /// Harmony hooks to modify the SeaTruck drag coefficient, reducing it to increase maximum speed
        /// </summary>
        /// 
        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("Start")]
        internal class PrawnSuitSpeed
        {
            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                // Grab the modifier value from Config and apply to the drag coefficient
                Logger.Log(Logger.Level.Debug, $"In post fix - PrawnSuitStart");

                // Get current value
                float currentValue = 0.0F;
                Logger.Log(Logger.Level.Debug, $"Current value: ");

                // Add to our list of instances to allow ad-hoc change to the value
                PrawnSuitHistoryItem newPrawnSuit = new PrawnSuitHistoryItem(__instance, currentValue);
                QMod.PrawnSuitHistory.Add(newPrawnSuit);

                // Get current modifier
                float modifier = QMod.Config.PrawnSuitSpeedModifier;
                Logger.Log(Logger.Level.Debug, $"Modifier: {modifier}");

                // Apply modifier
                float newValue = 0.0F;
                Logger.Log(Logger.Level.Debug, $"New value: {newValue}");

            }
        }
    }
}
