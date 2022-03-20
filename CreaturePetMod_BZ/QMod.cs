using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;
using Logger = QModManager.Utility.Logger;

namespace MroshawMods.CreaturePetMod_BZ
{
    /// <summary>
    /// Used to allow the player a choice of pet to spawn
    /// </summary>
    public enum PetCreatureType { SnowstalkerBaby, PenglingBaby, PenglingAdult, Pinnicarid, Unknown }
    // SnowstalkerJuvinile

    // Some pet names to choose from
    public enum PetNames { Anise, Beethoven, Bitey, Buddy, Cheerio, Clifford, Denali, Fuzzy, Gandalf, Hera, Jasper, Juju, Kovu, Lassie, Lois, Meera, Mochi, Oscar, Picasso, Ranger, Sampson, Shadow, Sprinkles, Stinky, Tobin, Wendy, Zola }

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

        // Maintain a global list of PrefabIds for spawned pets. This allows us to load and save
        internal static HashSet<PetDetails> PetDetailsHashSet;

        // Static reference to the External Pet AssetBundles
        // public static AssetBundle dypAssetBundle;
        // public static DypThePenguin dypThePenguin;

        /// <summary>
        /// Main patch method and save game management
        /// </summary>
        [QModPatch]
        public static void Patch()
        {
            // Init HashSet
            PetDetailsHashSet = new HashSet<PetDetails>();

            // Call harmony patching
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string id = "mroshaw_" + executingAssembly.GetName().Name;
            Logger.Log(Logger.Level.Info, "Patching " + id);
            new Harmony(id).PatchAll(executingAssembly);
            Logger.Log(Logger.Level.Info, "Patched successfully!");

            // Setup the save and load system
            Logger.Log(Logger.Level.Info, "Setting up save and load");
            ConfigSaveData();
            Logger.Log(Logger.Level.Info, "All done!");
        }

        // Maintain a custom save file
        [FileName("pet_creatures")]
        private class SaveData : SaveDataCache
        {
            public static Version LatestSaveDataVersion = new Version(2, 0, 0, 0);
            public DateTime SaveDateTime;
            public Version SaveDataVersion;
            public HashSet<PetDetails> PetDetailsHashSet { get; set; }
        }

        // Instance of old save data structure. Used to try loading
        // old data structure, before converting and using the new
        [FileName("pet_creatures")]
        private class SaveDataV1 : SaveDataCache
        {
            public HashSet<PetDetailsV1> PetDetailsHashSet { get; set; }
        }


        /// <summary>
        /// Setup and configure data and load of custom config
        /// </summary>
        private static void ConfigSaveData()
        {
            // Configure the SaveData
            SaveData saveData = SaveDataHandler.Main.RegisterSaveDataCache<SaveData>();

            // Attempt to load the OLD save file format
            saveData.OnFinishedLoading += (object sender, JsonFileEventArgs e) =>
            {
                Logger.Log(Logger.Level.Info, "Loading as new save file...");
                saveData = e.Instance as SaveData;
                if (saveData.SaveDataVersion is null || saveData.SaveDataVersion < SaveData.LatestSaveDataVersion)
                {
                    Logger.Log(Logger.Level.Info, "Found old save file. Migrating...");
                    PetDetailsHashSet = MigrateSaveData(saveData);
                }
                else
                {
                    Logger.Log(Logger.Level.Info, "New save file found. No migration necessary!");
                    PetDetailsHashSet = saveData.PetDetailsHashSet;
                }
            };

            // Save the Pet Prefab list to new file structure
            saveData.OnStartedSaving += (object sender, JsonFileEventArgs e) =>
            {
                Logger.Log(Logger.Level.Info, $"Saving Pet Save File...");
                SaveData data = e.Instance as SaveData;
                data.PetDetailsHashSet = PetDetailsHashSet;
                data.SaveDataVersion = SaveData.LatestSaveDataVersion;
                data.SaveDateTime = DateTime.Now;
            };
        }

        /// <summary>
        /// Migrates old save data to new
        /// </summary>
        /// <param name="saveData"></param>
        private static HashSet<PetDetails> MigrateSaveData(SaveData saveData)
        {
            if (saveData.SaveDataVersion is null)
            {
                Logger.Log(Logger.Level.Info, "Migrating from V1...");
                SaveDataV1 saveDataV1 = new SaveDataV1();
                saveDataV1.Load();
                if (saveDataV1.PetDetailsHashSet is null)
                {
                    Logger.Log(Logger.Level.Info, "HashSet is NULL!!!!!");
                    return null;
                }
                Logger.Log(Logger.Level.Info, "Miration starting...!");
                return ConvertPetDetailsV1ToV2(saveDataV1.PetDetailsHashSet);
            }
            Logger.Log(Logger.Level.Info, "No save game migration path found!");
            return null;
        }

        /// <summary>
        /// Converts an old PetDetails HashSet to a new one
        /// </summary>
        /// <param name="oldHashSet"></param>
        /// <returns></returns>
        private static HashSet<PetDetails> ConvertPetDetailsV1ToV2(HashSet<PetDetailsV1> oldHashSet)
        {
            Logger.Log(Logger.Level.Debug, "Reading old HashSet...");
            HashSet<PetDetails> newHashSet = new HashSet<PetDetails>();
            Logger.Log(Logger.Level.Debug, $"Found {oldHashSet.Count}");
            foreach (PetDetailsV1 oldDetails in oldHashSet)
            {
                PetDetails newDetails = new PetDetails(oldDetails.PrefabId, oldDetails.PetName, PetCreatureType.Unknown);
                newHashSet.Add(newDetails);
            }
            Logger.Log(Logger.Level.Debug, $"Save file converted!");
            return newHashSet;
        }

        /// <summary>
        /// Kills ALL pets. Use with caution!
        /// </summary>
        internal static void KillAllPets()
        {
            CreaturePet[] creaturePets = GameObject.FindObjectsOfType<CreaturePet>();
            Logger.Log(Logger.Level.Debug, $"Killing {creaturePets.Length} pets");
            foreach (CreaturePet creaturePet in creaturePets)
            {
                creaturePet.Kill();
            }
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
        [Choice("Pet to spawn", "Snowstalker Baby", "Pengling Baby", "Pengling Adult", "Pinnicarid")]
        public PetCreatureType PetType = PetCreatureType.SnowstalkerBaby;

        // Choice of pet names
        [Choice("Pet name")]
        public PetNames PetName = PetNames.Buddy;

        // Time to wait before pet walks to player after petting
        [Slider("Beckon delay", 0.0f, 10.0f, DefaultValue = 2.0f)]
        public float beckonDelay = 2.0f;

        // Only allow spawning pets indoors
        [Toggle("Indoor pets only (experimental)")]
        public bool IndoorPetOnly = true;

        // Max pets per room
        [Slider("Max pets per base", 0, 30, DefaultValue = 10)]
        public int MaxPetsPerRoom = 10;

        // Kill all pets
        [Button("Kill all pets - USE WITH CAUTION!")]
        public void KillAllButtonClicked()
        {
            Logger.Log(Logger.Level.Debug, "Kill all button pressed!");
            QMod.KillAllPets();
        }
    }
}
