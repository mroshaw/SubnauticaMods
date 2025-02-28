using BepInEx;
using BepInEx.Logging;
using DaftAppleGames.SubnauticaCheater_BZ.Config;
using HarmonyLib;
using Nautilus.Handlers;

namespace DaftAppleGames.SubnauticaCheater_BZ
{
    // TODO Review this file and update to your own requirements.

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class SubnauticaCheater_BZPlugin : BaseUnityPlugin
    {
        // Mod  details
        private const string MyGUID = "com.daftapplegames.SubnauticaCheater_BZ";
        private const string PluginName = "SubnauticaCheater_BZ";
        private const string VersionString = "1.0.0";

        // Config file / UI initialisation
        internal static ModConfigFile ConfigFile = OptionsPanelHandler.RegisterModOptions<ModConfigFile>();
        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {
            // Apply all of our patches
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            // Sets up our static Log, so it can be used elsewhere in code.
            Log = Logger;
        }
    }
}