using System.Collections.Generic;
using System.Linq;

namespace DaftAppleGames.SeaglideSpeedMod_BZ
{
    internal static class SeaglideHistory
    {
        private static readonly List<SeaglideHistoryItem> SeaglideInstanceHistory;

        static SeaglideHistory()
        {
            SeaglideInstanceHistory = new List<SeaglideHistoryItem>();
        }

        /// <summary>
        /// Apply the given speed multiplier to all seaglide instances
        /// </summary>
        internal static void UpdateAllSpeed(float multiplier)
        {
            foreach (SeaglideHistoryItem historyItem in SeaglideInstanceHistory)
            {
                historyItem.ApplySpeedMultiplier(multiplier);
            }
        }

        /// <summary>
        /// Add a new Seaglide
        /// </summary>
        internal static void AddSeaglide(Seaglide seaglide)
        {
            SeaglideHistoryItem newSeaglideItem = new SeaglideHistoryItem(seaglide);
            SeaglideInstanceHistory.Add(newSeaglideItem);
        }

        /// <summary>
        /// Remove a Seaglide
        /// </summary>
        internal static void RemoveBoosterTank(Seaglide seaglide)
        {
            foreach (SeaglideHistoryItem seaglideHistoryItem in SeaglideInstanceHistory.ToList())
            {
                if (seaglideHistoryItem.SeaglideInstance == seaglide)
                {
                    SeaglideInstanceHistory.Remove(seaglideHistoryItem);
                }
            }
        }
    }
}