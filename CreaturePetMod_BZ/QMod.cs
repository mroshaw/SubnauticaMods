using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using SMLHelper.V2.Json.Attributes;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;
using System.Media;
using Logger = QModManager.Utility.Logger;
using System.Collections.Generic;
using SMLHelper.V2.Interfaces;
using SMLHelper.V2.Options;

namespace CreaturePetMod_BZ
{
    /// <summary>
    /// Used to allow the player a choice of pet to spawn
    /// </summary>
    public enum PetChoice { SnowstalkerBaby, PenglingBaby, PenglingAdult };
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
                SaveData data = e.Instance as SaveData; // e.Instance is the instance of your SaveData stored as a JsonFile.
                                                        // We can use polymorphism to convert it back into a SaveData
                                                        // instance, and access its members, such as PlayerPosition.
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

            // Simply display the position we recorded to the save file whenever the save data it is saved
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
        [Choice("Pet to spawn", "Snowstalker Baby", "Pengling Baby", "Pengling Adult")]
        public PetChoice ChoiceOfPet = PetChoice.SnowstalkerBaby;

        // Choice of pet names
        [Choice("Pet name")]
        public PetNames PetName = PetNames.Buddy;

        // Only allow spawning pets indoors
        [Toggle("Indoor pets only (experimental)")]
        public bool IndoorPetOnly = true;

        // Max pets per room
        [Slider("Max pets per base", 0, 30, DefaultValue = 10)]
        public int MaxPetsPerRoom = 10;
    }
}
