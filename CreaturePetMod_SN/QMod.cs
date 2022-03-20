using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;
using Logger = QModManager.Utility.Logger;
using System.Collections.Generic;
using ECCLibrary;

namespace CreaturePetMod_SN
{
    /// <summary>
    /// Used to allow the player a choice of pet to spawn
    /// </summary>
    public enum PetCreatureType { DypPenguin };

    // Some pet names to choose from
    public enum PetNames { Anise, Beethoven, Bitey, Buddy, Cheerio, Clifford, Denali, Fuzzy, Gandalf, Hera, Jasper, Juju, Kovu, Lassie, Lois, Meera, Mochi, Oscar, Picasso, Ranger, Sampson, Shadow, Sprinkles, Stinky, Tobin, Wendy, Zola };

    /// <summary>
    /// This is our core Patching class
    /// </summary>
    [QModCore]
    public static class QMod
    {
        /// <summary>
        /// Config instance, to manage the QMod menu settings
        /// </summary>

        // Maintain our custom config
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        // Static references for our new Pet types and Asset Bundles
        public static AssetBundle dypAssetBundle;
        public static DypThePenguin dypThePenguin;
        public static TechType dypTechType;

        // Maintain a global list of PrefabIds for spawned pets. This allows us to load and save
        internal static HashSet<PetDetails> PetDetailsHashSet = new HashSet<PetDetails>();

        // Maintain a custom save file
        [FileName("pet_creatures")]
        internal class SaveData : SaveDataCache
        {
            public HashSet<PetDetails> PetDetailsHashSet { get; set; }
        }

        /// <summary>
        /// Main patch method and save game management
        /// </summary>
        [QModPatch]
        public static void Patch()
        {
            // Register our new Pets as AssetBundles
            dypAssetBundle = ECCHelpers.LoadAssetBundleFromAssetsFolder(Assembly.GetExecutingAssembly(), "dypassetbundle");
            ECCAudio.RegisterClips(dypAssetBundle);

            dypThePenguin = new DypThePenguin("Dyp", "Dyp", "Dyp the cute penguin", dypAssetBundle.LoadAsset<GameObject>("DypPrefab"), null);
            dypThePenguin.Patch();
            dypTechType = dypThePenguin.TechType;

            // Call harmony patching
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string id = "mroshaw_" + executingAssembly.GetName().Name;
            Logger.Log(Logger.Level.Info, "Patching " + id);
            new Harmony(id).PatchAll(executingAssembly);
            Logger.Log(Logger.Level.Info, "Patched successfully!");

            // Setup the save and load systesm
            Logger.Log(Logger.Level.Info, $"Setting up save and load");
            ConfigSaveData();
            Logger.Log(Logger.Level.Info, $"All done!");
        }
        
        /// <summary>
        /// Setup and configure data and load of custom config
        /// </summary>
        internal static void ConfigSaveData()
        {
            // Configure the SaveData
            SaveData saveData = SaveDataHandler.Main.RegisterSaveDataCache<SaveData>();

            saveData.OnFinishedLoading += (object sender, JsonFileEventArgs e) =>
            {
                SaveData data = e.Instance as SaveData;
                PetDetailsHashSet = data.PetDetailsHashSet;
                if (PetDetailsHashSet == null)
                {
                    PetDetailsHashSet = new HashSet<PetDetails>();
                }
                Logger.Log(Logger.Level.Info, $"Loaded PrefabKeys: {data.PetDetailsHashSet}");
            };

            // Save the Pet Prefab list
            saveData.OnStartedSaving += (object sender, JsonFileEventArgs e) =>
            {
                SaveData data = e.Instance as SaveData;
                data.PetDetailsHashSet = PetDetailsHashSet;
            };

            // Log success
            saveData.OnFinishedSaving += (object sender, JsonFileEventArgs e) =>
            {
                SaveData data = e.Instance as SaveData;
                Logger.Log(Logger.Level.Info, $"Saved PrefabKeys: {data.PetDetailsHashSet.ToString()}");
            };
        }
    }

    /// <summary>
    /// Setup the mod menu
    /// </summary>
    [Menu("Creature Pet Mod")]
    public class Config : ConfigFile
    {
        /// <summary>
        /// Allow toggling of various options
        /// </summary>
        // Key press binding to spawn the selected pet
        [Keybind("Spawn pet key")]
        public KeyCode SpawnPetKey = KeyCode.End;

        // Allow selection of custom pet
        [Choice("Pet to spawn", "Dyp The Penguin", "Dog Knight")]
        public PetCreatureType ChoiceOfPet = PetCreatureType.DypPenguin;

        // Choice of pet names
        [Choice("Pet name")]
        public PetNames PetName = PetNames.Buddy;

        // Kill all pets
        [Button("Kill all pets - USE WITH CAUTION!")]
        public void KillAllButtonClicked()
        {
            Logger.Log(Logger.Level.Debug, $"Kill all button pressed!");
            PetUtils.KillAllPets();
        }
    }
}
