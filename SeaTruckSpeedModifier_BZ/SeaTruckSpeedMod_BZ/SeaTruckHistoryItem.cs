using static DaftAppleGames.SeaTruckSpeedMod_BZ.SeaTruckSpeedPluginBz;

namespace DaftAppleGames.SeaTruckSpeedMod_BZ
{
    /// <summary>
    /// This class allows us to keep track of SeaTrucks that we've modded so we can dynamically
    /// change the modifier in real time.
    /// </summary>
    /// 
    internal class SeaTruckHistoryItem
    {
        private readonly SeaTruckMotor _seaTruckInstance;
        private readonly float _originalSeaTruckDrag;
        private readonly float _originalPowerEfficiencyFactor;

        public SeaTruckMotor SeaTruckInstance => _seaTruckInstance;

        public SeaTruckHistoryItem(SeaTruckMotor truckInstance)
        {
            _seaTruckInstance = truckInstance;
            _originalSeaTruckDrag = truckInstance.pilotingDrag;
            _originalPowerEfficiencyFactor = truckInstance.powerEfficiencyFactor;
        }

        /// <summary>
        /// Apply a multiplier to the seatruck speed
        /// </summary>
        internal void ApplyDragdModifier(float modifier)
        {
            _seaTruckInstance.pilotingDrag = _originalSeaTruckDrag / modifier;
            Log.LogInfo($"Updated SeaTruck. Drag modifier: {modifier}, from: {_originalSeaTruckDrag} to: {_originalSeaTruckDrag / modifier}");
        }
        /// <summary>
        /// Apply a multiplier to the seatruck power efficiency
        /// </summary>
        internal void ApplyPowerModifier(float modifier)
        {
            _seaTruckInstance.powerEfficiencyFactor = _originalPowerEfficiencyFactor / modifier;
            Log.LogInfo($"Updated SeaTruck. Power Efficiency modifier: {modifier}, from: {_originalPowerEfficiencyFactor} to: {_originalPowerEfficiencyFactor / modifier}");
        }


    }
}