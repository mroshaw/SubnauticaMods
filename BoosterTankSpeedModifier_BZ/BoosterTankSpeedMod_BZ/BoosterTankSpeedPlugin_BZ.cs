using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;

namespace DaftAppleGames.BoosterTankSpeedMod_BZ
{
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class BoosterTankSpeedPluginBz : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.boostertankspeedmodbz";
        private const string PluginName = "Booster Tank Speed Mod BZ";
        private const string VersionString = "2.1.0";

        // Config file / UI initialisation
        internal static ModConfigFile ConfigFile = OptionsPanelHandler.RegisterModOptions<ModConfigFile>();
        private static readonly Harmony Harmony = new Harmony(MyGuid);
        public static ManualLogSource Log;

        private void Awake()
        {
            // Patch in our MOD
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }
    }
}