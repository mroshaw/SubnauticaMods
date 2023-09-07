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
using Nautilus.Handlers;

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
        public static string SkipSpawnObstacleCheckKey = "Skip Spawn Obstacle Check";

        // Configuration entries. Static, so can be accessed directly elsewhere in code
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
            // Init Localisation
            LanguageHandler.RegisterLocalizationFolder();

            // Set up BepInEx config
            SetupConfigOptions();

            // Set up Nautilus config
            ModOptions modOptions = new PetModOptions();

            // Set static values
            SelectedPetName = "Dave";

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

            // Set up Databank entries
            DatabankEntries.ConfigureDataBank();

        }
        #endregion

        #region CONFIG_SETUP
        /// <summary>
        /// Set up the BepInEx config options
        /// </summary>
        private void SetupConfigOptions()
        {
            SkipSpawnObstacleCheckConfig = Config.Bind("Debug Settings",
                SkipSpawnObstacleCheckKey,
                false,
                Language.main.Get("Options_SkipCheck"));

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
