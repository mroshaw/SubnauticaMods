

using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ
{
    // Mod supports "Teleporting" a Seatruck, and forcing a an "Autopilot" behaviour
    public enum RecallMoveMethod
    {
        Teleport,
        Smooth,
        Fixed
    };


    internal class SeaTruckDockRecallPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.seatruckrecallbz";
        private const string PluginName = "Sea Truck Recall Mod BZ";
        private const string VersionString = "1.1.0";

    }

    internal static class Log
    {
        public static void LogDebug(string message)
        {
            Debug.Log(message);
        }

        public static void LogInfo(string message)
        {
            Debug.Log(message);
        }
    }
}