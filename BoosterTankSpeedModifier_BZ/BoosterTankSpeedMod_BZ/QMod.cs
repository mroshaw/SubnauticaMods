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
    [Menu("Booster Tank")]
    public class Config : ConfigFile
    {
        /// <summary>
        /// Slider element for float value of the modifier. We'll allow 1.0 (unchanged) to 5.0 (lightening speed)
        /// </summary>
        [Slider("Speed modifier", Format = "{0:F2}", Min = 1.0F, Max = 10.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(BoosterModifierChanged))]
        public float BoosterTankSpeedModifier;

        /// <summary>
        /// OnChange event, for debugging for now
        /// </summary>
        private void BoosterModifierChanged(IModOptionEventArgs e)
        {
            // Update max speed on all Booster Tanks
            if (QMod.BoosterTankHistory != null)
            {
                foreach (BoosterTankHistoryItem boosterTankHistoryItem in QMod.BoosterTankHistory)
                {
                    if (boosterTankHistoryItem.BoosterInstance != null)
                    {
                        // Apply modifier
                        boosterTankHistoryItem.BoosterInstance.motor.motorForce = (boosterTankHistoryItem.MotorForce * QMod.Config.BoosterTankSpeedModifier);
                        Logger.Log(Logger.Level.Debug, $"Updated existing BoosterTank. Current MotorForce: {boosterTankHistoryItem.MotorForce} to: {boosterTankHistoryItem.MotorForce * QMod.Config.BoosterTankSpeedModifier}");
                    }
                    else
                    {
                        // Remove from list
                        QMod.BoosterTankHistory.Remove(boosterTankHistoryItem);
                    }
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
    }
}