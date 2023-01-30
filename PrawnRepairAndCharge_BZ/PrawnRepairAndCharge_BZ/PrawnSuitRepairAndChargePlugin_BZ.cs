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
        private const string versionString = "2.1.0";

        // Config properties
        private const string enableInMoonpoolConfigKey = "Enable in Moonpool";
        private const string enableInSeaTruckConfigKey = "Enable on SeaTruck";
        private const string consumeSeaTruckPowerConfigKey = "Consume SeaTruck Power";
        private const string seatruckPowerUseChargeModifierConfigKey = "SeaTruck Power Use Modifier";
        private const string seatruckPowerUseRepairModifierConfigKey = "SeaTruck Repair Use Modifier";

        // Static config settings
        public static ConfigEntry<bool> EnableInMoonPool;
        public static ConfigEntry<bool> EnableInSeaTruck;
        public static ConfigEntry<bool> ConsumeSeaTruckPower;
        public static ConfigEntry<float> SeaTruckPowerUseChargeModifier;
        public static ConfigEntry<float> SeaTruckPowerUseRepairModifier;

        private static readonly Harmony harmony = new Harmony(myGUID);

        public static ManualLogSource Log;

        private void Awake()
        {
            // Modifier config
            EnableInMoonPool = Config.Bind("General",
                enableInMoonpoolConfigKey,
                true,
                "Enabled in Moonpool docking.");

            EnableInSeaTruck = Config.Bind("General",
                enableInSeaTruckConfigKey,
                true,
                "Enabled on Seatruck docking.");

            ConsumeSeaTruckPower = Config.Bind("General",
                consumeSeaTruckPowerConfigKey,
                true,
                "Consume SeaTruck Power on charge and repair.");

            SeaTruckPowerUseChargeModifier = Config.Bind("General",
                seatruckPowerUseChargeModifierConfigKey,
                0.5f,
                "How much relative power to draw from SeaTruck on re-charge.");

            SeaTruckPowerUseRepairModifier = Config.Bind("General",
                seatruckPowerUseRepairModifierConfigKey,
                0.1f,
                "How much relative power to draw from SeaTruck on repair.");

            // Patch in our MOD
            harmony.PatchAll();
            Logger.LogInfo(pluginName + " " + versionString + " " + "loaded.");
            Log = Logger;
        }
    }
}

