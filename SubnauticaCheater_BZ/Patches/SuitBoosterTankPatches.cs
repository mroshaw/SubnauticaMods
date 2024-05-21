using HarmonyLib;

namespace DaftAppleGames.SubnauticaCheater_BZ.Patches
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
            private const float BoostModifier = 5.0f;
            
            [HarmonyPatch(nameof(SuitBoosterTank.Awake))]
            [HarmonyPostfix]
            public static void Awake_Postfix(SuitBoosterTank __instance)
            {
                // Get current instance values
                float currentBoostValue = __instance.motor.motorForce;

                // Apply boost modifier
                float newBoostValue = currentBoostValue * BoostModifier;
                __instance.motor.motorForce = newBoostValue;

                // Update oxygen consumption
                __instance.boostOxygenUsePerSecond = 0.0f;
            }
        }
    }
}
