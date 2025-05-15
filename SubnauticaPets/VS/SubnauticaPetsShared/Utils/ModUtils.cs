using DaftAppleGames.SubnauticaPets.Pets;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DaftAppleGames.SubnauticaPets
{
    /// <summary>
    /// Static LogUtils class for common functions
    /// </summary>
    internal static class ModUtils
    {
        // Pirate check statics
        private static string SteamApi => "steam_api64.dll";
        private static int SteamApiLength => 220000;

        private static string Folder = Environment.CurrentDirectory;

        private static readonly HashSet<string> CrackedFiles = new HashSet<string>
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
        /// Find and Kill all pets. For use to clear out all pets in case
        /// of some sort of catastrophic failure.
        /// </summary>
        public static void KillAllPets()
        {
            foreach (Pet pet in GameObject.FindObjectsOfType<Pet>())
            {
                pet.Kill();
            }
        }

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