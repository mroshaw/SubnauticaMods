using BepInEx.Logging;
using HarmonyLib;

namespace Mroshaw.BoosterTankSpeedMod_BZ
{
    public class BoosterTankSpeedMod_BZ
    {
        /// <summary>
        /// Harmony hooks to modify the Booster Tank Max Speed
        /// </summary>
        /// 
        [HarmonyPatch(typeof(SuitBoosterTank))]
        internal class SuitBoosterTank_Patch
        {
            [HarmonyPatch(nameof(SuitBoosterTank.Awake))]
            [HarmonyPostfix]
            public static void Awake_Postfix(SuitBoosterTank __instance)
            {
                // Grab the modifier value from Config and apply to motorForce
                BoosterTankSpeedPlugin_BZ.Log.LogInfo("In SuitBoosterTank_Awake");

                // Get current modifiers from menu config
                float boostModifier = BoosterTankSpeedPlugin_BZ.BoosterSpeedMultiplier.Value;
                float oxygenModifier = BoosterTankSpeedPlugin_BZ.OxygenConsumptionMultiplier.Value;
                BoosterTankSpeedPlugin_BZ.Log.LogInfo($"Boost Modifier: {boostModifier}");
                BoosterTankSpeedPlugin_BZ.Log.LogInfo($"Oxygen: Modifier: {oxygenModifier}");

                // Get current instance values
                float currentBoostValue = __instance.motor.motorForce;
                float currentOxygenValue = __instance.boostOxygenUsePerSecond;

                // Apply boost modifier
                float newBoostValue = currentBoostValue * boostModifier;
                __instance.motor.motorForce = newBoostValue;
                BoosterTankSpeedPlugin_BZ.Log.LogInfo($"Changed motorForce from: {currentBoostValue} to: {newBoostValue}");

               // Update oxygen consumption
                float newOxygenValue = oxygenModifier * currentOxygenValue;
                __instance.boostOxygenUsePerSecond = newOxygenValue;
                BoosterTankSpeedPlugin_BZ.Log.LogInfo($"Changed oxygenConsumption from: {currentOxygenValue} to: {newOxygenValue}");

                // Add to list of instances
                BoosterTankHistoryItem boosterTankHistoryItem = new BoosterTankHistoryItem(__instance, currentBoostValue, currentOxygenValue);
                BoosterTankSpeedPlugin_BZ.BoosterTankHistory.Add(boosterTankHistoryItem);
            }
        }
      }
}
