using HarmonyLib;
using Logger = QModManager.Utility.Logger;

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

                // Get current modifiers from menu config
                float boostModifier = QMod.Config.BoosterTankSpeedModifier;
                float oxygenModifier = QMod.Config.OxygenConsumptionModifier;
                Logger.Log(Logger.Level.Debug, $"Boost Modifier: {boostModifier}");
                Logger.Log(Logger.Level.Debug, $"Oxygen: Modifier: {oxygenModifier}");

                // Get current instance values
                float currentBoostValue = __instance.motor.motorForce;
                float currentOxygenValue = __instance.boostOxygenUsePerSecond;

                // Apply boost modifier
                float newBoostValue = currentBoostValue * boostModifier;
                __instance.motor.motorForce = newBoostValue;
                Logger.Log(Logger.Level.Debug, $"Changed motorForce from: {currentBoostValue} to: {newBoostValue}");

               // Update oxygen consumption
                float newOxygenValue = oxygenModifier * currentOxygenValue;
                __instance.boostOxygenUsePerSecond = newOxygenValue;
                Logger.Log(Logger.Level.Debug, $"Changed oxygenConsumption from: {currentOxygenValue} to: {newOxygenValue}");

                // Add to list of instances
                BoosterTankHistoryItem boosterTankHistoryItem = new BoosterTankHistoryItem(__instance, currentBoostValue, currentOxygenValue);
                QMod.BoosterTankHistory.Add(boosterTankHistoryItem);
            }
        }
      }
}
