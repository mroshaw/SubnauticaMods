using System.Collections.Generic;
using System.Linq;

namespace DaftAppleGames.BoosterTankSpeedMod_BZ
{
    internal static class BoosterTankHistory
    {
        private static readonly List<BoosterTankHistoryItem> BoosterHistory;

        static BoosterTankHistory()
        {
            BoosterHistory = new List<BoosterTankHistoryItem>();
        }

        /// <summary>
        /// Apply the given oxygen multiplier to all booster instances
        /// </summary>
        internal static void UpdateAllOxygen(float multiplier)
        {
            foreach (BoosterTankHistoryItem historyItem in BoosterHistory)
            {
                historyItem.ApplyOxygenConsumptionMultiplier(multiplier);
            }
        }

        /// <summary>
        /// Apply the given speed multiplier to all booster instances
        /// </summary>
        internal static void UpdateAllSpeed(float multiplier)
        {
            foreach (BoosterTankHistoryItem historyItem in BoosterHistory)
            {
                historyItem.ApplySpeedMultiplier(multiplier);
            }
        }

        /// <summary>
        /// Add a new Booster Tank
        /// </summary>
        /// <param name="boosterTank"></param>
        internal static void AddBoosterTank(SuitBoosterTank boosterTank)
        {
            BoosterTankHistoryItem newBoosterTankItem = new BoosterTankHistoryItem(boosterTank);
            BoosterHistory.Add(newBoosterTankItem);
        }

        /// <summary>
        /// Remove a Booster Tank
        /// </summary>
        /// <param name="boosterTank"></param>
        internal static void RemoveBoosterTank(SuitBoosterTank boosterTank)
        {
            foreach (BoosterTankHistoryItem boosterHistoryItem in BoosterHistory.ToList())
            {
                if (boosterHistoryItem.BoosterInstance == boosterTank)
                {
                    BoosterHistory.Remove(boosterHistoryItem);
                }
            }
        }
    }
}