using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;

namespace DaftAppleGames.SeaglideSpeedMod_BZ
{
    /// <summary>
    /// Plugin mod to allow customization of a Seaglide Speed Modifier
    /// </summary>
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class SeaglideSpeedModPluginBz : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.seaglidespeedmodbz";
        private const string PluginName = "Seaglide Speed Mod BZ";
        private const string VersionString = "1.0.0";

        // Config properties
        private const string SeaglideSpeedModifierKey = "Seaglide Speed Modifier";

        // Static config settings
        public static ConfigEntry<float> SeaglideModifier;

        // Static tracking list of patched Seaglides to update
        internal static List<SeaglideHistoryItem> SeaglideHistory = new List<SeaglideHistoryItem>();

        // Harmony static instance
        private static readonly Harmony Harmony = new Harmony(MyGuid);

        // Static Log for mod logging
        public static ManualLogSource Log;

        private void Awake()
        {
            // Modifier config - speed
            SeaglideModifier = Config.Bind("General",
                SeaglideSpeedModifierKey,
                1.0f,
                new ConfigDescription("Seaglide speed multiplier.", new AcceptableValueRange<float>(0.0f, 10.0f)));

            // Listen for config change events
            SeaglideModifier.SettingChanged += ConfigSettingChanged;

            // Patch in our MOD and set up the static logger
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }

        /// <summary>
        /// Method to handle config setting changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigSettingChanged(object sender, System.EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;
            if (settingChangedEventArgs == null)
            {
                return;
            }

            // Speed setting changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == SeaglideSpeedModifierKey)
            {
                float newValue = (float)settingChangedEventArgs.ChangedSetting.BoxedValue;
                UpdateHistory(newValue);
            }
        }

        /// <summary>
        /// Updates all Seaglides with new parameters
        /// </summary>
        /// <param name="modifierValue"></param>
        private void UpdateHistory(float modifierValue)
        {
            // Check to see if there's anything to update
            if(SeaglideHistory == null || SeaglideHistory.Count == 0)
            {
                return;
            }

            // Go through our list of Seaglides and update them
            Logger.LogInfo($"Updating {SeaglideHistory.Count} Seaglides");
            foreach (SeaglideHistoryItem seaglideHistoryItem in SeaglideHistory)
            {
                if (seaglideHistoryItem.SeaglideInstance != null)
                {
                    // Apply modifier
                    SeaglideUtils.UpdateSeaglide(seaglideHistoryItem.SeaglideInstance, seaglideHistoryItem.SeaglideForce, modifierValue);
                }
                else
                {
                    // Remove from list if this Seaglide has since been destroyed
                    SeaglideHistory.Remove(seaglideHistoryItem);
                }
            }
        }
    }
}
