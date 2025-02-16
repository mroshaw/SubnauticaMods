using HarmonyLib;
using SeaTruckSpeedMod_BZ.MonoBehaviours;
using UnityEngine;

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
                // Determine if this is the motor for the Main Cab
                GameObject rootSeatruck = UWE.Utils.GetEntityRoot(__instance.gameObject);
                SeaTruckSegment seaTruckCabSegment = rootSeatruck.GetComponent<SeaTruckSegment>();
                if (!seaTruckCabSegment.isMainCab)
                {
                    return;
                }

                // Add the SpeedMod component, if not already there
                SeaTruckSpeedMultiplier speedMod = __instance.GetComponent<SeaTruckSpeedMultiplier>();
                if (!speedMod)
                {
                    speedMod = __instance.gameObject.AddComponent<SeaTruckSpeedMultiplier>();
                }
            }
        }
    }
}
