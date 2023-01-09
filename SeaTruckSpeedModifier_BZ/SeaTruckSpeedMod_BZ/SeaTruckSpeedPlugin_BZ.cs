using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;

namespace Mroshaw.SeaTruckSpeedMod_BZ
{
    [BepInPlugin(myGUID, pluginName, versionString)]
    public class SeaTruckSpeedPlugin_BZ : BaseUnityPlugin
    {
        // Plugin properties
        private const string myGUID = "com.mroshaw.seatruckspeedmodbz";
        private const string pluginName = "Sea Truck Speed Mod BZ";
        private const string versionString = "2.0.0";

        // Config properties
        private const string speedMultiplierConfigKey = "Speed Multiplier";

        // Static config settings
        public static ConfigEntry<float> BoosterSpeedMultiplier;

        // Static tracking list of booster tanks to update
        internal static List<SeaTruckHistoryItem> SeaTruckHistory = new List<SeaTruckHistoryItem>();

        private static readonly Harmony harmony = new Harmony(myGUID);

        public static ManualLogSource Log;

        private void Awake()
        {
            // Modifier config - speed
            BoosterSpeedMultiplier = Config.Bind("General",
                speedMultiplierConfigKey,
                1.0f,
                new ConfigDescription("SeaTruck speed multiplier.", new AcceptableValueRange<float>(0.0f, 10.0f)));

            // Listen for config change events
            BoosterSpeedMultiplier.SettingChanged += ConfigSettingChanged;

            // Patch in our MOD
            harmony.PatchAll();
            Logger.LogInfo(pluginName + " " + versionString + " " + "loaded.");
            Log = Logger;
        }

        /// <summary>
        /// Oxygen Consumption Modifier config was changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigSettingChanged(object sender, System.EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            // Speed setting changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == speedMultiplierConfigKey)
            {
                float newValue = (float)settingChangedEventArgs.ChangedSetting.BoxedValue;
                UpdateHistory(newValue);
            }
        }

        /// <summary>
        /// Updates all Booster Tanks known to the mod, with the amended config values
        /// </summary>
        /// <param name="changeType"></param>
        /// <param name="modifierValue"></param>
        private void UpdateHistory(float modifierValue)
        {
             // Update max speed on all SeaTruckMotors
            if (SeaTruckHistory != null)
            {
                Logger.LogInfo($"Updating {SeaTruckHistory.Count} SeaTruckMotors");
                foreach (SeaTruckHistoryItem seaTruckHistoryItem in SeaTruckHistory)
                {
                    if (seaTruckHistoryItem.SeaTruckInstance != null)
                    {
                        // Apply modifier
                        float currentDrag = seaTruckHistoryItem.SeaTruckDrag;
                        float newDrag = currentDrag / modifierValue;
                        seaTruckHistoryItem.SeaTruckInstance.pilotingDrag = (newDrag);
                        Logger.LogInfo($"Updated existing SeaTruckMotor. Current drag: {currentDrag} to new drag: {newDrag}");
                    }
                    else
                    {
                        // Remove from list
                        SeaTruckHistory.Remove(seaTruckHistoryItem);
                    }
                }
            }          
        }
    }
}

