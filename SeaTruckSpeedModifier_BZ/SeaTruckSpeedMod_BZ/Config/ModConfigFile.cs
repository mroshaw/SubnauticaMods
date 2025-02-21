using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;

namespace DaftAppleGames.SeaTruckSpeedMod_BZ.Config
{
    /// <summary>
    /// Nautilus mod config class
    /// </summary>
    [Menu("SeaTruck Speed")]
    internal class ModConfigFile : ConfigFile
    {
        /// <summary>
        /// Drag Modifier
        /// </summary>
        [Slider("Speed Multiplier", Step = 0.1f, Format = "{0:F2}", Min = 1.0f, Max = 10.0f, DefaultValue = 2.0f), OnChange(nameof(SpeedSliderChangedHandler))]
        public float DragModifier = 2.0f;

        /// <summary>
        /// Power Efficiency Modifier
        /// </summary>
        [Slider("Energy Drain Multiplier", Step = 0.1f, Format = "{0:F2}", Min = 1.0f, Max = 10.0f, DefaultValue = 2.5f), OnChange(nameof(PowerSliderChangedHandler))]
        public float PowerEfficiencyModifier = 2.5f;


        /// <summary>
        /// Handle Drag slider changes
        /// </summary>
        private void SpeedSliderChangedHandler(SliderChangedEventArgs newDragArgs)
        {
            SeaTruckHistory.UpdateAllDrag(newDragArgs.Value);
        }

        /// <summary>
        /// Handle Power slider changes
        /// </summary>
        private void PowerSliderChangedHandler(SliderChangedEventArgs newPowerArgs)
        {
            SeaTruckHistory.UpdateAllDrag(newPowerArgs.Value);
        }
    }
}