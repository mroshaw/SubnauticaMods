using System.Collections.Generic;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.DockRecaller
{
    public static class DockRecallers
    {
        private static readonly List<SeaTruckDockRecaller> AllDockRecallers;
        private static int Count => AllDockRecallers.Count;

        static DockRecallers()
        {
            AllDockRecallers = new List<SeaTruckDockRecaller>();
        }

        /// <summary>
        /// Add a new recaller
        /// </summary>
        internal static void AddInstance(SeaTruckDockRecaller dockRecaller)
        {
            dockRecaller.MaxRange = ConfigFile.MaximumRange;
            Log.LogDebug($"DockRecaller: Registered new instance: {dockRecaller.gameObject.name} with Range: {dockRecaller.MaxRange}");
            AllDockRecallers.Add(dockRecaller);
        }

        /// <summary>
        /// Remove recaller
        /// </summary>
        internal static void RemoveInstance(SeaTruckDockRecaller dockRecaller)
        {
            AllDockRecallers.Remove(dockRecaller);
            Log.LogDebug($"DockRecaller: Removed instance: {dockRecaller.gameObject.name}");
        }

        /// <summary>
        /// Update all Dock settings (Range)
        /// </summary>
        internal static void UpdateAllDockRange(float recallRange)
        {
            Log.LogDebug($"DockRecaller: Updating range to {recallRange} for {Count} DockRecallers");
            foreach (SeaTruckDockRecaller dockRecaller in AllDockRecallers)
            {
                dockRecaller.MaxRange = recallRange;
            }
        }
    }
}