using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using System.Collections.Generic;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Interfaces;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;

using UnityEngine.UI;
using Logger = QModManager.Utility.Logger;

namespace VehicleSpeedMod_BZ
{
    [QModCore]
    public static class QMod
    {
        /// <summary>
        /// Here, we are setting up a instance of <see cref="Config"/>, which will automatically generate an options menu using
        /// Attributes. The values in this instance will be updated whenever the user changes the corresponding option in the menu.
        /// </summary>
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();
        // Maintain a list of vehicles that we've modded, to allow dynamic change using menu
        internal static List<SeaTruckHistoryItem> SeaTruckHistory = new List<SeaTruckHistoryItem>();
        internal static List<SeaGlideHistoryItem> SeaGlideHistory = new List<SeaGlideHistoryItem>();
        internal static List<SnowFoxHistoryItem> SnowFoxHistory = new List<SnowFoxHistoryItem>();
        internal static List<PrawnSuitHistoryItem> PrawnSuitHistory = new List<PrawnSuitHistoryItem>();
        [QModPatch]
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var modName = ($"mroshaw_{assembly.GetName().Name}");
            Logger.Log(Logger.Level.Info, $"Patching {modName}");
            Harmony harmony = new Harmony(modName);
            harmony.PatchAll(assembly);
            Logger.Log(Logger.Level.Info, "Patched successfully!");
         }
    }
    /// <summary>
    /// Set up the Mod menu
    /// </summary>
    [Menu("Vehicle Speed Mod")]
    public class Config : ConfigFile
    {
        /// <summary>
        /// Slider element for float value of the modifier. We'll allow 1.0 (unchanged) to 5.0 (lightening speed)
        /// </summary>
        [Slider("Sea Truck Speed modifier", Format = "{0:F2}", Min = 1.0F, Max = 10.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(SeaTruckSpeedValuesChanged))]
        public float SeaTruckSpeedModifier;
        [Slider("Sea Glide Speed modifier", Format = "{0:F2}", Min = 1.0F, Max = 10.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(SeaGlideSpeedValueChanged))]
        public float SeaGlideSpeedModifier;
        [Slider("Snow Fox Speed modifier", Format = "{0:F2}", Min = 1.0F, Max = 10.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(SnowFoxValueSpeedChanged))]
        public float SnowFoxSpeedModifier;
        [Slider("Prawn Suit Speed modifier", Format = "{0:F2}", Min = 1.0F, Max = 10.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(PrawnSuitSpeedValueChanged))]
        public float PrawnSuitSpeedModifier;
        /// <summary>
        /// OnChange event, for debugging for now
        /// </summary>
        private void SeaTruckSpeedValuesChanged(IModOptionEventArgs e)
        {
            Logger.Log(Logger.Level.Debug, "Sea truck speed changed!");
            Logger.Log(Logger.Level.Debug, $"{e.Id}: {e.GetType()}");

            // Update max speed on all SeaTruckMotors
            if (QMod.SeaTruckHistory != null)
            {
                int numVehicles = 0;
                foreach (SeaTruckHistoryItem seaTruckHistoryItem in QMod.SeaTruckHistory)
                {
                    if (seaTruckHistoryItem.VehicleInstance != null)
                    {
                        // Apply modifier
                        numVehicles++;
                        seaTruckHistoryItem.VehicleInstance.pilotingDrag = (seaTruckHistoryItem.VehicleValue / QMod.Config.SeaTruckSpeedModifier);
                        Logger.Log(Logger.Level.Debug, $"Updated existing SeaTruckMotor. Current drag {seaTruckHistoryItem.VehicleValue} to new drag {seaTruckHistoryItem.VehicleValue / QMod.Config.SeaTruckSpeedModifier}");
                    }
                    else
                    {
                        // Remove from list
                        QMod.SeaTruckHistory.Remove(seaTruckHistoryItem);
                    }
                    Logger.Log(Logger.Level.Debug, $"Modified {numVehicles} seatrucks");
                }
            }
            switch (e)
            {
                case KeybindChangedEventArgs keybindChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, keybindChangedEventArgs.KeyName);
                    break;
                case ChoiceChangedEventArgs choiceChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, $"{choiceChangedEventArgs.Index}: {choiceChangedEventArgs.Value}");
                    break;
                case SliderChangedEventArgs sliderChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, sliderChangedEventArgs.Value.ToString());
                    break;
                case ToggleChangedEventArgs toggleChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, toggleChangedEventArgs.Value.ToString());
                    break;
            }
        }
        private void SeaGlideSpeedValueChanged(IModOptionEventArgs e)
        {
            Logger.Log(Logger.Level.Debug, "Sea glide speed changed!");
            Logger.Log(Logger.Level.Debug, $"{e.Id}: {e.GetType()}");

            // Update max speed on all SeaGlides
            if (QMod.SeaGlideHistory != null)
            {
                int numVehicles = 0;
                foreach (SeaGlideHistoryItem seaModHistoryItem in QMod.SeaGlideHistory)
                {
                    if (seaModHistoryItem.VehicleInstance != null)
                    {
                        numVehicles++;
                    }
                }
                Logger.Log(Logger.Level.Debug, $"Modified {numVehicles} seaglides");
            }
            switch (e)
            {
                case KeybindChangedEventArgs keybindChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, keybindChangedEventArgs.KeyName);
                    break;
                case ChoiceChangedEventArgs choiceChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, $"{choiceChangedEventArgs.Index}: {choiceChangedEventArgs.Value}");
                    break;
                case SliderChangedEventArgs sliderChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, sliderChangedEventArgs.Value.ToString());
                    break;
                case ToggleChangedEventArgs toggleChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, toggleChangedEventArgs.Value.ToString());
                    break;
            }
        }
        private void PrawnSuitSpeedValueChanged(IModOptionEventArgs e)
        {
            Logger.Log(Logger.Level.Debug, "Prawn suit speed changed!");
            Logger.Log(Logger.Level.Debug, $"{e.Id}: {e.GetType()}");

            // Update max speed on all Prawn suits
            if (QMod.PrawnSuitHistory != null)
            {
                int numVehicles = 0;
                foreach (PrawnSuitHistoryItem prawnSuitHistoryItem in QMod.PrawnSuitHistory)
                {
                    if (prawnSuitHistoryItem.VehicleInstance != null)
                    {
                        numVehicles++;
                    }
                    else
                    {
                        // Remove from list
                        QMod.PrawnSuitHistory.Remove(prawnSuitHistoryItem);
                    }
                }
                Logger.Log(Logger.Level.Debug, $"Modified {numVehicles} prawn suits");
            }
            switch (e)
            {
                case KeybindChangedEventArgs keybindChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, keybindChangedEventArgs.KeyName);
                    break;
                case ChoiceChangedEventArgs choiceChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, $"{choiceChangedEventArgs.Index}: {choiceChangedEventArgs.Value}");
                    break;
                case SliderChangedEventArgs sliderChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, sliderChangedEventArgs.Value.ToString());
                    break;
                case ToggleChangedEventArgs toggleChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, toggleChangedEventArgs.Value.ToString());
                    break;
            }
        }
        private void SnowFoxValueSpeedChanged(IModOptionEventArgs e)
        {
            Logger.Log(Logger.Level.Debug, "Snow Fox speed changed!");
            Logger.Log(Logger.Level.Debug, $"{e.Id}: {e.GetType()}");

            // Update max speed on all Prawn suits
            if (QMod.PrawnSuitHistory != null)
            {
                int numVehicles = 0;
                foreach (SnowFoxHistoryItem snowFoxHistoryItem in QMod.SnowFoxHistory)
                {
                    if (snowFoxHistoryItem.VehicleInstance != null)
                    {
                        numVehicles++;
                    }
                    else
                    {
                        // Remove from list
                        QMod.SnowFoxHistory.Remove(snowFoxHistoryItem);
                    }
                }
                Logger.Log(Logger.Level.Debug, $"Modified {numVehicles} snow foxes");

            }
            switch (e)
            {
                case KeybindChangedEventArgs keybindChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, keybindChangedEventArgs.KeyName);
                    break;
                case ChoiceChangedEventArgs choiceChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, $"{choiceChangedEventArgs.Index}: {choiceChangedEventArgs.Value}");
                    break;
                case SliderChangedEventArgs sliderChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, sliderChangedEventArgs.Value.ToString());
                    break;
                case ToggleChangedEventArgs toggleChangedEventArgs:
                    Logger.Log(Logger.Level.Debug, toggleChangedEventArgs.Value.ToString());
                    break;
            }
        }
    }
}