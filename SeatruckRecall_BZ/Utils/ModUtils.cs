using DaftAppleGames.SeatruckRecall_BZ.AutoPilot;
using DaftAppleGames.SeatruckRecall_BZ.DockRecaller;
using System.Collections.Generic;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;

namespace DaftAppleGames.SeatruckRecall_BZ.Utils
{
    /// <summary>
    /// Class of mod specific static utility methods
    /// </summary>
    internal static class ModUtils
    {
        // Static list of Docking Recallers
        internal static List<SeaTruckDockRecaller> AllDockRecallers { get; set; } = new List<SeaTruckDockRecaller>();

        // Static list of SeaTrucks(AutoPilot)
        internal static List<SeaTruckAutoPilot> AllSeaTruckAutoPilots { get; set; } = new List<SeaTruckAutoPilot>();

        /// <summary>
        /// Register a DockRecaller
        /// </summary>
        /// <param name="dockRecaller"></param>
        internal static void RegisterDockRecaller(SeaTruckDockRecaller dockRecaller)
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Registering SeaTruckDockRecaller...");
            AllDockRecallers.Add(dockRecaller);
            SeaTruckDockRecallPlugin.Log.LogDebug("Registered SeaTruckDockRecaller!");
        }

        /// <summary>
        /// Un-register a DockRecaller
        /// </summary>
        /// <param name="dockRecaller"></param>
        internal static void UnRegisterDockRecaller(SeaTruckDockRecaller dockRecaller)
        {
            SeaTruckDockRecallPlugin.Log.LogInfo("Un-registering SeaTruckDockRecaller...");
            AllDockRecallers.Remove(dockRecaller);
            SeaTruckDockRecallPlugin.Log.LogInfo("Un-registered SeaTruckDockRecaller!");
        }

        /// <summary>
        /// Register a Seatruck with the recaller
        /// </summary>
        /// <param name="autoPilot"></param>
        internal static void RegisterAutoPilot(SeaTruckAutoPilot autoPilot)
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Registering SeaTruckAutoPilot...");
            AllSeaTruckAutoPilots.Add(autoPilot);
            SeaTruckDockRecallPlugin.Log.LogDebug("Registered SeaTruckAutoPilot!");
        }

        /// <summary>
        /// Un-register a Seatruck with the recaller
        /// </summary>
        /// <param name="autoPilot"></param>
        internal static void UnRegisterAutoPilot(SeaTruckAutoPilot autoPilot)
        {
            SeaTruckDockRecallPlugin.Log.LogInfo("Un-registering SeaTruckAutoPilot...");
            AllSeaTruckAutoPilots.Remove(autoPilot);
            SeaTruckDockRecallPlugin.Log.LogInfo("Un-registered SeaTruckAutoPilot!");
        }

        /// <summary>
        /// Update all AutoPilots (Speed)
        /// </summary>
        /// <param name="moveSpeed"></param>
        internal static void UpdateAllAutoPilotSpeed(float moveSpeed)
        {
            foreach (SeaTruckAutoPilot autoPilot in AllSeaTruckAutoPilots)
            {
                autoPilot.gameObject.GetComponent<BaseNavMovement>().MoveSpeed = moveSpeed;
            }
        }

        /// <summary>
        /// Update all AutoPilots (Rotate)
        /// </summary>
        /// <param name="rotateSpeed"></param>
        internal static void UpdateAllAutoPilotRotate(float rotateSpeed)
        {
            foreach (SeaTruckAutoPilot autoPilot in AllSeaTruckAutoPilots)
            {
                autoPilot.gameObject.GetComponent<BaseNavMovement>().RotateSpeed = rotateSpeed;
            }
        }

        /// <summary>
        /// Update all Dock settings (Range)
        /// </summary>
        /// <param name="recallRange"></param>
        internal static void UpdateAllDockSettings(float recallRange)
        {
            foreach (SeaTruckDockRecaller dockRecaller in AllDockRecallers)
            {
                dockRecaller.MaxRange = recallRange;
            }
        }
    }
}
