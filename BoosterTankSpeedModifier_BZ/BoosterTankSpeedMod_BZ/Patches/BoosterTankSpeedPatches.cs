using BepInEx.Logging;
using HarmonyLib;

namespace DaftAppleGames.BoosterTankSpeedMod_BZ.Patches
{
    public class BoostTankSpeedPatches
    {
        /// <summary>
        /// Harmony hooks to modify the Booster Tank Max Speed
        /// </summary>
        /// 
        [HarmonyPatch(typeof(SuitBoosterTank))]
        internal class SuitBoosterTankPatch
        {
            [HarmonyPatch(nameof(SuitBoosterTank.Awake))]
            [HarmonyPostfix]
            public static void Awake_Postfix(SuitBoosterTank __instance)
            {
                // Grab the modifier value from Config and apply to motorForce
                BoosterTankSpeedPluginBz.Log.LogInfo("In SuitBoosterTank_Awake");

                // Get current modifiers from menu config
                float boostModifier = BoosterTankSpeedPluginBz.BoosterSpeedMultiplier.Value;
                float oxygenModifier = BoosterTankSpeedPluginBz.OxygenConsumptionMultiplier.Value;
                BoosterTankSpeedPluginBz.Log.LogInfo($"Boost Modifier: {boostModifier}");
                BoosterTankSpeedPluginBz.Log.LogInfo($"Oxygen: Modifier: {oxygenModifier}");

                // Get current instance values
                float currentBoostValue = __instance.motor.motorForce;
                float currentOxygenValue = __instance.boostOxygenUsePerSecond;

                // Apply boost modifier
                float newBoostValue = currentBoostValue * boostModifier;
                __instance.motor.motorForce = newBoostValue;
                BoosterTankSpeedPluginBz.Log.LogInfo($"Changed motorForce from: {currentBoostValue} to: {newBoostValue}");

               // Update oxygen consumption
                float newOxygenValue = oxygenModifier * currentOxygenValue;
                __instance.boostOxygenUsePerSecond = newOxygenValue;
                BoosterTankSpeedPluginBz.Log.LogInfo($"Changed oxygenConsumption from: {currentOxygenValue} to: {newOxygenValue}");

                // Add to list of instances
                BoosterTankHistoryItem boosterTankHistoryItem = new BoosterTankHistoryItem(__instance, currentBoostValue, currentOxygenValue);
                BoosterTankSpeedPluginBz.BoosterTankHistory.Add(boosterTankHistoryItem);
            }
        }
    }
}
