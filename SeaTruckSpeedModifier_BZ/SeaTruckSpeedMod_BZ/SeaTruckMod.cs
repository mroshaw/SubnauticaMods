using HarmonyLib;
using Logger = QModManager.Utility.Logger;

namespace SeaTruckSpeedMod_BZ
{
    class SeaTruckMod
    {
        /// <summary>
        /// Harmony hooks to modify the SeaTruck drag coefficient, reducing it to increase maximum speed
        /// </summary>
        /// 
        [HarmonyPatch(typeof(SeaTruckMotor))]
        [HarmonyPatch("Start")]
        internal class SeaTruckDragMod
        {
            [HarmonyPostfix]
            public static void Postfix(SeaTruckMotor __instance)
            {
                // Grab the modifier value from Config and apply to the drag coefficient
                Logger.Log(Logger.Level.Debug, "In post fix");

                // Get current drag
                float currentDrag = __instance.pilotingDrag;
                Logger.Log(Logger.Level.Debug, $"Current drag: {currentDrag}");

                // Add to our list of instances to allow ad-hoc change to the value
                SeaTruckHistoryItem newSeaTruck = new SeaTruckHistoryItem(__instance, currentDrag);
                QMod.SeaTruckHistory.Add(newSeaTruck);

                // Get current modifier
                float dragModifier = QMod.Config.SeaTruckSpeedModifier;

                // Apply modifier
                float newDrag = currentDrag / dragModifier;
                __instance.pilotingDrag = newDrag;
                Logger.Log(Logger.Level.Debug, $"Current drag: {currentDrag} to new drag: {newDrag}");
            }
        }
    }
}
