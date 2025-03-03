using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;

namespace DaftAppleGames.PrawnSuitRepairAndCharge_BZ
{
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class PrawnSuitRepairAndChargePluginBz : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.prawnsuitrepairandchargemodbz";
        private const string PluginName = "Prawn Suit Repair And Charge Mod BZ";
        private const string VersionString = "2.2.0";

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