using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DaftAppleGames.SeatruckRecall_BZ.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ
{
    // Mod supports "Teleporting" a Seatruck, and forcing a an "Autopilot" behaviour
    public enum RecallMoveMethod
    {
        Teleport,
        Autopilot
    };

    public enum DockRecallStatus
    {
        None,
        Docked,
        DockClear,
        RecallInProgress,
        RecallAborted,
        NoSeaTrucks,
        NoneInRange,
        FoundClosestSeaTruck
    }

    public enum SeaTruckRecallStatus
    {
        None,
        Docked,
        InTransit,
        ArrivedAtDock,
        RotatingToPosition,
        InPosition,
        Docking,
        Blocked
    }

    [BepInPlugin(MyGuid, PluginName, VersionString)]
    public class SeaTruckDockRecallPlugin : BaseUnityPlugin
    {
        // Plugin properties
        private const string MyGuid = "com.mroshaw.seatruckrecallbz";
        private const string PluginName = "Sea Truck Recall Mod BZ";
        private const string VersionString = "1.0.0";

        // Config properties
        internal static string RecallKeyboardShortcutKey = "Recall Keyboard Shortcut";
        private const string RecallMethodKey = "Recall Travel Method";
        private const string RecallRangeKey = "Recall Maximum Range";
        private const string TransitSpeedKey = "Recall Transit Speed";

        // Static config settings
        internal static ConfigEntry<RecallMoveMethod> RecallMethod;
        internal static ConfigEntry<KeyboardShortcut> RecallKeyboardShortcut;
        internal static ConfigEntry<float> RecallRange;
        internal static ConfigEntry<float> TransitSpeed;

        // Static global list of SeaTruck Docking Recall modules
        private static List<SeaTruckDockRecaller> _allDockRecallers = new List<SeaTruckDockRecaller>();
        private static List<SeaTruckAutoPilot> _allSeaTruckAutoPilots = new List<SeaTruckAutoPilot>();

        private static readonly Harmony Harmony = new Harmony(MyGuid);

        internal static ManualLogSource Log;

        private void Awake()
        {

            // Recall creature keyboard shortcut
            RecallKeyboardShortcut = Config.Bind("General",
                RecallKeyboardShortcutKey,
                new KeyboardShortcut(KeyCode.R, KeyCode.LeftControl));

            // Travel method for the recall process
            RecallMethod = Config.Bind("General",
                RecallMethodKey,
                RecallMoveMethod.Teleport,
                "Determines how the SeaTruck will move to the dock location");

            TransitSpeed = Config.Bind("General",
                TransitSpeedKey,
                2.0f,
                new ConfigDescription("The speed at which the SeaTruck will travel on autopilot.",
                    new AcceptableValueRange<float>(1.0f, 10.0f)));


            RecallRange = Config.Bind("General",
                RecallRangeKey,
                100.0f,
                new ConfigDescription("The maximum range of the recall function.",
                    new AcceptableValueRange<float>(10.0f, 500.0f)));

            // Patch in our MOD
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loading...");
            Harmony.PatchAll();
            Logger.LogInfo(PluginName + " " + VersionString + " " + "loaded.");
            Log = Logger;
        }

        /// <summary>
        /// Register a Seatruck with the recaller
        /// </summary>
        /// <param name="seatruck"></param>
        internal static void RegisterDockRecaller(SeaTruckDockRecaller dockRecaller)
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Registering SeaTruckDockRecaller...");
            _allDockRecallers.Add(dockRecaller);
            SeaTruckDockRecallPlugin.Log.LogDebug("Registered SeaTruckDockRecaller!");
        }

        /// <summary>
        /// Unregister a Seatruck with the recaller
        /// </summary>
        /// <param name="seatruck"></param>
        internal static void UnregisterDockRecaller(SeaTruckDockRecaller dockRecaller)
        {
            SeaTruckDockRecallPlugin.Log.LogInfo("Unregistering SeaTruckDockRecaller...");
            _allDockRecallers.Remove(dockRecaller);
            SeaTruckDockRecallPlugin.Log.LogInfo("Unregistered SeaTruckDockRecaller!");
        }

        /// <summary>
        /// Register a Seatruck with the recaller
        /// </summary>
        /// <param name="seatruck"></param>
        internal static void RegisterAutoPilot(SeaTruckAutoPilot autoPilot)
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Registering SeaTruckAutoPilot...");
            _allSeaTruckAutoPilots.Add(autoPilot);
            SeaTruckDockRecallPlugin.Log.LogDebug("Registered SeaTruckAutoPilot!");
        }

        /// <summary>
        /// Unregister a Seatruck with the recaller
        /// </summary>
        /// <param name="seatruck"></param>
        internal static void UnRegisterAutoPilot(SeaTruckAutoPilot autoPilot)
        {
            SeaTruckDockRecallPlugin.Log.LogInfo("Unregistering SeaTruckAutoPilot...");
            _allSeaTruckAutoPilots.Remove(autoPilot);
            SeaTruckDockRecallPlugin.Log.LogInfo("Unregistered SeaTruckAutoPilot!");
        }

        /// <summary>
        /// Return the current list of SeaTruckAutoPilots
        /// </summary>
        /// <returns></returns>
        internal static List<SeaTruckAutoPilot> GetAllAutoPilots()
        {
            return _allSeaTruckAutoPilots;
        }
    }
}
