using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using DaftAppleGames.SeaTruckSpeedMod_BZ.Config;
using Nautilus.Handlers;

namespace DaftAppleGames.SeaTruckSpeedMod_BZ
{
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class SeaTruckSpeedPluginBz : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.seatruckspeedmodbz";
        private const string PluginName = "Sea Truck Speed Mod BZ";
        private const string VersionString = "2.2.0";

        // Config file / UI initialisation
        internal static ModConfigFile ConfigFile = OptionsPanelHandler.RegisterModOptions<ModConfigFile>();
        private static readonly Harmony Harmony = new Harmony(MyGuid);
        public static ManualLogSource Log;

        /// <summary>
        /// Configure the mod
        /// </summary>
        private void Awake()
        {
            // Patch in our MOD
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }
    }
}