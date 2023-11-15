﻿using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using DaftAppleGames.SubnauticaPets.Utils;

namespace DaftAppleGames.SubnauticaPets.Mono.Pets
{
    /// <summary>
    /// Load and Save pet details alongside save games
    /// </summary>
    public class PetSaver : MonoBehaviour
    {
        // HashSet used for saving and loading
        public HashSet<PetDetails> PetDetailsHashSet = new();

        // List to maintain accessible set of active Pets
        public List<Pet> PetList = new List<Pet>();

        // Unity Events
        public UnityEvent RegisterPetEvent;
        public UnityEvent UnregisterPetEvent;

        public UnityEvent PetListAddEvent;
        public UnityEvent PetListRemoveEvent;

        private static readonly string SaveFileName = $"{SubnauticaPetsPlugin.PluginName}.json";
        private static readonly string SaveFileFolder = SubnauticaPetsPlugin.PluginName;
        public static Version LatestSaveDataVersion = new Version(1, 0, 0, 0);

        /// <summary>
        /// Register a new pet to the HashSet - used by save and load game
        /// </summary>
        /// <param name="pet"></param>
        public PetDetails RegisterPet(Pet pet)
        {
            PetDetailsHashSet ??= new();

            string prefabId = GetPrefabId(pet);
            if (string.IsNullOrEmpty(prefabId))
            {
                LogUtils.LogError("PetSaver: Couldn't derive the PrefabId from the Pet GameObject!");
                return null;
            }

            // Add to the HashSet
            PetDetails newPetDetails = new PetDetails(prefabId, pet.PetName, pet.PetCreatureType);
            LogUtils.LogDebug(LogArea.MonoPets, "PetSaver: Adding new Pet to HashSet...");
            LogUtils.LogDebug(LogArea.MonoPets, $"PetSaver: Is HashSet null? {PetDetailsHashSet==null}");
            PetDetailsHashSet.Add(newPetDetails);
            LogUtils.LogDebug(LogArea.MonoPets, "PetSaver: Adding new Pet to HashSet... Done.");

            // Add to Pet List
            AddPetToList(pet);

            // Call any event listeners
            RegisterPetEvent.Invoke();
            
            return newPetDetails;
        }

        /// <summary>
        /// Remove pet from the Hashset - used by save and load game
        /// </summary>
        /// <param name="pet"></param>
        public void UnregisterPet(Pet pet)
        {
            // Remove from HashSet
            PetDetailsHashSet.Remove(pet.PetSaverDetails);

            // Remove from Pet List
            RemovePetFromList(pet);

            // Call any event listeners
            UnregisterPetEvent.Invoke();
        }

        /// <summary>
        /// Add a new Pet to the active list - used by the UI
        /// </summary>
        /// <param name="pet"></param>
        public void AddPetToList(Pet pet)
        {
            PetList.Add(pet);
            LogUtils.LogDebug(LogArea.MonoPets, "PetSaver: Calling PetListAddEvent.");
            PetListAddEvent.Invoke();
        }

        /// <summary>
        /// Remove Pet from active list - used by the UI
        /// </summary>
        /// <param name="pet"></param>
        public void RemovePetFromList(Pet pet)
        {
            PetList.Remove(pet);
            LogUtils.LogDebug(LogArea.MonoPets, "PetSaver: Calling PetListRemoveEvent.");
            PetListRemoveEvent.Invoke();
        }

        /// <summary>
        /// Get the PrefabId from a pet component GameObject
        /// </summary>
        /// <param name="pet"></param>
        /// <returns></returns>
        private string GetPrefabId(Pet pet)
        {
            PrefabIdentifier prefabIdentifier = pet.GetComponent<PrefabIdentifier>();
            if (prefabIdentifier)
            {
                return prefabIdentifier.Id;
            }

            return null;
        }

        /// <summary>
        /// Return the PetDetails entry by the given prefabId
        /// or return null if not found
        /// </summary>
        /// <param name="prefabId"></param>
        /// <returns></returns>
        public PetDetails GetPetDetailsWithPrefabId(string prefabId)
        {
            foreach (PetDetails petDetails in PetDetailsHashSet)
            {
                if (petDetails.PrefabId == prefabId)
                {
                    return petDetails;
                }
            }
            return null;
        }

        /// <summary>
        /// Public method to save the HashSet to a game save file
        /// </summary>
        public void SavePetsGame()
        {
            string savePath = SaveLoadManager.GetTemporarySavePath();
            LogUtils.LogDebug(LogArea.MonoPets, $"PetSaver.SavePetGame: Found this save path: {savePath}");
            WritePetSave(savePath);
        }

        /// <summary>
        /// Public method to load the HashSet from a game save file
        /// </summary>
        public void LoadPetsGame()
        {
            string savePath = SaveLoadManager.GetTemporarySavePath();
            LogUtils.LogDebug(LogArea.MonoPets, $"PetSaver.LoadPetGame: Found this save path: {savePath}");
            ReadPetSave(savePath);
        }

        /// <summary>
        /// Save the Pets save file
        /// </summary>
        /// <param name="baseFolderLocation"></param>
        private void WritePetSave(string baseFolderLocation)
        {
            // Determine where to save the file
            LogUtils.LogDebug(LogArea.MonoPets, "PetSaver: Writing Pets save file...");
            string saveFileFolder = $"{baseFolderLocation}\\{SaveFileFolder}";

            // Create the folder if it doesn't already exist
            if (!Directory.Exists(saveFileFolder))
            {
                Directory.CreateDirectory(saveFileFolder);
            }
            string saveFile = $"{saveFileFolder}\\{SaveFileName}";

            // Create new Save Data
            SaveData newSave = new SaveData
            {
                SaveDateTime = DateTime.Now,
                SaveDataVersion = LatestSaveDataVersion,
                PetDetailsHashSet = PetDetailsHashSet
            };

            LogUtils.LogDebug(LogArea.MonoPets, $"PetSaver: Saving {saveFile}...");

            // Serialize to JSON and write the save file
            string serializedJson = JsonConvert.SerializeObject(newSave, Formatting.Indented);
            File.WriteAllText(saveFile, serializedJson);
            LogUtils.LogDebug(LogArea.MonoPets, $"PetSaver: Saved {saveFile}.");
        }

        /// <summary>
        /// Load the Pets save file
        /// </summary>
        /// <param name="baseFolderLocation"></param>
        private void ReadPetSave(string baseFolderLocation)
        {
            // Determine location from which to read the save file
            string saveFile = $"{baseFolderLocation}\\{SaveFileFolder}\\{SaveFileName}";
            LogUtils.LogDebug(LogArea.MonoPets, $"Reading Pets save file: {saveFile}...");


            // Check if the file exists and then De-serialize the JSON
            if (File.Exists(saveFile))
            {
                LogUtils.LogDebug(LogArea.MonoPets, $"PetSaver: Reading {saveFile}...");
                string serializedJson = File.ReadAllText(saveFile);
                SaveData tempSave = JsonConvert.DeserializeObject<SaveData>(serializedJson);
                PetDetailsHashSet = tempSave.PetDetailsHashSet;
            }
            else
            {
                LogUtils.LogDebug(LogArea.MonoPets, "PetSaver: No save file found.");
                PetDetailsHashSet = new HashSet<PetDetails>();
            }
        }

        /// <summary>
        /// Pets save data format
        /// </summary>
        private class SaveData
        {
            public Version LatestSaveVersion = LatestSaveDataVersion;
            public DateTime SaveDateTime = DateTime.Now;
            public Version SaveDataVersion = LatestSaveDataVersion;
            public HashSet<PetDetails> PetDetailsHashSet { get; set; }
        }

        /// <summary>
        /// Internal PetDetails class, used to store "minimum" attributes for a pet
        /// so we can serialize and deserialize for saving and loading pet data
        /// </summary>
        public class PetDetails
        {
            public string PrefabId { get; }
            public string PetName { get; set; }
            public PetCreatureType PetType { get; set; }


            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="prefabId"></param>
            /// <param name="petName"></param>
            /// <param name="petType"></param>
            public PetDetails(string prefabId, string petName, PetCreatureType petType)
            {
                PrefabId = prefabId;
                PetName = petName;
                PetType = petType;
            }

            /// <summary>
            /// Override the HashSet Equals so we don't duplicate
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return obj is PetDetails q && q.PrefabId == PrefabId && q.PetName == PetName;
            }

            /// <summary>
            /// Further supports dedupe of the Hashset
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return PrefabId.GetHashCode() ^ PetName.GetHashCode();
            }
        }
    }
}
