using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using DaftAppleGames.SubnauticaPets.Mono.Utils;
using HarmonyLib;
using DaftAppleGames.SubnauticaPets.Prefabs;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Handlers;
using Nautilus.Json.Attributes;
using Nautilus.Json;
using UnityEngine;
using static FlexibleGridLayout;

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
        private const string VersionString = "2.1.0";
        private static Version LatestSaveDataVersion = new Version(1, 0, 0, 0);
        #endregion
        // Public log static so we can call logging elsewhere in the mod
        public static ManualLogSource Log = new ManualLogSource(PluginName);
        
        // Public PetSaver as a persistent list of active pets
        public static PetSaver PetSaver;

        // SaveData instance for managing loading of Pet config data
        public static HashSet<PetSaver.PetDetails> LoadedPetDetailsHashSet;

        // Keep tabs on currently selected options
        public static TechType SelectedCreaturePetType;

        // Check an eye out for Very Naughty Boys
        public static bool IsANaughtyBoy = ModUtils.IsPirate();

        // Mod Config
        public static ModConfigFile ModConfig;
        private static readonly Harmony Harmony = new Harmony(MyGuid);

        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {
            // Init Localisation
            LanguageHandler.RegisterLocalizationFolder();

            // Add PetLoadFixer for Subnautica: Below Zero

            // Create PetSaver instance
            PetSaver = gameObject.AddComponent<PetSaver>();
            SaveData saveData = SaveDataHandler.RegisterSaveDataCache<SaveData>();
            // Save the HashSet
            saveData.OnStartedSaving += (object sender, JsonFileEventArgs e) =>
            {
                LogUtils.LogDebug(LogArea.Main, "Started Saving Data...");
                SaveData data = e.Instance as SaveData;
                data.PetDetailsHashSet = PetSaver.GetPetListAsHashSet();
                LogUtils.LogDebug(LogArea.Main, "Started Saving Data... Done.");
            };

            // Load the HashSet
            saveData.OnFinishedLoading += (object sender, JsonFileEventArgs e) =>
            {
                LogUtils.LogDebug(LogArea.Main, "Finished Loading Data...");
                SaveData data = e.Instance as SaveData;
                if (data.PetDetailsHashSet != null)
                {
                    LoadedPetDetailsHashSet = data.PetDetailsHashSet;
                }
                else
                {
                    LoadedPetDetailsHashSet = new();
                }

                PetSaver.Init();
#if SUBNAUTICAZERO
                PetSaver.LoadData();
#endif

                LogUtils.LogDebug(LogArea.Main, "Finished Loading Data... Done.");
            };

            // Setup mod options
            ModConfig = OptionsPanelHandler.RegisterModOptions<ModConfigFile>();

            // Apply all of our patches
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            // Sets up our static Log, so it can be used elsewhere in code.
            Log = Logger;
            PetDnaPrefabs.RegisterAll();
            PetPrefabs.RegisterAll();
            BaseModulePrefabs.RegisterAll();
            FragmentPrefabs.RegisterAll();
        }

        [FileName("SubnauticaPets")]
        public class SaveData : SaveDataCache
        {
            public Version LatestSaveVersion = LatestSaveDataVersion;
            public DateTime SaveDateTime = DateTime.Now;
            public Version SaveDataVersion = LatestSaveDataVersion;
            public HashSet<PetSaver.PetDetails> PetDetailsHashSet { get; set; }
        }
    }
}
