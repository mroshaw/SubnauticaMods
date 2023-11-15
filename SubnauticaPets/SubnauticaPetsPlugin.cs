using System.Collections;
using BepInEx;
using BepInEx.Logging;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using HarmonyLib;
using DaftAppleGames.SubnauticaPets.Prefabs;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Handlers;
using Nautilus.Utility;

namespace DaftAppleGames.SubnauticaPets
{
    /// <summary>
    /// BepInEx Pet mod class
    /// </summary>
    [BepInDependency("com.snmodding.nautilus")]
    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class SubnauticaPetsPlugin : BaseUnityPlugin
    {
        #region Mod Details
        /// Mod details
#if SUBNAUTICA
        private const string MyGuid = "com.daftapplegames.subnauticapets";
#endif
#if SUBNAUTICAZERO
        private const string MyGuid = "com.daftapplegames.subnauticapetsbz";
#endif
        public const string PluginName = "SubnauticaPets";
        private const string VersionString = "2.0.0";
#endregion
        #region Mod Statics
        // Public log static so we can call logging elsewhere in the mod
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        // Keep a public static Saver for use in load and save operations
        public static PetSaver Saver;
        
        // Keep tabs on currently selected options
        public static PetCreatureType SelectedCreaturePetType;
        public static string SelectedPetName;

        // Check an eye out for Very Naughty Boys
        public static bool IsANaughtyBoy = PirateCheckUtils.IsPirate();

        // Mod Config
        public static ModConfigFile ModConfig;

#endregion
        #region Private Properties
        private static readonly Harmony Harmony = new Harmony(MyGuid);
        #endregion
        #region Unity Methods        
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
            // CreaturePetMod_SNPlugin.LogUtils.LogDebug("Debug Message to BepInEx log file");
            Log = Logger;

            // Add the PetSaver component
            Saver = gameObject.AddComponent<PetSaver>();
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
        #region Private Methods
        /// <summary>
        /// Wait for any dependent systems to initialise, then init our mod
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitWhenReadyAsync()
        {
            while (!MaterialUtils.IsReady || (Base.pieces == null || Base.pieces.Length == 0))
            {
                yield return null;
            }
            LogUtils.LogDebug(LogArea.Main, "SubnauticaPetsPlugin: Calling InitMod()...");
            InitMod();
            LogUtils.LogDebug(LogArea.Main, "SubnauticaPetsPlugin: Calling InitMod()... Done.");
        }

        /// <summary>
        /// Initialise the mod
        /// </summary>
        private void InitMod()
        {
            // Initialise Pet DNA prefabs
            LogUtils.LogDebug(LogArea.Main, "Init Pet Prefabs...");
            PetDnaPrefab.InitPetPrefabs();

            // Initialise the Pet Buildables
            LogUtils.LogDebug(LogArea.Main, "Init Pet Buildables...");
            PetBuildablePrefab.InitPetBuildables();

            // Init Console and Fabricator Fragments
            LogUtils.LogDebug(LogArea.Main, "Init Fragments...");
            PetFabricatorFragmentPrefab.InitPrefab();
            PetConsoleFragmentPrefab.InitPrefab();

            // Init the Pet Console Fabricator
            LogUtils.LogDebug(LogArea.Main, "Init Pet Console and Fabricator...");
            PetConsolePrefab.InitPetConsole();
            PetFabricatorPrefab.InitPetFabricator();
        }
        #endregion
    }
}
