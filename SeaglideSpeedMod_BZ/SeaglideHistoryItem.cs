using static DaftAppleGames.SeaglideSpeedMod_BZ.SeaglideSpeedModPluginBz;

namespace DaftAppleGames.SeaglideSpeedMod_BZ
{
    /// <summary>
    /// Class to allow us to maintain a list of patches Seaglides
    /// </summary>
    internal class SeaglideHistoryItem
    {
        // Public properties
        private readonly Seaglide _seaglideInstance;
        private readonly float _originalSeaglideForce;

        internal Seaglide SeaglideInstance => _seaglideInstance;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SeaglideHistoryItem(Seaglide seaglideInstance)
        {
            _seaglideInstance = seaglideInstance;
            _originalSeaglideForce = seaglideInstance.powerGlideForce;

            ApplySpeedMultiplier(ConfigFile.SpeedBoostMultiplier);
        }

        /// <summary>
        /// Apply a multiplier to the seaglide speed
        /// </summary>
        internal void ApplySpeedMultiplier(float multiplier)
        {
            _seaglideInstance.powerGlideForce = _originalSeaglideForce * multiplier;
            Log.LogInfo($"Updated Seaglide. Force multiplier: {multiplier}, from: {_originalSeaglideForce} to: {_originalSeaglideForce * multiplier}");
        }
    }
}