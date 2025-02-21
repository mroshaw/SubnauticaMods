using System.Collections.Generic;
using System.Linq;

namespace DaftAppleGames.SeaTruckSpeedMod_BZ
{
    internal static class SeaTruckHistory
    {
        private static readonly List<SeaTruckHistoryItem> SeaTruckInstanceHistory;

        static SeaTruckHistory()
        {
            SeaTruckInstanceHistory = new List<SeaTruckHistoryItem>();
        }

        /// <summary>
        /// Apply the given drag modifier to all seatruck instances
        /// </summary>
        internal static void UpdateAllDrag(float multiplier)
        {
            foreach (SeaTruckHistoryItem historyItem in SeaTruckInstanceHistory)
            {
                historyItem.ApplyDragdModifier(multiplier);
            }
        }

        /// <summary>
        /// Apply the given power efficiency modifier to all seatruck instances
        /// </summary>
        internal static void UpdateAllPowerEfficiency(float multiplier)
        {
            foreach (SeaTruckHistoryItem historyItem in SeaTruckInstanceHistory)
            {
                historyItem.ApplyPowerModifier(multiplier);
            }
        }


        /// <summary>
        /// Add a new SeaTruck
        /// </summary>
        internal static void AddSeaTruck(SeaTruckMotor seaTruck)
        {
            SeaTruckHistoryItem newSeaglideItem = new SeaTruckHistoryItem(seaTruck);
            SeaTruckInstanceHistory.Add(newSeaglideItem);
        }

        /// <summary>
        /// Remove a SeaTruck
        /// </summary>
        internal static void RemoveSeaTruck(SeaTruckMotor seaTruck)
        {
            foreach (SeaTruckHistoryItem seaTruckHistoryItem in SeaTruckInstanceHistory.ToList())
            {
                if (seaTruckHistoryItem.SeaTruckInstance == seaTruck)
                {
                    SeaTruckInstanceHistory.Remove(seaTruckHistoryItem);
                }
            }
        }
    }
}