using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Options;
using System;
using UnityEngine;
using DaftAppleGames.SubnauticaPets.ConfigOptions;
using DaftAppleGames.SubnauticaPets.CustomObjects;
using DaftAppleGames.SubnauticaPets.MonoBehaviours;
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets;
using DaftAppleGames.SubnauticaPets.Utils;

namespace DaftAppleGames.SubnauticaPets
{
    /// <summary>
    /// BepInEx Pet mod class
    /// </summary>
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class SubnauticaPetsPlugin : BaseUnityPlugin
    {
        #region MOD_DETAILS
        /// Mod details
        private const string MyGuid = "com.daftapplegames.subnauticapets";
        public const string PluginName = "SubnauticaPets";
        private const string VersionString = "2.0.0";
        #endregion
        #region MOD_STATICS
        // Public log static so we can call logging elsewhere in the mod
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        // Keep a public static Saver for use in load and save operations
        public static PetSaver Saver;

        // Config entry key strings
        public static string SpawnKeyboardShortcutKey = "Spawn Keyboard Shortcut";
        public static string SpawnKeyboardShortcutModifierKey = "Spawn Keyboard Shortcut Modifier";
        public static string KillAllKeyboardShortcutKey = "Kill All Keyboard Shortcut";
        public static string KillAllKeyboardShortcutModifierKey = "Kill All Keyboard Shortcut Modifier";
        public static string PetCreatureTypeKey = "Pet Creature Type";
        public static string PetNameKey = "Pet Name";
        public static string SkipSpawnObstacleCheckKey = "Skip Spawn Obstacle Check";

        // Configuration entries. Static, so can be accessed directly elsewhere in code
        public static ConfigEntry<KeyCode> SpawnKeyboardShortcutConfig;
        public static ConfigEntry<KeyCode> SpawnKeyboardShortcutModifierConfig;
        public static ConfigEntry<KeyCode> KillAllKeyboardShortcutConfig;
        public static ConfigEntry<KeyCode> KillAllKeyboardShortcutModifierConfig;
        public static ConfigEntry<PetCreatureType> PetCreatureTypeConfig;
        public static ConfigEntry<PetName> PetNameConfig;
        public static ConfigEntry<bool> SkipSpawnObstacleCheckConfig;

        // Keep tabs on currently selected options
        public static PetCreatureType SelectedCreaturePetType;
        public static string SelectedPetName;

        // Check an eye out for Very Naughty Boys
        public static bool IsANaughtyBoy = PirateCheckUtils.IsPirate();

#endregion
        #region MOD_PRIVATE
        private static readonly Harmony Harmony = new Harmony(MyGuid);
        #endregion
        #region UNITY_SIGNALS        
        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {
            // Set up BepInEx config
            SetupConfigOptions();

            // Set up Nautilus config
            ModOptions modOptions = new PetModOptions();

            // Set static values
            SelectedPetName = PetNameConfig.Value.ToString();
            SelectedCreaturePetType = PetCreatureTypeConfig.Value;

            // Apply all of our patches
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            // Sets up our static Log, so it can be used elsewhere in code.
            // .e.g.
            // CreaturePetMod_SNPlugin.Log.LogDebug("Debug Message to BepInEx log file");
            Log = Logger;
        }

        /// <summary>
        /// Set up the PetSaver component. We can call methods from this using the static Saver property
        /// from anywhere else in the mod.
        /// </summary>
        private void Start()
        {
            // Add the PetSaver component
            Saver = gameObject.AddComponent<PetSaver>();

            // Initialise Pet DNA prefabs
            PetDnaPrefab.InitPetPrefabs();

            // Initialise the Pet Buildables
            PetBuildablePrefab.InitPetBuildables();

            // Init the Pet Fabricator
            PetFabricatorPrefab.InitPetFabricator();

            // Init the Pet Console
            PetConsolePrefab.InitPetConsole();
        }
        #endregion

        #region CONFIG_SETUP
        /// <summary>
        /// Set up the BepInEx config options
        /// </summary>
        private void SetupConfigOptions()
        {
            SpawnKeyboardShortcutConfig = Config.Bind("Keyboard Settings",
                SpawnKeyboardShortcutKey,
                KeyCode.Keypad0, "Key to press to spawn a new pet");

            SpawnKeyboardShortcutConfig.SettingChanged += ConfigSettingChanged;

            SpawnKeyboardShortcutModifierConfig = Config.Bind("Keyboard Settings",
                SpawnKeyboardShortcutModifierKey,
                KeyCode.LeftControl, "Modifier key that must pressed to trigger a pet spawn");

            SpawnKeyboardShortcutModifierConfig.SettingChanged += ConfigSettingChanged;

            KillAllKeyboardShortcutConfig = Config.Bind("Keyboard Settings",
                KillAllKeyboardShortcutKey,
                KeyCode.K, "Key to press to kill all pets");

            KillAllKeyboardShortcutConfig.SettingChanged += ConfigSettingChanged;

            KillAllKeyboardShortcutModifierConfig = Config.Bind("Keyboard Settings",
                KillAllKeyboardShortcutModifierKey,
                KeyCode.LeftControl, "Modifier key that must be pressed to kill all spawned pets"
            );

            KillAllKeyboardShortcutModifierConfig.SettingChanged += ConfigSettingChanged;

            // Creature Type enum setting
            PetCreatureTypeConfig = Config.Bind("Pet Settings",
                PetCreatureTypeKey,
#if SUBNAUTICA
                PetCreatureType.CaveCrawler, "Creature type to spawn as a pet");
#endif
#if SUBNAUTICAZERO
                PetCreatureType.PenglingBaby, "Creature type to spawn as a pet");
#endif
            PetCreatureTypeConfig.SettingChanged += ConfigSettingChanged;

            // Creature Name enum setting
            PetNameConfig = Config.Bind("Pet Settings",
                PetNameKey,
                PetName.Anise, "Spawned creature's name");

            PetNameConfig.SettingChanged += ConfigSettingChanged;

            SkipSpawnObstacleCheckConfig = Config.Bind("Debug Settings",
                SkipSpawnObstacleCheckKey,
                false,
                "Skip the check for obstacles when spawning pets.");

            SkipSpawnObstacleCheckConfig.SettingChanged += ConfigSettingChanged;
        }

        /// <summary>
        /// Method to handle changes to BepInEx configuration made by the player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigSettingChanged(object sender, EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            // Check if null and return
            if (settingChangedEventArgs == null)
            {
                return;
            }

            // Update Input Managers if Spawn shortcut is changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == SpawnKeyboardShortcutKey)
            {
                KeyCode newKeyboardShortcut =
                    (KeyCode)settingChangedEventArgs.ChangedSetting.BoxedValue;
                Log.LogDebug("Updating keyboard shortcut...");
                ModUtils.UpdateSpawnKeyboardShortcut(newKeyboardShortcut);
                return;

            }

            // Update Input Managers if Spawn shortcut modifier is changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == SpawnKeyboardShortcutModifierKey)
            {
                KeyCode newKeyboardShortcut =
                    (KeyCode)settingChangedEventArgs.ChangedSetting.BoxedValue;
                Log.LogDebug("Updating Spawn keyboard modifier shortcut...");
                ModUtils.UpdateSpawnKeyboardModifierShortcut(newKeyboardShortcut);
                return;

            }
            
            // Update Input Managers if Kill All shortcut is changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == KillAllKeyboardShortcutKey)
            {
                KeyCode newKeyboardShortcut =
                    (KeyCode)settingChangedEventArgs.ChangedSetting.BoxedValue;
                Log.LogDebug("Updating Kill All keyboard shortcut...");
                ModUtils.UpdateKillAllKeyboardShortcut(newKeyboardShortcut);
                return;

            }

            // Update Input Managers if Kill All modifier shortcut is changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == KillAllKeyboardShortcutModifierKey)
            {
                KeyCode newKeyboardShortcut =
                    (KeyCode)settingChangedEventArgs.ChangedSetting.BoxedValue;
                Log.LogDebug("Updating Kill All keyboard modifier shortcut...");
                ModUtils.UpdateKillAllKeyboardModifierShortcut(newKeyboardShortcut);
                return;

            }

            // Update Pet Spawner if Pet Creature Type is changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == PetCreatureTypeKey)
            {
                PetCreatureType newPetCreatureType = (PetCreatureType)settingChangedEventArgs.ChangedSetting.BoxedValue;
                Log.LogDebug("Updating pet type...");
                SelectedCreaturePetType = newPetCreatureType;
                return;
            }

            // Update Pet Spawner if Pet Name is changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == PetNameKey)
            {
                PetName newPetName = (PetName)settingChangedEventArgs.ChangedSetting.BoxedValue;
                Log.LogDebug("Updating pet name...");
                SelectedPetName = newPetName.ToString();
            }

            // Update Skip Obstacle check
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == SkipSpawnObstacleCheckKey)
            {
                bool newValue = (bool)settingChangedEventArgs.ChangedSetting.BoxedValue;
                Log.LogDebug("Updating skip obstacle check...");
                ModUtils.UpdateSkipObstacleCheck(newValue);
            }
        }
#endregion
    }
}
