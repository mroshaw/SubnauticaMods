using System;
using System.Collections.Generic;
using System.IO;

/*
    Courtesy of:
    https://github.com/SubnauticaModding/QModManager/blob/Dev/QModManager/Checks/PirateCheck.cs
    https://github.com/SubnauticaModding/QModManager/blob/Dev/LICENSE.md
    All code attributed to "Subnautica Modding" and "desperationfighter"
*/

namespace DaftAppleGames.CreaturePetMod_BZ.GlobalUtils
{
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
                    CreaturePetPluginBz.Log.LogDebug($"File length check. ({fileInfo.Length})");
                    return true;
                }
            }

            foreach (string file in CrackedFiles)
            {
                if (File.Exists(Path.Combine(Folder, file)))
                {
                    CreaturePetPluginBz.Log.LogDebug("File exists check.");
                    return true;
                }
            }
            return false;
        }
    }
}