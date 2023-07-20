using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEngine;
using static CreaturePetMod_SN.CreaturePetMod_SNPlugin;

namespace CreaturePetMod_SN.MonoBehaviours
{
    /// <summary>
    /// Load and Save pet details alongside save games
    /// </summary>
    public class PetSaver : MonoBehaviour
    {
        public HashSet<PetDetails> PetDetailsHashSet = new HashSet<PetDetails>();

        private static readonly string SaveFileName = "pet_creatures.json";
        private static readonly string SaveFileFolder = "CreaturePetMod_SN";
        public static Version LatestSaveDataVersion = new Version(1, 0, 0, 0);

        /// <summary>
        /// Register a new pet to the HashSet
        /// </summary>
        /// <param name="pet"></param>
        public PetDetails RegisterPet(Pet pet)
        {
            string prefabId = GetPrefabId(pet);
            if (string.IsNullOrEmpty(prefabId))
            {
                Log.LogError("PetSaver: Couldn't derive the PrefabId from the Pet GameObject!");
                return null;
            }
            Log.LogDebug("PetSaver: Adding new Pet to HashSet...");
            PetDetails newPetDetails = new PetDetails(prefabId, pet.PetName, pet.PetCreatureType);
            PetDetailsHashSet.Add(newPetDetails);
            Log.LogDebug("PetSaver: Adding new Pet to HashSet... Done.");
            return newPetDetails;
        }

        /// <summary>
        /// Remove pet from the Hashset
        /// </summary>
        /// <param name="pet"></param>
        public void RemovePet(Pet pet)
        {
            PetDetailsHashSet.Remove(pet.PetSaverDetails);
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
            else
            {
                return null;
            }
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
            Log.LogDebug($"PetSaver.SavePetGame: Found this save path: {savePath}");
            WritePetSave(savePath);
        }

        /// <summary>
        /// Public method to load the HashSet from a game save file
        /// </summary>
        public void LoadPetsGame()
        {
            string savePath = SaveLoadManager.GetTemporarySavePath();
            Log.LogDebug($"PetSaver.LoadPetGame: Found this save path: {savePath}");
            ReadPetSave(savePath);
        }

        /// <summary>
        /// Save the Pets save file
        /// </summary>
        /// <param name="baseFolderLocation"></param>
        private void WritePetSave(string baseFolderLocation)
        {
            // Determine where to save the file
            Log.LogDebug("PetSaver: Writing Pets save file...");
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

            Log.LogDebug($"PetSaver: Saving {saveFile}...");

            // Serialize to JSON and write the save file
            string serializedJson = JsonConvert.SerializeObject(newSave, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(saveFile, serializedJson);
            Log.LogDebug($"PetSaver: Saved {saveFile}.");
        }

        /// <summary>
        /// Load the Pets save file
        /// </summary>
        /// <param name="baseFolderLocation"></param>
        private void ReadPetSave(string baseFolderLocation)
        {
            // Determine location from which to read the save file
            string saveFile = $"{baseFolderLocation}\\{SaveFileFolder}\\{SaveFileName}";
            Log.LogDebug($"Reading Pets save file: {saveFile}...");


            // Check if the file exists and then De-serialize the JSON
            if (File.Exists(saveFile))
            {
                Log.LogDebug($"PetSaver: Reading {saveFile}...");
                string serializedJson = File.ReadAllText(saveFile);
                SaveData tempSave = JsonConvert.DeserializeObject<SaveData>(serializedJson);
                PetDetailsHashSet = tempSave.PetDetailsHashSet;
            }
            else
            {
                Log.LogDebug("PetSaver: No save file found.");
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
            public string PrefabId { get; set; }
            public PetName PetName { get; set; }
            public PetCreatureType PetType { get; set; }


            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="prefabId"></param>
            /// <param name="petName"></param>
            /// <param name="petType"></param>
            public PetDetails(string prefabId, PetName petName, PetCreatureType petType)
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
