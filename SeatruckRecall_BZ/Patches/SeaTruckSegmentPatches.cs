using HarmonyLib;
using DaftAppleGames.SeatruckRecall_BZ.MonoBehaviours;

namespace DaftAppleGames.SeatruckRecall_BZ.Patches
{
    /// <summary>
    /// Harmony patches for the Seatruck
    /// </summary>
    [HarmonyPatch(typeof(SeaTruckSegment))]
    internal class SeaTruckSegmentPatches
    {
        /// <summary>
        /// Patch the Start method, to add the instance
        /// to the static global list
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(SeaTruckSegment.Start))]
        [HarmonyPostfix]
        internal static void StartPostfix(SeaTruckSegment __instance)
        {
            // Check to see if this is the "main" truck segment
            if (__instance.isMainCab)
            {
                // Add the SeatruckRecallListener component
                if (!__instance.gameObject.GetComponent<SeaTruckAutoPilot>())
                {
                    // Add the new AutoPilot component
                    SeaTruckDockRecallPlugin.Log.LogInfo("Adding SeaTruckAutopilot component...");
                    SeaTruckAutoPilot newAutoPilot = __instance.gameObject.AddComponent<SeaTruckAutoPilot>();
                    SeaTruckDockRecallPlugin.Log.LogInfo(
                        $"Added SeaTruckAutopilot component to {__instance.gameObject.name}!");

                    // Register the new AutoPilot component with all registered Dock Recallers
                    SeaTruckDockRecallPlugin.RegisterAutoPilot(newAutoPilot);

                    // Add the Waypoint Nav component
                    SeaTruckDockRecallPlugin.Log.LogInfo("Adding WaypointNavigation component...");
                    WaypointNavigation waypointNav = __instance.gameObject.AddComponent<WaypointNavigation>();
                    SeaTruckDockRecallPlugin.Log.LogInfo(
                        $"Added WaypointNavigation component to {__instance.gameObject.name}!");
                }
            }
        }

        /// <summary>
        /// Patch the OnDestroy method, to remove
        /// the instance from the global list
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(SeaTruckSegment.OnDestroy))]
        [HarmonyPostfix]
        internal static void OnDestroyPostfix(SeaTruckSegment __instance)
        {
            SeaTruckAutoPilot newAutoPilot = __instance.GetComponent<SeaTruckAutoPilot>();
            if (newAutoPilot)
            {
                SeaTruckDockRecallPlugin.UnRegisterAutoPilot(newAutoPilot);
            }
        }
    }
}
