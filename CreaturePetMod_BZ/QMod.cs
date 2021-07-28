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

namespace CreaturePetMod_BZ
{
    /// <summary>
    /// Used to allow the player a choice of pet to spawn
    /// </summary>
    public enum PetChoice { SnowstalkerBaby, PenglingBaby, PenglingAdult }

    /// <summary>
    /// This is our core Patching class
    /// </summary>
    [QModCore]
    public static class QMod
    {
        /// <summary>
        /// Config instance, to manage the QMod menu settings
        /// </summary>
        /// 
        // Maintain our custome config
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();
        internal static List<string> PetPrefabKeyList = new List<string>();
        internal static List<GameObject> PetList = new List<GameObject>();

        // Maintain a custom save file
        [FileName("pet_creatures")]
        internal class SaveData : SaveDataCache
        {
            public List<string> PetPrefabKeys { get; set; }
        }
       
        [QModPatch]
        public static void Patch()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string id = "mroshaw_" + executingAssembly.GetName().Name;
            Logger.Log(Logger.Level.Info, "Patching " + id);
            new Harmony(id).PatchAll(executingAssembly);
            Logger.Log(Logger.Level.Info, "Patched successfully!");

            // Configure the SaveData
            SaveData saveData = SaveDataHandler.Main.RegisterSaveDataCache<SaveData>();

            saveData.OnFinishedLoading += (object sender, JsonFileEventArgs e) =>
            {
                SaveData data = e.Instance as SaveData; // e.Instance is the instance of your SaveData stored as a JsonFile.
                                                        // We can use polymorphism to convert it back into a SaveData
                                                        // instance, and access its members, such as PlayerPosition.
                PetPrefabKeyList = data.PetPrefabKeys;
                if (PetPrefabKeyList == null)
                {
                    PetPrefabKeyList = new List<string>();
                }
                Logger.Log(Logger.Level.Info, $"Loaded PrefabKeys: {data.PetPrefabKeys}", showOnScreen: true);
            };

            // Save the Pet Prefab list
            saveData.OnStartedSaving += (object sender, JsonFileEventArgs e) =>
            {
                SaveData data = e.Instance as SaveData;
                data.PetPrefabKeys = PetPrefabKeyList;
            };

            // Simply display the position we recorded to the save file whenever the save data it is saved
            saveData.OnFinishedSaving += (object sender, JsonFileEventArgs e) =>
            {
                SaveData data = e.Instance as SaveData;
                Logger.Log(Logger.Level.Info, $"Saved PrefabKeys: {data.PetPrefabKeys.ToString()}", showOnScreen: true);
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

        // Toggle restriction to indoors only
        [Toggle("Indoor pet only")]
        public bool IndoorPetOnly = true;

        // Max pets per room
        [Slider("Max pets per base", 0, 15, DefaultValue = 5)]
        public int MaxPetsPerRoom = 5;
    }
}
