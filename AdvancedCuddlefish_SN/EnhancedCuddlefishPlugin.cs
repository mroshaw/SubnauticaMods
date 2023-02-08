using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace DaftAppleGames.EnhancedCuddlefish_SN
{
    // Mod supports "Teleporting" a creature, and forcing a "Swim To" behaviour
    public enum RecallMoveMethod
    {
        Teleport,
        SwimTo
    };

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class EnhancedCuddlefishPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.mroshaw.enhancedcuddlefishsn";
        private const string PluginName = "Enhanced Cuddlefish Mod SN";
        private const string VersionString = "1.0.0";

        // Config entry key strings
        public static string HealthRegenModifierKey = "Health Regen Modifier";
        public static string RecallKeyboardShortcutKey = "Recall Keyboard Shortcut";
        public static string RecallMethodKey = "Recall Method";

        // Declare damage multiplier config entry
        public static ConfigEntry<float> HealthRegenModifier;
        public static ConfigEntry<KeyboardShortcut> RecallKeyboardShortcut;
        public static ConfigEntry<RecallMoveMethod> RecallMethod;

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake()
        {
            // Setup health modifier config entry
            HealthRegenModifier = Config.Bind("General",   // The section under which the option is shown
                HealthRegenModifierKey,                           // The key of the configuration option
                0.01f,                                 // The default value
                "Health Regen Modifier.");              // Description of the config value

            RecallKeyboardShortcut = Config.Bind("General",
                RecallKeyboardShortcutKey,
                new KeyboardShortcut(KeyCode.LeftControl, KeyCode.C));

            RecallMethod = Config.Bind("General",
                RecallMethodKey,
                RecallMoveMethod.Teleport,
                "Determines how the Cuddlefish will move to the player location");

            // Patch in our MOD
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;
        }
    }
}