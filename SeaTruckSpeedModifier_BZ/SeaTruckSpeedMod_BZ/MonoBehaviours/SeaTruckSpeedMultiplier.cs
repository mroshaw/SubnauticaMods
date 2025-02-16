using DaftAppleGames.SeaTruckSpeedMod_BZ;
using UnityEngine;
using static DaftAppleGames.SeaTruckSpeedMod_BZ.SeaTruckSpeedPluginBz;

namespace SeaTruckSpeedMod_BZ.MonoBehaviours
{

    /// <summary>
    /// SeaTruck speed modifier MonoBehaviour
    /// </summary>
    internal class SeaTruckSpeedMultiplier : MonoBehaviour
    {
        private SeaTruckMotor _seaTruckMotor;

        /// <summary>
        /// Unity Start method
        /// </summary>
        public void Start()
        {
            _seaTruckMotor = GetComponent<SeaTruckMotor>();
            UpdateModifiers();
            RegisterNewSeaTruck();
        }

        /// <summary>
        /// Register this SeaTruck with our list, so changes can be applied
        /// </summary>
        private void RegisterNewSeaTruck()
        {
            float currentDrag = _seaTruckMotor.pilotingDrag;
            SeaTruckHistoryItem newSeaTruck = new SeaTruckHistoryItem(_seaTruckMotor, currentDrag);
            SeaTruckHistory.Add(newSeaTruck);
        }

        /// <summary>
        /// Update modifiers on current SeaTruck
        /// </summary>
        private void UpdateModifiers()
        {
            UpdateSpeedModifier();
            UpdateEnergyDrainModifier();
        }

        /// <summary>
        /// Sets the speed modifier
        /// </summary>
        public void UpdateSpeedModifier()
        {
            // Grab the modifier value from Config and apply to the drag coefficient
            // Get current drag
            float currentDrag = _seaTruckMotor.pilotingDrag;
            Log.LogInfo($"Current drag: {currentDrag}");

            // Get current speed modifier
            float dragModifier = BoosterSpeedMultiplierConfig.Value;

            // Apply modifier
            float newDrag = currentDrag / dragModifier;
            _seaTruckMotor.pilotingDrag = newDrag;
            SeaTruckSpeedPluginBz.Log.LogInfo($"Current drag: {currentDrag} to new drag: {newDrag}");
        }

        /// <summary>
        /// Sets the energy drain modifier
        /// </summary>
        public void UpdateEnergyDrainModifier()
        {
            float currentPowerEfficiency = _seaTruckMotor.powerEfficiencyFactor;
            Log.LogInfo($"Current power efficiency: {currentPowerEfficiency}");

            // Get current speed modifier
            float energyModifier = EnergyDrainMultiplierConfig.Value;

            // Apply modifier
            float newPowerEfficiency = currentPowerEfficiency / energyModifier;
            _seaTruckMotor.powerEfficiencyFactor = newPowerEfficiency;
            Log.LogInfo($"Current power efficiency: {currentPowerEfficiency} to new power efficiency: {newPowerEfficiency}");
        }

    }
}
