using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;

namespace DaftAppleGames.BoosterTankSpeedMod_BZ
{
    /// <summary>
    /// Used to determine what's changed in Config during the game
    /// </summary>
    public enum ChangeType
    {
        Oxygen,
        Motor
    }

    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class BoosterTankSpeedPluginBz : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.boostertankspeedmodbz";
        private const string PluginName = "Booster Tank Speed Mod BZ";
        private const string VersionString = "2.0.0";

        // Config properties
        private const string SpeedMultiplierConfigKey = "Booster Speed Multiplier";
        private const string OxygenMultiplierConfigKey = "Oxygen Consumption Multiplier";

        // Static config settings
        public static ConfigEntry<float> BoosterSpeedMultiplier;
        public static ConfigEntry<float> OxygenConsumptionMultiplier;

        // Static tracking list of booster tanks to update
        internal static List<BoosterTankHistoryItem> BoosterTankHistory = new List<BoosterTankHistoryItem>();

        private static readonly Harmony Harmony = new Harmony(MyGuid);

        public static ManualLogSource Log;

        private void Awake()
        {
            // Modifier config - speed
            BoosterSpeedMultiplier = Config.Bind("General",
                SpeedMultiplierConfigKey,
                1.0f,
                new ConfigDescription("Booster speed multiplier.", new AcceptableValueRange<float>(0.0f, 10.0f)));

            // Modifier config - oxygen
            OxygenConsumptionMultiplier = Config.Bind("General",
                OxygenMultiplierConfigKey,
                1.0f,
                new ConfigDescription("Oxygen consumption multiplier.", new AcceptableValueRange<float>(0.0f, 10.0f)));

            // Listen for config change events
            BoosterSpeedMultiplier.SettingChanged += ConfigSettingChanged;
            OxygenConsumptionMultiplier.SettingChanged += ConfigSettingChanged;

            // Patch in our MOD
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
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
            if(settingChangedEventArgs.ChangedSetting.Definition.Key == SpeedMultiplierConfigKey)
            {
                float newValue = (float)settingChangedEventArgs.ChangedSetting.BoxedValue;
                UpdateHistory(ChangeType.Motor, newValue);
            }

            // Oxygen setting changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == OxygenMultiplierConfigKey)
            {
                float newValue = (float)settingChangedEventArgs.ChangedSetting.BoxedValue;
                UpdateHistory(ChangeType.Oxygen, newValue);
            }
        }

        /// <summary>
        /// Updates all Booster Tanks known to the mod, with the amended config values
        /// </summary>
        /// <param name="changeType"></param>
        /// <param name="modifierValue"></param>
        private void UpdateHistory(ChangeType changeType, float modifierValue)
        {
            // Update oxygen usage on all Booster Tanks
            if (BoosterTankHistory != null)
            {
                foreach (BoosterTankHistoryItem boosterTankHistoryItem in BoosterTankHistory)
                {
                    if (boosterTankHistoryItem.BoosterInstance != null)
                    {
                        if (changeType == ChangeType.Motor)
                        {
                            // Apply booster modifier
                            float currentBoostValue = boosterTankHistoryItem.MotorForce;
                            float newBoostValue = currentBoostValue * modifierValue;
                            boosterTankHistoryItem.BoosterInstance.motor.motorForce = newBoostValue;
                            Logger.LogInfo($"Updated existing BoosterTank. Current MotorForce: {currentBoostValue} to: {newBoostValue}");
                        }
                        if (changeType == ChangeType.Oxygen)
                        {
                            // Apply oxygen modifier
                            float currentOxygenValue = boosterTankHistoryItem.OxygenConsumption;
                            float newOxygenValue = currentOxygenValue * modifierValue;
                            boosterTankHistoryItem.BoosterInstance.boostOxygenUsePerSecond = newOxygenValue;
                            Logger.LogInfo($"Updated existing BoosterTank. Current Oxygen consumption: {currentOxygenValue} to: {newOxygenValue}");
                        }
                    }
                    else
                    {
                        // Remove from list
                        Logger.LogInfo("Booster tank is null. Removing from list");
                        BoosterTankHistory.Remove(boosterTankHistoryItem);
                    }
                }
            }
        }
    }
}

