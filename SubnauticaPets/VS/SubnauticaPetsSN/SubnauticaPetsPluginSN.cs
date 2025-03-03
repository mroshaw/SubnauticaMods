using BepInEx;
using BepInEx.Logging;
using DaftAppleGames.SubnauticaPets.Pets;
using DaftAppleGames.SubnauticaPets.Prefabs;
using DaftAppleGames.SubnauticaPets.Utils;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Json;
using Nautilus.Json.Attributes;
using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DaftAppleGames.SubnauticaPets
{
    // MOD PLUGIN CLASS FOR "SUBNAUTICA"
    [BepInDependency("com.snmodding.nautilus")]
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class SubnauticaPetsPlugin : BaseUnityPlugin
    {
        // Mod specific details. MyGUID should be unique, and follow the reverse domain pattern
        private const string MyGUID = "com.daftapplegames.subnauticapets2";
        private const string PluginName = "SubnauticaPets2";
        private const string VersionString = "2.6.2";

        private static Version LatestSaveDataVersion = new Version(1, 0, 0, 0);

        internal static ManualLogSource Log = new ManualLogSource(PluginName);

        // Public PetSaver as a persistent list of active pets
        internal static PetSaver PetSaver;

        // SaveData instance for managing loading of Pet config data
        internal static HashSet<PetSaver.PetDetails> LoadedPetDetailsHashSet;

        // Keep tabs on currently selected options
        internal static TechType SelectedCreaturePetType;

        // Check an eye out for Very Naughty Boys
        internal static bool IsANaughtyBoy = ModUtils.IsPirate();

        // Mod Options Config
        internal static ModConfigFile ModConfig = OptionsPanelHandler.RegisterModOptions<ModConfigFile>();

        private static readonly Harmony Harmony = new Harmony(MyGUID);

        // For debugging
        private static Object[] _debugAssetBundle = CustomAssetBundleUtils.AllAssets;

        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {
            // Init Localisation
            LanguageHandler.RegisterLocalizationFolder();

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
                    LoadedPetDetailsHashSet = new HashSet<PetSaver.PetDetails>();
                }

                CraftData.PreparePrefabIDCache();
                PetSaver.Init();
                LogUtils.LogDebug(LogArea.Main, "Finished Loading Data... Done.");
            };
            // Apply all of our patches
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            // Sets up our static Log, so it can be used elsewhere in code.
            Log = Logger;

            // Register our new prefabs
            PetDnaPrefabs.RegisterAll();
            PetPrefabs.RegisterAll();
            CustomPetPrefabs.RegisterAll();
            PetFabricatorPrefab.Register();
            PetConsolePrefab.Register();
            PetFabricatorFragmentPrefab.Register();
            PetConsoleFragmentPrefab.Register();
        }

        [FileName("SubnauticaPets")]
        internal class SaveData : SaveDataCache
        {
            public Version LatestSaveVersion = LatestSaveDataVersion;
            public DateTime SaveDateTime = DateTime.Now;
            public Version SaveDataVersion = LatestSaveDataVersion;
            public HashSet<PetSaver.PetDetails> PetDetailsHashSet { get; set; }
        }
    }
}