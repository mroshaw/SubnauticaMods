using UnityEngine;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    public enum LogArea { Main, MonoBaseParts, MonoPets, Prefabs, MonoUtils, Utilities, Patches, PetConfigUtils }

    internal static class LogUtils
    {
        /// <summary>
        /// Write to debug log, if detailed logging is enabled
        /// </summary>
        public static void LogDebug(LogArea logArea, string logEntry)
        {
            Debug.Log($"{logArea}: {logEntry}");
        }

        /// <summary>
        /// Write to error log, always
        /// </summary>
        public static void LogError(LogArea logArea, string logEntry)
        {
            Debug.LogError($"{logArea}: {logEntry}");
        }

        /// <summary>
        /// Write to info log, if detailed logging is enabled.
        /// </summary>
        public static void LogInfo(string logEntry, LogArea logArea)
        {
            Debug.Log($"{logArea}: {logEntry}");
        }
    }
}
