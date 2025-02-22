using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;

namespace DaftAppleGames.SeaTruckFishScoopMod_BZ
{
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class SeaTruckFishScoopPluginBz : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.seatruckfishscoopmodbz";
        private const string PluginName = "Sea Truck Fish Scoop Mod BZ";
        private const string VersionString = "2.1.0";

        // Config file / UI initialisation
        internal static ModConfigFile ConfigFile = OptionsPanelHandler.RegisterModOptions<ModConfigFile>();
        private static readonly Harmony Harmony = new Harmony(MyGuid);
        public static ManualLogSource Log;

        private void Awake()
        {
            // Patch in our MOD
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loading...");
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }
    }
}