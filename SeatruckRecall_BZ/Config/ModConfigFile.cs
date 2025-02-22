using DaftAppleGames.SeatruckRecall_BZ.AutoPilot;
using DaftAppleGames.SeatruckRecall_BZ.DockRecaller;
using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;

namespace DaftAppleGames.SeatruckRecall_BZ
{    /// <summary>
    /// Nautilus mod config class
    /// </summary>
    [Menu("Sea Truck Recall")]
    public class ModConfigFile : ConfigFile
    {
        /// <summary>
        /// Recall Config
        /// </summary>
        [Choice("Recall Method", "Teleport", "Swim")]
        public RecallMoveMethod RecallMoveMethod = RecallMoveMethod.Teleport;

        /// <summary>
        /// Speed and movement
        /// </summary>
        [Slider("Maximum Range", Step = 10, Min = 100, Max = 2000, DefaultValue = 200), OnChange(nameof(RangeChangeHandler))]
        public int MaximumRange = 1000;

        [Slider("Transit Speed", Step = 0.1f, Format = "{0:F2}", Min = 1.0f, Max = 10.0f, DefaultValue = 5.0f), OnChange(nameof(SpeedChangeHandler))]
        public float TransitSpeed = 5.0f;

        [Slider("Rotate Speed", Step = 0.1f, Format = "{0:F2}", Min = 1.0f, Max = 40.0f, DefaultValue = 20.0f), OnChange(nameof(RotateChangeHandler))]
        public float RotateSpeed = 20.0f;

        /// <summary>
        /// Handlers for config changes
        /// </summary>
        private void RangeChangeHandler(SliderChangedEventArgs newRangeArgs)
        {
            DockRecallers.UpdateAllDockRange(newRangeArgs.Value);
        }

        private void SpeedChangeHandler(SliderChangedEventArgs newSpeedArgs)
        {
            AutoPilots.UpdateAllSpeed(newSpeedArgs.Value);
        }

        private void RotateChangeHandler(SliderChangedEventArgs newRotateArgs)
        {
            AutoPilots.UpdateAllRotation(newRotateArgs.Value);
        }
    }
}