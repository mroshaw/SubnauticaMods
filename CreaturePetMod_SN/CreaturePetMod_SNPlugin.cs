using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CreaturePetMod_SN.MonoBehaviours;
using CreaturePetMod_SN.Utils;
using HarmonyLib;
using UnityEngine;

namespace CreaturePetMod_SN
{
    // TODO Review this file and update to your own requirements.

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class CreaturePetMod_SNPlugin : BaseUnityPlugin
    {
        // Mod specific details. MyGUID should be unique, and follow the reverse domain pattern
        // e.g.
        // com.mynameororg.pluginname
        // Version should be a valid version string.
        // e.g.
        // 1.0.0
        private const string MyGUID = "com.mrosh.CreaturePetMod_SN";
        private const string PluginName = "CreaturePetMod_SN";
        private const string VersionString = "1.0.0";

        // Config entry key strings
        // These will appear in the config file created by BepInEx and can also be used
        // by the OnSettingsChange event to determine which setting has changed.
        public static string SpawnKeyboardShortcutKey = "Spawn Keyboard Shortcut";
        public static string PetCreatureTypeKey = "Pet Creature Type";
        public static string PetNameKey = "Pet Name";

        // Configuration entries. Static, so can be accessed directly elsewhere in code via
        public static ConfigEntry<KeyboardShortcut> SpawnKeyboardShortcutConfig;
        public static ConfigEntry<PetCreatureType> PetCreatureTypeConfig;
        public static ConfigEntry<PetName> PetNameConfig;

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        public static PetSaver Saver;

        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {
            // Keyboard shortcut setting example
            SpawnKeyboardShortcutConfig = Config.Bind("Keyboard Settings",
                SpawnKeyboardShortcutKey,
                new KeyboardShortcut(KeyCode.Keypad0, KeyCode.LeftControl));

            SpawnKeyboardShortcutConfig.SettingChanged += ConfigSettingChanged;

            PetCreatureTypeConfig = Config.Bind("Pet Settings",
                PetCreatureTypeKey,
                PetCreatureType.CaveCrawler, "Creature type to spawn as a pet");

            PetCreatureTypeConfig.SettingChanged += ConfigSettingChanged;

            PetNameConfig = Config.Bind("Pet Settings",
                PetNameKey,
                PetName.Anise, "Spawned creature's name");

            PetNameConfig.SettingChanged += ConfigSettingChanged;

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
        }

        /// <summary>
        /// Method to handle changes to configuration made by the player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigSettingChanged(object sender, System.EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            // Check if null and return
            if (settingChangedEventArgs == null)
            {
                return;
            }

            // Update Input Managers if shortcut is changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == SpawnKeyboardShortcutKey)
            {
                KeyboardShortcut newKeyboardShortcut =
                    (KeyboardShortcut)settingChangedEventArgs.ChangedSetting.BoxedValue;
                Log.LogDebug("Updating keyboard shortcut...");
                ModUtils.UpdateSpawnKeyboardShortcut(newKeyboardShortcut);
                return;

            }

            // Update Pet Spawner if Pet Creature Type is changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == PetCreatureTypeKey)
            {
                PetCreatureType newPetCreatureType = (PetCreatureType)settingChangedEventArgs.ChangedSetting.BoxedValue;
                Log.LogDebug("Updating pet type...");
                ModUtils.UpdatePetType(newPetCreatureType);
                return;
            }

            // Update Pet Spawner if Pet Name is changed
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == PetNameKey)
            {
                PetName newPetName = (PetName)settingChangedEventArgs.ChangedSetting.BoxedValue;
                Log.LogDebug("Updating pet name...");
                ModUtils.UpdatePetName(newPetName);
                return;
            }

        }
    }
}
