using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using SeaTruckSpeedMod_BZ.MonoBehaviours;

namespace DaftAppleGames.SeaTruckSpeedMod_BZ
{
    public enum ChangeType { Speed, EnergyDrain }

    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class SeaTruckSpeedPluginBz : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.seatruckspeedmodbz";
        private const string PluginName = "Sea Truck Speed Mod BZ";
        private const string VersionString = "2.1.0";

        // Config properties
        private const string SpeedMultiplierConfigKey = "Speed Multiplier";
        private const string EnergyDrainMultiplierConfigKey = "Energy Drain Multiplier";

        // Static config settings
        public static ConfigEntry<float> BoosterSpeedMultiplierConfig;
        public static ConfigEntry<float> EnergyDrainMultiplierConfig;

        // Static tracking list of SeaTrucks to update
        internal static List<SeaTruckHistoryItem> SeaTruckHistory = new List<SeaTruckHistoryItem>();

        private static readonly Harmony Harmony = new Harmony(MyGuid);

        public static ManualLogSource Log;

        /// <summary>
        /// Configure the mod
        /// </summary>
        private void Awake()
        {
            // Modifier config - speed
            BoosterSpeedMultiplierConfig = Config.Bind("General",
                SpeedMultiplierConfigKey,
                2.0f,
                new ConfigDescription("SeaTruck speed multiplier.", new AcceptableValueRange<float>(0.0f, 10.0f)));

            // Modifier config - energy drain
            EnergyDrainMultiplierConfig = Config.Bind("General",
                EnergyDrainMultiplierConfigKey,
                2.5f,
                new ConfigDescription("SeaTruck energy drain multiplier.", new AcceptableValueRange<float>(0.0f, 10.0f)));


            // Listen for config change events
            BoosterSpeedMultiplierConfig.SettingChanged += ConfigSettingChanged;
            EnergyDrainMultiplierConfig.SettingChanged += ConfigSettingChanged;

            // Patch in our MOD
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }

        /// <summary>
        /// Propagates any config changes to all SeaTrucks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigSettingChanged(object sender, System.EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            // Speed setting changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == SpeedMultiplierConfigKey)
            {
                float newValue = (float)settingChangedEventArgs.ChangedSetting.BoxedValue;
                UpdateHistory(ChangeType.Speed, newValue);
            }

            // Energy multiplier changed
            // Speed setting changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == EnergyDrainMultiplierConfigKey)
            {
                float newValue = (float)settingChangedEventArgs.ChangedSetting.BoxedValue;
                UpdateHistory(ChangeType.EnergyDrain, newValue);
            }
        }

        /// <summary>
        /// Updates all SeaTrucks known to the mod, with the amended config values
        /// </summary>
        /// <param name="changeType"></param>
        /// <param name="newValue"></param>
        private void UpdateHistory(ChangeType changeType, float newValue)
        {
             // Update max speed on all SeaTruckMotors
            if (SeaTruckHistory != null)
            {
                Logger.LogInfo($"Updating {SeaTruckHistory.Count} SeaTruckMotors");
                foreach (SeaTruckHistoryItem seaTruckHistoryItem in SeaTruckHistory)
                {
                    if (seaTruckHistoryItem.SeaTruckInstance != null)
                    {
                        SeaTruckSpeedMultiplier speedMultiplier =
                            seaTruckHistoryItem.SeaTruckInstance.GetComponent<SeaTruckSpeedMultiplier>();

                        if (speedMultiplier)
                        {
                            switch (changeType)
                            {
                                case ChangeType.Speed:
                                    speedMultiplier.UpdateSpeedModifier();
                                    break;

                                case ChangeType.EnergyDrain:
                                    speedMultiplier.UpdateEnergyDrainModifier();
                                    break;
                            }
                        }
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

