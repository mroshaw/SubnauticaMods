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
        [Choice("Recall Method (Restart Required)")]
        public RecallMoveMethod RecallMoveMethod = RecallMoveMethod.Teleport;

        /// <summary>
        /// Speed and movement
        /// </summary>
        [Slider("Maximum Range", Step = 10, Min = 100, Max = 2000, DefaultValue = 200), OnChange(nameof(RangeChangeHandler))]
        public int MaximumRange = 1000;

        /// <summary>
        /// Handlers for config changes
        /// </summary>
        private void RangeChangeHandler(SliderChangedEventArgs newRangeArgs)
        {
            AllSeaTruckDockRecallers.UpdateAllDockRange(newRangeArgs.Value);
        }
    }
}