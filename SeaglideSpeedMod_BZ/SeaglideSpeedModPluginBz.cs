using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;

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
        private const string VersionString = "1.1.0";

        // Config file / UI initialisation
        internal static ModConfigFile ConfigFile = OptionsPanelHandler.RegisterModOptions<ModConfigFile>();
        // Harmony static instance
        private static readonly Harmony Harmony = new Harmony(MyGuid);

        // Static Log for mod logging
        public static ManualLogSource Log;

        private void Awake()
        {
            // Patch in our MOD and set up the static logger
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }
    }
}