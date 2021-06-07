using HarmonyLib;
using Logger = QModManager.Utility.Logger;
using System.Collections.Generic;

namespace SeaTruckSpeedMod_BZ
{
    class BoosterTankMod
    {
        /// <summary>
        /// Harmony hooks to modify the Booster Tank Max Speed
        /// </summary>
        /// 
        [HarmonyPatch(typeof(SuitBoosterTank))]
        [HarmonyPatch("Awake")]
        internal class BoosterTankSpeedMod
        {
            [HarmonyPostfix]
            public static void Postfix(SuitBoosterTank __instance)
            {
                // Grab the modifier value from Config and apply to motorForce
                Logger.Log(Logger.Level.Debug, $"In SuitBoosterTank_Awake");

                // Get current modifier
                float modifier = QMod.Config.BoosterTankSpeedModifier;
                Logger.Log(Logger.Level.Debug, $"Modifier: {modifier}");

                // Apply modifier
                float currentValue = __instance.motor.motorForce;
                float newValue = currentValue * modifier;
                __instance.motor.motorForce = newValue;
                Logger.Log(Logger.Level.Debug, $"Changed motorForce from: {currentValue} to: {newValue}");

                // Add to list of instances
                BoosterTankHistoryItem boosterTankHistoryItem = new BoosterTankHistoryItem(__instance, currentValue);
                QMod.BoosterTankHistory.Add(boosterTankHistoryItem);
            }
        }
    }
}
