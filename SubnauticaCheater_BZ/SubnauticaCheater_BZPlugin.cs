using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace DaftAppleGames.SubnauticaCheater_BZ
{
    // TODO Review this file and update to your own requirements.

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class SubnauticaCheater_BZPlugin : BaseUnityPlugin
    {
        // Mod specific details. MyGUID should be unique, and follow the reverse domain pattern
        // e.g.
        // com.mynameororg.pluginname
        // Version should be a valid version string.
        // e.g.
        // 1.0.0
        private const string MyGUID = "com.daftapplegames.SubnauticaCheater_BZ";
        private const string PluginName = "SubnauticaCheater_BZ";
        private const string VersionString = "1.0.0";

        // Config entry key strings
        // These will appear in the config file created by BepInEx and can also be used
        // by the OnSettingsChange event to determine which setting has changed.
        public static string OxygenCheatKey = "Oxygen Cheat";
        public static string ColdCheatKey = "Cold Cheat";

        // Configuration entries. Static, so can be accessed directly elsewhere in code via
        // e.g.
        // float myFloat = SubnauticaCheater_BZPlugin.FloatExample.Value;
        // TODO Change this code or remove the code if not required.
        public static ConfigEntry<bool> OxygenCheat;
        public static ConfigEntry<bool> ColdCheat;

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {
            // Oxygen cheat setting
            OxygenCheat = Config.Bind("General",    // The section under which the option is shown
                OxygenCheatKey,                            // The key of the configuration option
                true,                            // The default value
                "Oxygen cheat toggle.");     // Acceptable range, enabled slider and validation in Configuration Manager

            // Cold cheat setting
            // TODO Change this code or remove the code if not required.
            ColdCheat = Config.Bind("General",
                ColdCheatKey,
                true,
                "");

            // Apply all of our patches
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            // Sets up our static Log, so it can be used elsewhere in code.
            // .e.g.
            // SubnauticaCheater_BZPlugin.Log.LogDebug("Debug Message to BepInEx log file");
            Log = Logger;
        }
    }
}
