using System.Collections;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DaftAppleGames.SubnauticaPets.BaseParts;
using HarmonyLib;
using DaftAppleGames.SubnauticaPets.CustomObjects;
using DaftAppleGames.SubnauticaPets.Pets;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Handlers;
using Nautilus.Utility;
using static OVRHaptics;

namespace DaftAppleGames.SubnauticaPets
{
    /// <summary>
    /// BepInEx Pet mod class
    /// </summary>
    [BepInDependency("com.snmodding.nautilus")]
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

        // Mod Config
        public static ModConfigFile ModConfig;

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

            // Setup mod options
            ModConfig = OptionsPanelHandler.RegisterModOptions<ModConfigFile>();

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
            StartCoroutine(InitWhenReadyAsync());
        }
        #endregion

        /// <summary>
        /// Wait for any dependent systems to initialise, then init our mod
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitWhenReadyAsync()
        {
            while (!MaterialUtils.IsReady)
            {
                yield return null;
            }

            InitMod();
        }

        /// <summary>
        /// Initialise the mod
        /// </summary>
        private void InitMod()
        {
            // Add the PetSaver component
            Saver = gameObject.AddComponent<PetSaver>();

            // Initialise Pet DNA prefabs
            PetDnaPrefab.InitPetPrefabs();

            // Initialise the Pet Buildables
            PetBuildablePrefab.InitPetBuildables();

            // Init Console and Fabricator Fragments
            PetConsoleFragmentPrefab.InitPrefab();
            PetFabricatorFragmentPrefab.InitPrefab();

            // Init the Pet Fabricator
            PetFabricatorPrefab.InitPetFabricator();

            // Init the Pet Console
            PetConsolePrefab.InitPetConsole();

            // Set up Databank entries
            PetDatabankEntries.ConfigureDataBank();
            BasePartDatabankEntries.ConfigureDataBank();
        }
    }
}
