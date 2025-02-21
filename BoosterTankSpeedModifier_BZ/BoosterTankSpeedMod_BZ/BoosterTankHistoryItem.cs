using static DaftAppleGames.BoosterTankSpeedMod_BZ.BoosterTankSpeedPluginBz;

namespace DaftAppleGames.BoosterTankSpeedMod_BZ
{
    /// <summary>
    /// This class allows us to keep track of Booster Tanks that we've modded so we can dynamically
    /// change the modifier in real time.
    /// </summary>
    internal class BoosterTankHistoryItem
    {
        // Instance and corresponding settings
        private readonly SuitBoosterTank _boosterInstance;
        private readonly float _originalMotorForce;
        private readonly float _originalOxygenConsumption;

        internal SuitBoosterTank BoosterInstance => _boosterInstance;
        /// <summary>
        /// Default constructor
        /// </summary>
        internal BoosterTankHistoryItem(SuitBoosterTank boosterInstance)
        {
            Log.LogInfo($"Creating new BoosterTankHistoryItem. MotorForce: {boosterInstance.motor.motorForce}, Oxygen Usage: {boosterInstance.boostOxygenUsePerSecond}");

            _boosterInstance = boosterInstance;
            _originalMotorForce = boosterInstance.motor.motorForce;
            _originalOxygenConsumption = boosterInstance.boostOxygenUsePerSecond;

            ApplySpeedMultiplier(ConfigFile.SpeedBoostMultiplier);
            ApplyOxygenConsumptionMultiplier(ConfigFile.OxygenConsumptionMultiplier);
        }

        /// <summary>
        /// Apply a multiplier to the booster speed
        /// </summary>
        internal void ApplySpeedMultiplier(float multiplier)
        {
            _boosterInstance.motor.motorForce = _originalMotorForce * multiplier;
            Log.LogInfo($"Updated BoosterTank. Motor Force multiplier: {multiplier}, from: {_originalMotorForce} to: {_originalMotorForce * multiplier}");
        }

        /// <summary>
        /// Apply a multiplier to booster oxygen consumption
        /// </summary>
        internal void ApplyOxygenConsumptionMultiplier(float multiplier)
        {
            _boosterInstance.boostOxygenUsePerSecond = _originalOxygenConsumption * multiplier;
            Log.LogInfo($"Updated BoosterTank. Oxygen Consumption multiplier {multiplier}, from: {_originalOxygenConsumption} to: {_originalOxygenConsumption * multiplier}");
        }
    }
}