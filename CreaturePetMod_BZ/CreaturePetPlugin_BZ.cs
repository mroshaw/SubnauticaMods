using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DaftAppleGames.CreaturePetMod_BZ.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace DaftAppleGames.CreaturePetMod_BZ
{
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class CreaturePetPluginBz : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.creaturepetmodbz";
        private const string PluginName = "Creature Pet Mod BZ";
        private const string VersionString = "2.0.0";

        // Config properties
        private const string SpawnPetKeyboardShortcutKey = "Spawn Pet Keyboard Shortcut";
        private const string KillAllKeyboardShortcutKey = "Kill Pets Keyboard Shortcut";
        private const string PetTypeKey = "Spawned Pet Type";
        private const string PetNameKey = "Spawned Pet Name";
        private const string BeckonDelayKey = "Beckon Delay";

        // Static config settings
        public static ConfigEntry<KeyboardShortcut> SpawnKeyboardShortcutConfig;
        public static ConfigEntry<KeyboardShortcut> KillAllKeyboardShortcutConfig;
        public static ConfigEntry<PetCreatureType> PetTypeConfig;
        public static ConfigEntry<PetCreatureName> PetNameConfig;
        public static ConfigEntry<float> BeckonDelayConfig;

        // Static HashSet of all Pet details
        internal static HashSet<PetDetails> PetDetailsHashSet = new HashSet<PetDetails>();

        private static readonly Harmony Harmony = new Harmony(MyGuid);

        public static ManualLogSource Log;

        private void Awake()
        {
            // Config - Spawn keyboard shortcut
            SpawnKeyboardShortcutConfig = Config.Bind("General",
                SpawnPetKeyboardShortcutKey,
                new KeyboardShortcut(KeyCode.End));

            // Config - Pet type
            PetTypeConfig = Config.Bind("General",
                PetTypeKey,
                PetCreatureType.PenglingBaby,
                "Type of Pet to spawn.");

            // Config - Pet name
            PetNameConfig = Config.Bind("General",
                PetNameKey,
                PetCreatureName.Buddy,
                "Name of newly spawned Pet.");

            // Confiig - Beckon delay
            BeckonDelayConfig = Config.Bind("General",
                BeckonDelayKey,
                1.0f,
                new ConfigDescription("Beckon delay in seconds.", new AcceptableValueRange<float>(0.0f, 10.0f)));

            // Config - Kill all pets keyboard shortcut
            KillAllKeyboardShortcutConfig = Config.Bind("General",
                KillAllKeyboardShortcutKey,
                new KeyboardShortcut(KeyCode.K, KeyCode.LeftControl));

            // Patch in our MOD
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loading...");
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }
    }
}

