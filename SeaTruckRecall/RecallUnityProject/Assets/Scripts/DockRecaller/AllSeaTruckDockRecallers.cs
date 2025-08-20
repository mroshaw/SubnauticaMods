using System.Collections.Generic;
using DaftAppleGames.SeatruckRecall_BZ.AutoPilot;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.DockRecaller
{
    public static class AllSeaTruckDockRecallers
    {
        private static readonly List<SeaTruckDockRecaller> AllDockRecallersList;
        private static int Count => AllDockRecallersList.Count;

        static AllSeaTruckDockRecallers()
        {
            AllDockRecallersList = new List<SeaTruckDockRecaller>();
        }

        /// <summary>
        /// Add a new recaller
        /// </summary>
        internal static void AddInstance(SeaTruckDockRecaller dockRecaller)
        {
            dockRecaller.MaxRange = ConfigFile.MaximumRange;
            Log.LogDebug($"DockRecaller: Registered new instance: {dockRecaller.gameObject.name} with Range: {dockRecaller.MaxRange}");
            AllDockRecallersList.Add(dockRecaller);
        }

        /// <summary>
        /// Remove recaller
        /// </summary>
        internal static void RemoveInstance(SeaTruckDockRecaller dockRecaller)
        {
            AllDockRecallersList.Remove(dockRecaller);
            Log.LogDebug($"DockRecaller: Removed instance: {dockRecaller.gameObject.name}");
        }

        /// <summary>
        /// Update all Dock settings (Range)
        /// </summary>
        internal static void UpdateAllDockRange(float recallRange)
        {
            Log.LogDebug($"DockRecaller: Updating range to {recallRange} for {Count} DockRecallers");
            foreach (SeaTruckDockRecaller dockRecaller in AllDockRecallersList)
            {
                dockRecaller.MaxRange = recallRange;
            }
        }
    }
}