using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;

namespace DaftAppleGames.SeaglideSpeedMod_BZ
{
    /// <summary>
    /// Nautilus mod config class
    /// </summary>
    [Menu("Seaglide Speed")]
    internal class ModConfigFile : ConfigFile
    {
        /// <summary>
        /// Speed Boost multiplier
        /// </summary>
        [Slider("Speed Multiplier", Step = 0.1f, Format = "{0:F2}", Min = 1.0f, Max = 10.0f, DefaultValue = 1.5f), OnChange(nameof(SpeedSliderChangedHandler))]
        public float SpeedBoostMultiplier = 1.5f;

        /// <summary>
        /// Handle Speed slider changes
        /// </summary>
        private void SpeedSliderChangedHandler(SliderChangedEventArgs newSpeedArgs)
        {
            SeaglideHistory.UpdateAllSpeed(newSpeedArgs.Value);
        }
    }
}