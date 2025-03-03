using Nautilus.Json;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace DaftAppleGames.PrawnSuitRepairAndCharge_BZ
{
    /// <summary>
    /// Nautilus mod config class
    /// </summary>
    [Menu("Prawn Suit Repair And Charge")]
    public class ModConfigFile : ConfigFile
    {
        /// <summary>
        /// Usage option toggles
        /// </summary>
        [Toggle("Enable In MoonPool")]
        public bool EnableInMoonPool = true;

        [Toggle("Enable In SeaTruck")]
        public bool EnableInSeaTruck = true;

        [Toggle("Consume SeaTruck Power")]
        public bool ConsumeSeaTruckPower = true;

        /// <summary>
        /// SeaTruck effects
        /// </summary>
        [Slider("SeaTruck Power Usage Modifier", Step = 0.1f, Format = "{0:F2}", Min = 0.0f, Max = 1.0f, DefaultValue = 0.01f)]
        public float SeaTruckPowerUseChargeModifier = 0.5f;

        [Slider("SeaTruck Repair Modifier", Step = 0.1f, Format = "{0:F2}", Min = 0.0f, Max = 10.0f, DefaultValue = 1.5f)]
        public float SeaTruckPowerUseRepairModifier = 0.1f;
    }
}