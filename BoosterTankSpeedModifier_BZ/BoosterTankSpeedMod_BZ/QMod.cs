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


namespace SeaTruckSpeedMod_BZ
{
    /// <summary>
    /// Let's us determine what's been updated in the Mod menu
    /// </summary>
    public enum ChangeType
    {
        Oxygen,
        Motor
    }

    [QModCore]
    public static class QMod
    {
        /// <summary>
        /// Here, we are setting up a instance of <see cref="Config"/>, which will automatically generate an options menu using
        /// Attributes. The values in this instance will be updated whenever the user changes the corresponding option in the menu.
        /// </summary>
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        // Maintain a list of Boosters that we've modded, to allow dynamic change using menu
        internal static List<BoosterTankHistoryItem> BoosterTankHistory = new List<BoosterTankHistoryItem>();
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
    [Menu("Booster Tank Speed Mod")]
    public class Config : ConfigFile
    {
        /// <summary>
        /// Slider element for float value of the modifiers. We'll allow 1.0 (unchanged) to 11.0 (lightening speed).
        /// </summary>
        [Slider("Speed modifier", Format = "{0:F2}", Min = 1.0F, Max = 11.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(BoosterModifierChanged))]
        public float BoosterTankSpeedModifier = 1.0F;

        /// <summary>
        /// Oxygen consumption isn't really practical over 5.0. Allow 0 to stop oxygen consumption altogether.
        /// </summary>
        [Slider("Oxygen consumption modifier", Format = "{0:F2}", Min = 0.0F, Max = 5.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(OxygenModifierChanged))]
        public float OxygenConsumptionModifier = 1.0F;

        /// <summary>
        /// OnChange event, parse through our list of modified Booster Tanks to update in place.
        /// </summary>
        private void BoosterModifierChanged(IModOptionEventArgs e)
        {
            // Update max speed on all Booster Tanks
            UpdateHistory(ChangeType.Motor, (SliderChangedEventArgs)e);
        }
        private void OxygenModifierChanged(IModOptionEventArgs e)
        {
            // Update oxygen usage on all Booster Tanks
            UpdateHistory(ChangeType.Oxygen, (SliderChangedEventArgs)e);
        }

        private void UpdateHistory(ChangeType changeType, SliderChangedEventArgs eventArgs)
        {
            // Update oxygen usage on all Booster Tanks
            if (QMod.BoosterTankHistory != null)
            {
                float modifierValue = eventArgs.Value;
                foreach (BoosterTankHistoryItem boosterTankHistoryItem in QMod.BoosterTankHistory)
                {
                    if (boosterTankHistoryItem.BoosterInstance != null)
                    {
                        if (changeType == ChangeType.Motor)
                        {
                            // Apply booster modifier
                            float currentBoostValue = boosterTankHistoryItem.MotorForce;
                            float newBoostValue = currentBoostValue * modifierValue;
                            boosterTankHistoryItem.BoosterInstance.motor.motorForce = newBoostValue;
                            Logger.Log(Logger.Level.Debug, $"Updated existing BoosterTank. Current MotorForce: {currentBoostValue} to: {newBoostValue}");
                        }
                        if (changeType == ChangeType.Oxygen)
                        {
                            // Apply oxygen modifier
                            float currentOxygenValue = boosterTankHistoryItem.OxygenConsumption;
                            float newOxygenValue = currentOxygenValue * modifierValue;
                            boosterTankHistoryItem.BoosterInstance.boostOxygenUsePerSecond = newOxygenValue;
                            Logger.Log(Logger.Level.Debug, $"Updated existing BoosterTank. Current Oxygen consumption: {currentOxygenValue} to: {newOxygenValue}");
                        }
                    }
                    else
                    {
                        // Remove from list
                        Logger.Log(Logger.Level.Debug, $"Booster tank is null. Removing from list");
                        QMod.BoosterTankHistory.Remove(boosterTankHistoryItem);
                    }
                }
            }
        }
    }
}