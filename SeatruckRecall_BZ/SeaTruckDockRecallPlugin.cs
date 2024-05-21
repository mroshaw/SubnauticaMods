using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DaftAppleGames.SeatruckRecall_BZ.Utils;
using HarmonyLib;

namespace DaftAppleGames.SeatruckRecall_BZ
{
    // Mod supports "Teleporting" a Seatruck, and forcing a an "Autopilot" behaviour
    internal enum RecallMoveMethod
    {
        Teleport,
        Smooth,
        Fixed
    };

    [BepInPlugin(MyGuid, PluginName, VersionString)]
    internal class SeaTruckDockRecallPlugin : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.seatruckrecallbz";
        private const string PluginName = "Sea Truck Recall Mod BZ";
        private const string VersionString = "1.0.0";

        // Config properties
        private const string TravelMethodKey = "Travel Method";
        private const string MaximumRangeKey = "Maximum Range";
        private const string TransitSpeedKey = "Transit Speed";
        private const string RotationSpeedKey = "Rotation Speed";

        // Static config settings
        internal static ConfigEntry<RecallMoveMethod> TravelMethod;
        internal static ConfigEntry<float> MaximumRange;
        internal static ConfigEntry<float> TransitSpeed;
        internal static ConfigEntry<float> RotationSpeed;

        private static readonly Harmony Harmony = new Harmony(MyGuid);

        internal static ManualLogSource Log;

        /// <summary>
        /// Set up the mod plugin
        /// </summary>
        private void Awake()
        {
            // Travel method for the recall process
            TravelMethod = Config.Bind("General",
                TravelMethodKey,
                RecallMoveMethod.Smooth,
                "Determines how the SeaTruck will move to the dock location. Change requires a game restart.");

            TransitSpeed = Config.Bind("General",
                TransitSpeedKey,
                5.0f,
                new ConfigDescription("The speed at which the SeaTruck will travel on autopilot.",
                    new AcceptableValueRange<float>(0.1f, 10.0f)));

            MaximumRange = Config.Bind("General",
                MaximumRangeKey,
                200.0f,
                new ConfigDescription("The maximum range of the recall function.",
                    new AcceptableValueRange<float>(10.0f, 2000.0f)));

            RotationSpeed = Config.Bind("General",
                RotationSpeedKey,
                20.0f,
                new ConfigDescription("The speed at which the SeaTruck will rotate.",
                    new AcceptableValueRange<float>(0.1f, 30.0f)));

            TransitSpeed.SettingChanged += ConfigSettingChanged;
            MaximumRange.SettingChanged += ConfigSettingChanged;
            RotationSpeed.SettingChanged += ConfigSettingChanged;

            // Patch in our MOD
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loading...");
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }

        /// <summary>
        /// Manage changes made to configuration settings at run-time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigSettingChanged(object sender, System.EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            // Check if null and return
            if (settingChangedEventArgs == null)
            {
                return;
            }

            // Speed changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == TransitSpeedKey)
            {
                ModUtils.UpdateAllAutoPilotSpeed((float)settingChangedEventArgs.ChangedSetting.BoxedValue);
            }

            // Rotation changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == RotationSpeedKey)
            {
                ModUtils.UpdateAllAutoPilotRotate((float)settingChangedEventArgs.ChangedSetting.BoxedValue);
            }

            // Range changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == MaximumRangeKey)
            {
                ModUtils.UpdateAllDockSettings((float)settingChangedEventArgs.ChangedSetting.BoxedValue);
            }
        }
    }
}