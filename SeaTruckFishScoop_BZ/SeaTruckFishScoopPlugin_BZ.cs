using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace DaftAppleGames.SeaTruckFishScoopMod_BZ
{
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class SeaTruckFishScoopPluginBz : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.seatruckfishscoopmodbz";
        private const string PluginName = "Sea Truck Fish Scoop Mod BZ";
        private const string VersionString = "2.0.0";

        // Config properties
        private const string EnableConfigKey = "Enable Fish Scoop";
        private const string ScoopWhileStaticKey = "Scoop While Static";
        private const string ScoopWhilePilotingKey = "Scoop While Piloting";
        private const string ToggleScoopKeyboardKey = "Scoop Toggle Keyboard Shortcut";
        private const string ReleaseAllKeyboardKey = "Release All Fish Keyboard Shortcut";

        // Static config settings
        public static ConfigEntry<bool> EnableFishScoop;
        public static ConfigEntry<bool> ScoopWhileStatic;
        public static ConfigEntry<bool> ScoopWhileNotPiloting;
        public static ConfigEntry<KeyboardShortcut> ToggleScoopKeyboardShortcut;
        public static ConfigEntry<KeyboardShortcut> ReleaseAllKeyboardShortcut;

        private static readonly Harmony Harmony = new Harmony(MyGuid);

        public static ManualLogSource Log;

        private void Awake()
        {
            // Modifier config
            EnableFishScoop = Config.Bind("General",
                EnableConfigKey,
                true,
                "Enable fish scoop.");

            ScoopWhileStatic = Config.Bind("General",
                ScoopWhileStaticKey,
                true,
                "Scoop while static.");

            ScoopWhileNotPiloting = Config.Bind("General",
                ScoopWhilePilotingKey,
                true,
                "Scoop while piloting.");

            ToggleScoopKeyboardShortcut = Config.Bind("General",
                ToggleScoopKeyboardKey,
                new KeyboardShortcut(KeyCode.Delete));

            ReleaseAllKeyboardShortcut = Config.Bind("General",
                ReleaseAllKeyboardKey,
                new KeyboardShortcut(KeyCode.Insert));

            // Patch in our MOD
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }
    }
}

