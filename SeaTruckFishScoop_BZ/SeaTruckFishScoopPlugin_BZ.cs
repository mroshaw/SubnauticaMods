using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Mroshaw.SeaTruckFishScoopMod_BZ
{
    [BepInPlugin(myGUID, pluginName, versionString)]
    public class SeaTruckFishScoopPlugin_BZ : BaseUnityPlugin
    {
        // Plugin properties
        private const string myGUID = "com.mroshaw.seatruckfishscoopmodbz";
        private const string pluginName = "Sea Truck Fish Scoop Mod BZ";
        private const string versionString = "2.0.0";

        // Config properties
        private const string enableConfigKey = "Enable Fish Scoop";
        private const string scoopWhileStaticKey = "Scoop While Static";
        private const string scoopWhilePilotingKey = "Scoop While Piloting";
        private const string toggleScoopKeyboardKey = "Scoop Toggle Keyboard Shortcut";
        private const string releaseAllKeyboardKey = "Release All Fish Keyboard Shortcut";

        // Static config settings
        public static ConfigEntry<bool> EnableFishScoop;
        public static ConfigEntry<bool> ScoopWhileStatic;
        public static ConfigEntry<bool> ScoopWhileNotPiloting;
        public static ConfigEntry<KeyboardShortcut> ToggleScoopKeyboardShortcut;
        public static ConfigEntry<KeyboardShortcut> ReleaseAllKeyboardShortcut;

        private static readonly Harmony harmony = new Harmony(myGUID);

        public static ManualLogSource Log;

        private void Awake()
        {
            // Modifier config
            EnableFishScoop = Config.Bind("General",
                enableConfigKey,
                true,
                "Enable fish scoop.");

            ScoopWhileStatic = Config.Bind("General",
                scoopWhileStaticKey,
                true,
                "Scoop while static.");

            ScoopWhileNotPiloting = Config.Bind("General",
                scoopWhilePilotingKey,
                true,
                "Scoop while piloting.");

            ToggleScoopKeyboardShortcut = Config.Bind("General",
                toggleScoopKeyboardKey,
                new KeyboardShortcut(KeyCode.Delete));

            ReleaseAllKeyboardShortcut = Config.Bind("General",
                releaseAllKeyboardKey,
                new KeyboardShortcut(KeyCode.Insert));

            // Patch in our MOD
            harmony.PatchAll();
            Logger.LogInfo(pluginName + " " + versionString + " " + "loaded.");
            Log = Logger;
        }
    }
}

