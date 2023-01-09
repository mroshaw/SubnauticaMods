using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace Mroshaw.PrawnSuitRepairAndCharge_BZ
{
    [BepInPlugin(myGUID, pluginName, versionString)]
    public class PrawnSuitRepairAndChargePlugin_BZ : BaseUnityPlugin
    {
        // Plugin properties
        private const string myGUID = "com.mroshaw.prawnsuitrepairandchargemodbz";
        private const string pluginName = "Prawn Suit Repair And Charge Mod BZ";
        private const string versionString = "2.0.0";

        // Config properties
        private const string enableInMoonpoolConfigKey = "Enable in Moonpool";
        private const string enableInSeaTruckConfigKey = "Enable on SeaTruck";

        // Static config settings
        public static ConfigEntry<bool> EnableInMoonPool;
        public static ConfigEntry<bool> EnableInSeaTruck;

        private static readonly Harmony harmony = new Harmony(myGUID);

        public static ManualLogSource Log;

        private void Awake()
        {
            // Modifier config
            EnableInMoonPool = Config.Bind("General",
                enableInMoonpoolConfigKey,
                true,
                "Enabled in Moonpool docking");

            EnableInSeaTruck = Config.Bind("General",
                enableInSeaTruckConfigKey,
                true,
                "Enabled on Seatruck docking");

            // Patch in our MOD
            harmony.PatchAll();
            Logger.LogInfo(pluginName + " " + versionString + " " + "loaded.");
            Log = Logger;
        }
    }
}

