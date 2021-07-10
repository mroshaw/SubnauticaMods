using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using System.Collections.Generic;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Interfaces;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;
using Logger = QModManager.Utility.Logger;

namespace SeaTruckSpeedMod_BZ
{
    /// <summary>
    /// Keep tabs on what we're changing in QMod menus
    /// </summary>
    public enum ModifierType
    {
        SpeedModifier
    }

    [QModCore]
    public static class QMod
    {
        /// <summary>
        /// Set up the config for the mod menu
        /// </summary>
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        // Maintain a list of SeaTrucks that we've modded, to allow dynamic change using menu
        internal static List<SeaTruckHistoryItem> SeaTruckHistory = new List<SeaTruckHistoryItem>();

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
    [Menu("Sea Truck Speed Mod")]
    public class Config : ConfigFile
    {
        /// <summary>
        /// Slider element for float value of the modifier. We'll allow 1.0 (unchanged) to 11.0 (lightening speed)
        /// </summary>
        [Slider("Speed modifier", Format = "{0:F2}", Min = 1.0F, Max = 11.0F, DefaultValue = 1.0F, Step = 0.1F), OnChange(nameof(SpeedModifierChanged))]
        public float SeaTruckSpeedModifier;

        /// <summary>
        /// OnChange event, for debugging for now
        /// </summary>
        private void SpeedModifierChanged(IModOptionEventArgs e)
        {
            UpdateAllSeaTrucks(ModifierType.SpeedModifier, ((SliderChangedEventArgs)e).Value);
        }

        private void UpdateAllSeaTrucks(ModifierType modifierType, float modifierValue)
        {
            // Update max speed on all SeaTruckMotors
            if (QMod.SeaTruckHistory != null)
            {
                Logger.Log(Logger.Level.Debug, $"Updating {QMod.SeaTruckHistory.Count} SeaTruckMotors");
                foreach (SeaTruckHistoryItem seaTruckHistoryItem in QMod.SeaTruckHistory)
                {
                    if (seaTruckHistoryItem.SeaTruckInstance != null)
                    {
                        if (modifierType == ModifierType.SpeedModifier)
                        {
                            // Apply modifier
                            float currentDrag = seaTruckHistoryItem.SeaTruckDrag;
                            float newDrag = currentDrag / modifierValue;
                            seaTruckHistoryItem.SeaTruckInstance.pilotingDrag = (newDrag);
                            Logger.Log(Logger.Level.Debug, $"Updated existing SeaTruckMotor. Current drag: {currentDrag} to new drag: {newDrag}");
                        }
                    }
                    else
                    {
                        // Remove from list
                        QMod.SeaTruckHistory.Remove(seaTruckHistoryItem);
                    }
                }
            }
        }
    }
}