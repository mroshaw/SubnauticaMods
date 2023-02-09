using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DaftAppleGames.CreaturePetMod_BZ.MonoBehaviours;
using HarmonyLib;
using Newtonsoft.Json;

namespace DaftAppleGames.CreaturePetMod_BZ.Patches
{

    /// <summary>
    /// Internal class to support loading and saving of Pet details alongside standard game saves
    /// </summary>
    internal class LoadSavePatches
    {
        private static readonly string SaveFileName = "pet_creatures.json";
        private static readonly string SaveFileFolder = "CreaturePetMod_BZ";
        public static Version LatestSaveDataVersion = new Version(2, 0, 0, 0);

        /// <summary>
        /// Hook to SAVE pets game data
        /// </summary>
        [HarmonyPatch(typeof(IngameMenu))]
        internal class IngameMenuPatch
        {
            [HarmonyPatch(nameof(IngameMenu.SaveGame))]
            [HarmonyPostfix]
            private static void SaveGame_Postfix()
            {
                string savePath = SaveLoadManager.GetTemporarySavePath();
                CreaturePetPluginBz.Log.LogDebug($"SaveGame_Prefix: Found this save path: {savePath}");
                WritePetSave(savePath);
            }
        }

        /// <summary>
        /// Hook to LOAD pets game data
        /// </summary>
        [HarmonyPatch(typeof(SaveLoadManager))]
        internal class SaveLoadManagerPatch
        {
            /// <summary>
            /// Patch "Loading"
            /// </summary>
            /// <param name="result"></param>
            /// <returns></returns>
            [HarmonyPatch(nameof(SaveLoadManager.LoadAsync), typeof(IOut<SaveLoadManager.LoadResult>))]
            [HarmonyPostfix]
            private static IEnumerator LoadAsync_Postfix(IEnumerator result)
            {
                yield return result;
                CreaturePetPluginBz.Log.LogDebug("LoadAsync done.");
                string savePath = SaveLoadManager.GetTemporarySavePath();
                CreaturePetPluginBz.Log.LogDebug($"SaveGame_Prefix: Found this save path: {savePath}");
                ReadPetSave(savePath);
            }
        }

        /// <summary>
        /// Save the Pets save file
        /// </summary>
        /// <param name="baseFolderLocation"></param>
        private static void WritePetSave(string baseFolderLocation)
        {
            // Determine where to save the file
            CreaturePetPluginBz.Log.LogDebug("Writing Pets save file...");
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
                PetDetailsHashSet = CreaturePetPluginBz.PetDetailsHashSet
            };

            CreaturePetPluginBz.Log.LogDebug($"Saving {saveFile}...");

            // Serialize to JSON and write the save file
            string serializedJson = JsonConvert.SerializeObject(newSave, Formatting.Indented);
            File.WriteAllText(saveFile, serializedJson);
            CreaturePetPluginBz.Log.LogDebug($"Saved {saveFile}.");
        }

        /// <summary>
        /// Load the Pets save file
        /// </summary>
        /// <param name="baseFolderLocation"></param>
        private static void ReadPetSave(string baseFolderLocation)
        {
            // Determine location from which to read the save file
            string saveFile = $"{baseFolderLocation}\\{SaveFileFolder}\\{SaveFileName}";
            CreaturePetPluginBz.Log.LogDebug($"Reading Pets save file: {saveFile}...");


            // Check if the file exists and then De-serialize the JSON
            if (File.Exists(saveFile))
            {
                CreaturePetPluginBz.Log.LogDebug($"Reading {saveFile}...");
                string serializedJson = File.ReadAllText(saveFile);
                SaveData tempSave = JsonConvert.DeserializeObject<SaveData>(serializedJson);
                CreaturePetPluginBz.PetDetailsHashSet = tempSave.PetDetailsHashSet;
            }
            else
            {
                CreaturePetPluginBz.Log.LogDebug("No save file found.");
                CreaturePetPluginBz.PetDetailsHashSet = new HashSet<PetDetails>();
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
    }
}
