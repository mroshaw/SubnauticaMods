using HarmonyLib;
using UnityEngine;

namespace Mroshaw.SeaTruckSpeedMod_BZ
{
    class SeaTruckSpeedMod_BZ
    {
        /// <summary>
        /// Harmony hooks to modify the SeaTruck drag coefficient, reducing it to increase maximum speed
        /// </summary>
        /// 
        [HarmonyPatch(typeof(SeaTruckMotor))]
        internal class SeaTruckMotor_Patch
        {
            [HarmonyPatch(nameof(SeaTruckMotor.Start))]
            [HarmonyPostfix]
            public static void Start_Postfix(SeaTruckMotor __instance)
            {
                // Grab the modifier value from Config and apply to the drag coefficient
                SeaTruckSpeedPlugin_BZ.Log.LogInfo($"SeaTruckMotor_Start ({__instance.name})");

                // Determine if this is the motor for the Main Cab
                GameObject rootSeatruck = UWE.Utils.GetEntityRoot(__instance.gameObject);
                SeaTruckSegment seaTruckCabSegment = rootSeatruck.GetComponent<SeaTruckSegment>();
                if (seaTruckCabSegment.isMainCab)
                { 
                    // Get current drag
                    float currentDrag = __instance.pilotingDrag;
                    SeaTruckSpeedPlugin_BZ.Log.LogInfo($"Current drag: {currentDrag}");

                    // Add to our list of instances to allow ad-hoc change to the value
                    SeaTruckHistoryItem newSeaTruck = new SeaTruckHistoryItem(__instance, currentDrag);
                    SeaTruckSpeedPlugin_BZ.SeaTruckHistory.Add(newSeaTruck);

                    // Get current modifier
                    float dragModifier = SeaTruckSpeedPlugin_BZ.BoosterSpeedMultiplier.Value;

                    // Apply modifier
                    float newDrag = currentDrag / dragModifier;
                    __instance.pilotingDrag = newDrag;
                    SeaTruckSpeedPlugin_BZ.Log.LogInfo($"Current drag: {currentDrag} to new drag: {newDrag}");
                }
                else
                {
                    SeaTruckSpeedPlugin_BZ.Log.LogInfo("Not main cab");
                }
            }
        }
    }
}
