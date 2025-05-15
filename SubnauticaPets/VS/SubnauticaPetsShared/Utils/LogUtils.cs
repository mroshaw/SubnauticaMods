using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets
{
    public enum LogArea { Main, MonoBaseParts, MonoPets, Prefabs, MonoUtils, Utilities, Patches, PetConfigUtils }

    /// <summary>
    /// Static Log methods, to standardise logging and allow for granular log areas to be reported.
    /// Driven by the "Detailed Logging" mod option
    /// </summary>
    internal static class LogUtils
    {
        /// <summary>
        /// Write to debug log, if detailed logging is enabled
        /// </summary>
        public static void LogDebug(LogArea logArea, string logEntry)
        {
            if (!ModConfig.DetailedLogging)
            {
                return;
            }

            Log.LogDebug($"{logArea}: {logEntry}");
        }

        /// <summary>
        /// Write to error log, always
        /// </summary>
        public static void LogError(LogArea logArea, string logEntry)
        {
            Log.LogError($"{logArea}: {logEntry}");
        }

        /// <summary>
        /// Write to info log, if detailed logging is enabled.
        /// </summary>
        public static void LogInfo(string logEntry)
        {
            if (ModConfig.DetailedLogging)
            {
                Log.LogInfo(logEntry);
            }
        }
    }
}