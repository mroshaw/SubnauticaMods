using System;
using System.Collections.Generic;
using System.IO;
using BepInEx.Configuration;
using DaftAppleGames.CreaturePetMod_SN.MonoBehaviours;
using DaftAppleGames.CreaturePetMod_SN.MonoBehaviours.Pets;
using UnityEngine;
using static DaftAppleGames.CreaturePetMod_SN.CreaturePetModSnPlugin;
using Object = UnityEngine.Object;

namespace DaftAppleGames.CreaturePetMod_SN.Utils
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class ModUtils
    {
        /// <summary>
        /// Example static method to return Players current location / transform
        /// </summary>
        /// <returns></returns>
        internal static Transform GetPlayerTransform()
        {
            return Player.main.transform;
        }

        /// <summary>
        /// Updates the InputManager shortcut key
        /// </summary>
        /// <param name="newKeyboardShortcut"></param>
        internal static void UpdateSpawnKeyboardShortcut(KeyboardShortcut newKeyboardShortcut)
        {
            ModInputManager inputManager = Object.FindObjectOfType<ModInputManager>();
            if (inputManager != null)
            {
                inputManager.KeyboardShortcut = newKeyboardShortcut;
            }
            else
            {
                Log.LogDebug("UpdateSpawnKeyboardShortcut: Didn't find a ModInputManager");
            }
        }

        /// <summary>
        /// Update the PetSpawner Pet Type
        /// </summary>
        /// <param name="newPetType"></param>
        internal static void UpdatePetType(PetCreatureType newPetType)
        {
            PetSpawner petSpawner = Object.FindObjectOfType<PetSpawner>();
            if (petSpawner != null)
            {
                petSpawner.PetCreatureType = newPetType;
            }
            else
            {
                Log.LogDebug("UpdatePetType: Didn't find a PetSpawner");
            }
        }

        /// <summary>
        /// Update the PetSpawner Pet Name
        /// </summary>
        /// <param name="newPetName"></param>
        internal static void UpdatePetName(PetName newPetName)
        {
            PetSpawner petSpawner = Object.FindObjectOfType<PetSpawner>();
            if (petSpawner != null)
            {
                petSpawner.PetName = newPetName;
            }
            else
            {
                Log.LogDebug("UpdatePetName: Didn't find a PetSpawner");
            }
        }

        /// <summary>
        /// Class methods to check if game is likely a pirated version.
        /// </summary>
        internal static class PirateCheck
        {
            internal static string SteamApi => "steam_api64.dll";
            internal static int SteamApiLength => 220000;

            internal static string Folder = Environment.CurrentDirectory;

            internal static readonly HashSet<string> CrackedFiles = new HashSet<string>
            {
                "steam_api64.cdx",
                "steam_api64.ini",
                "steam_emu.ini",
                "valve.ini",
                "SmartSteamEmu.ini",
                "Subnautica_Data/Plugins/steam_api64.cdx",
                "Subnautica_Data/Plugins/steam_api64.ini",
                "Subnautica_Data/Plugins/steam_emu.ini",
                "Profile/SteamUserID.cfg",
                "Profile/Stats/Achievements.Bin",
                "launcher.bat",
                "chuj.cdx",
            };

            /// <summary>
            /// Run various checks to detect pirated version of the game
            /// </summary>
            /// <returns></returns>
            internal static bool IsPirate()
            {
                string steamDll = Path.Combine(Folder, SteamApi);
                bool steamStore = File.Exists(steamDll);
                if (steamStore)
                {
                    FileInfo fileInfo = new FileInfo(steamDll);
                    if (fileInfo.Length > SteamApiLength)
                    {
                        return true;
                    }
                }

                foreach (string file in CrackedFiles)
                {
                    if (File.Exists(Path.Combine(Folder, file)))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
