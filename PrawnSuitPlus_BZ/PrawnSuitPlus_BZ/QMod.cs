using System;
using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Commands;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Interfaces;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;

using UnityEngine;
using UnityEngine.UI;
using Logger = QModManager.Utility.Logger;

namespace PrawnSuitPlus_BZ
{
    [QModCore]
    public static class QMod
    {
        /// <summary>
        /// Here, we are setting up a instance of <see cref="Config"/>, which will automatically generate an options menu using
        /// Attributes. The values in this instance will be updated whenever the user changes the corresponding option in the menu.
        /// </summary>
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        // Maintain a list of PrawnSuits that we've modded, to allow dynamic change using menu
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
    /// Setup the mod menu
    /// </summary>
    [Menu("Prawn Suit Plus")]
    public class Config : ConfigFile
    {

        /// <summary>
        /// Allow toggling of Moonpool and Seatruck
        /// </summary>
        [Toggle("Charging Enabled in Moonpool"), OnChange(nameof(MyCheckboxToggleEvent)), OnChange(nameof(MyGenericValueChangedEvent))]
        public bool ChargingEnableMoonPool;

        [Toggle("Charging Enabled on Seatruck"), OnChange(nameof(MyCheckboxToggleEvent)), OnChange(nameof(MyGenericValueChangedEvent))]
        public bool ChargingEnableSeaTruck;

        [Toggle("Repair Enabled in Moonpool"), OnChange(nameof(MyCheckboxToggleEvent)), OnChange(nameof(MyGenericValueChangedEvent))]
        public bool RepairEnableMoonPool;

        [Toggle("Repair Enabled on Seatruck"), OnChange(nameof(MyCheckboxToggleEvent)), OnChange(nameof(MyGenericValueChangedEvent))]
        public bool RepairEnableSeaTruck;

        [Toggle("Invincible"), OnChange(nameof(MyCheckboxToggleEvent)), OnChange(nameof(MyGenericValueChangedEvent))]
        public bool Invincible;

        [Toggle("Crashfish Launcher"), OnChange(nameof(MyCheckboxToggleEvent)), OnChange(nameof(MyGenericValueChangedEvent))]
        public bool CrashFishLauncher;

        [Slider("Speed modifier", Format = "{0:F2}", Min = 1.0F, Max = 10.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(SpeedChanged))]
        public float SpeedModifier;

        [Slider("Thrust modifier", Format = "{0:F2}", Min = 1.0F, Max = 2.0F, DefaultValue = 1.0F, Step = 0.01F), OnChange(nameof(ThrustChanged))]
        public float ThrustModifier;

        [Slider("Drill damage modifier", Format = "{0:F2}", Min = 1.0F, Max = 10.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(MyGenericValueChangedEvent))]
        public float DrillArmDamageModifier;

        private void MyCheckboxToggleEvent(ToggleChangedEventArgs e)
        {

        }

        /// <summary>
        /// Generic OnChanged event, for debugging for now
        /// </summary>
         private void MyGenericValueChangedEvent(IModOptionEventArgs e)
        {
            switch (e)
            {
                case KeybindChangedEventArgs keybindChangedEventArgs:
                    break;
                case ChoiceChangedEventArgs choiceChangedEventArgs:
                    break;
                case SliderChangedEventArgs sliderChangedEventArgs:
                    break;
                case ToggleChangedEventArgs toggleChangedEventArgs:
                    break;
            }
        }
        /// <summary>
        /// Iterate across all PrawnSuits and update the Thrust modifier change
        /// </summary>
        private void ThrustChanged(SliderChangedEventArgs sliderChangedEventArgs)
        {
            // Call our helper function to iterate through the list
            Logger.Log(Logger.Level.Debug, $"Update Thrust on all PrawnSuits");
            UpdateAllPrawnSuits(ModType.SuperThrust, sliderChangedEventArgs.Value);
        }
        /// <summary>
        /// Iterate across all PrawnSuits and update the Thrust modifier change
        /// </summary>
        private void SpeedChanged(SliderChangedEventArgs sliderChangedEventArgs)
        {
            // Call our helper function to iterate through the list
            Logger.Log(Logger.Level.Debug, $"Update Speed on all PrawnSuits");
            UpdateAllPrawnSuits(ModType.SuperSpeed, sliderChangedEventArgs.Value);
        }
        private void UpdateAllPrawnSuits(ModType modType, float newValue)
        {
            if (QMod.PrawnSuitHistory != null)
            {
                int numberOfItems = 0;
                foreach (PrawnSuitHistoryItem prawnSuitHistoryItem in QMod.PrawnSuitHistory)
                {
                    if (prawnSuitHistoryItem.PrawnSuitInstance != null)
                    {
                        switch (modType)
                        {
                            case ModType.SuperThrust:
                                if (prawnSuitHistoryItem.ModType == ModType.SuperThrust)
                                {
                                    // Apply modifier
                                    numberOfItems++;
                                    prawnSuitHistoryItem.PrawnSuitInstance.thrustPower = prawnSuitHistoryItem.OriginalValue * newValue;
                                    Logger.Log(Logger.Level.Debug, $"Updated existing PrawnSuit. Current Thrust {prawnSuitHistoryItem.OriginalValue} to new Thrust {prawnSuitHistoryItem.OriginalValue / newValue}");
                                }
                                break;
                            case ModType.SuperSpeed:
                                if (prawnSuitHistoryItem.ModType == ModType.SuperSpeed)
                                {
                                    // Do SuperSpeed changes here
                                }
                                break;
                        }
                    }
                    else
                    {
                        // Remove from list if the instance is null
                        QMod.PrawnSuitHistory.Remove(prawnSuitHistoryItem);
                    }
                }
                Logger.Log(Logger.Level.Debug, $"Updated {numberOfItems} PrawnSuits.");
            }
        }
    }
}