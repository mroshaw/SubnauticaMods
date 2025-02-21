using HarmonyLib;

namespace DaftAppleGames.SeaTruckSpeedMod_BZ.Patches
{
    class SeaTruckMotorPatches
    {
        /// <summary>
        /// Harmony hooks to modify the SeaTruck drag coefficient and power efficiency,
        /// reducing it to increase maximum speed
        /// </summary>
        /// 
        [HarmonyPatch(typeof(SeaTruckMotor))]
        internal class SeaTruckMotorPatch
        {
            [HarmonyPatch(nameof(SeaTruckMotor.Start))]
            [HarmonyPostfix]
            public static void Start_Postfix(SeaTruckMotor __instance)
            {
                SeaTruckHistory.AddSeaTruck(__instance);
            }
        }
    }
}