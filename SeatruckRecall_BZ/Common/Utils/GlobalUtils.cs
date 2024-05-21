using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Plugin = DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.Common.Utils
{
    /// <summary>
    /// Class of global useful static mod utilities
    /// </summary>
    public static class GlobalUtils
    {
        // Pirate check fields
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
        /// Locate the named GameObject within the given Parent
        /// </summary>
        /// <returns></returns>
        public static GameObject GetNamedGameObject(GameObject parent, string childName)
        {
            foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
            {
                if (child.gameObject.name == childName)
                {
                    Plugin.Log.LogDebug($"Found {childName} GameObject...");
                    return child.gameObject;
                }
            }
            return null;
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
