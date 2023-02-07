using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace DaftAppleGames.PrawnSuitRepairAndCharge_BZ
{
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class PrawnSuitRepairAndChargePluginBz : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.prawnsuitrepairandchargemodbz";
        private const string PluginName = "Prawn Suit Repair And Charge Mod BZ";
        private const string VersionString = "2.1.0";

        // Config properties
        private const string EnableInMoonpoolConfigKey = "Enable in Moonpool";
        private const string EnableInSeaTruckConfigKey = "Enable on SeaTruck";
        private const string ConsumeSeaTruckPowerConfigKey = "Consume SeaTruck Power";
        private const string SeatruckPowerUseChargeModifierConfigKey = "SeaTruck Power Use Modifier";
        private const string SeatruckPowerUseRepairModifierConfigKey = "SeaTruck Repair Use Modifier";

        // Static config settings
        public static ConfigEntry<bool> EnableInMoonPool;
        public static ConfigEntry<bool> EnableInSeaTruck;
        public static ConfigEntry<bool> ConsumeSeaTruckPower;
        public static ConfigEntry<float> SeaTruckPowerUseChargeModifier;
        public static ConfigEntry<float> SeaTruckPowerUseRepairModifier;

        private static readonly Harmony Harmony = new Harmony(MyGuid);

        public static ManualLogSource Log;

        private void Awake()
        {
            // Modifier config
            EnableInMoonPool = Config.Bind("General",
                EnableInMoonpoolConfigKey,
                true,
                "Enabled in Moonpool docking.");

            EnableInSeaTruck = Config.Bind("General",
                EnableInSeaTruckConfigKey,
                true,
                "Enabled on Seatruck docking.");

            ConsumeSeaTruckPower = Config.Bind("General",
                ConsumeSeaTruckPowerConfigKey,
                true,
                "Consume SeaTruck Power on charge and repair.");

            SeaTruckPowerUseChargeModifier = Config.Bind("General",
                SeatruckPowerUseChargeModifierConfigKey,
                0.5f,
                "How much relative power to draw from SeaTruck on re-charge.");

            SeaTruckPowerUseRepairModifier = Config.Bind("General",
                SeatruckPowerUseRepairModifierConfigKey,
                0.1f,
                "How much relative power to draw from SeaTruck on repair.");

            // Patch in our MOD
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }
    }
}

