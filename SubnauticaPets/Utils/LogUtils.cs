using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Utils
{
    public enum LogArea { Main, MonoBaseParts, MonoPets, Prefabs, MonoUtils, Utilities, Patches, PetConfigUtils  }

    internal static class LogUtils
    {
        /// <summary>
        /// Write to debug log, if detailed logging is enabled
        /// </summary>
        /// <param name="logArea"></param>
        /// <param name="logEntry"></param>
        public static void LogDebug(LogArea logArea, string logEntry)
        {
            if (!ModConfig.DetailedLogging)
            {
                return;
            }

            // Check if we're set up to log this message
            if ((logArea == LogArea.MonoBaseParts && !ModConfig.LogBaseParts) ||
                (logArea == LogArea.MonoPets && !ModConfig.LogPets) ||
                (logArea == LogArea.Patches && !ModConfig.LogPatches) ||
                (logArea == LogArea.Prefabs && !ModConfig.LogPrefabs) ||
                ((logArea == LogArea.MonoUtils || logArea == LogArea.Utilities) && !ModConfig.LogUtils))
            {
                return;
            }

            if (ModConfig.DetailedLogging)
            {
                Log.LogDebug($"{logArea}: {logEntry}");
            }
        }

        /// <summary>
        /// Write to error log, always
        /// </summary>
        /// <param name="logEntry"></param>
        public static void LogError(string logEntry)
        {
            Log.LogError(logEntry);
        }

        /// <summary>
        /// Write to info log, if detailed logging is enabled.
        /// </summary>
        /// <param name="logEntry"></param>
        public static void LogInfo(string logEntry)
        {
            if (ModConfig.DetailedLogging)
            {
                Log.LogInfo(logEntry);
            }
        }
    }
}
