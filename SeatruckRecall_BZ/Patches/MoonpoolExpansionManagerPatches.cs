﻿using DaftAppleGames.SeatruckRecall_BZ.DockRecaller;
using DaftAppleGames.SeatruckRecall_BZ.DockRecaller.Ui;
using HarmonyLib;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Patches
{
    /// <summary>
    /// Harmony patching methods for the SeatruckDock
    /// </summary>
    [HarmonyPatch(typeof(MoonpoolExpansionManager))]
    internal class MoonpoolExpansionManagerPatches
    {
        /// <summary>
        /// Patch the Start method, adding the new component
        /// and register with the static list.
        /// </summary>
        [HarmonyPatch(nameof(MoonpoolExpansionManager.Start))]
        [HarmonyPostfix]
        internal static void StartPostfix(MoonpoolExpansionManager __instance)
        {
            // Add the SeatruckRecall component
            SeaTruckDockRecaller newDockRecaller = __instance.gameObject.EnsureComponent<SeaTruckDockRecaller>();
            DockRecallers.AddInstance(newDockRecaller);

            Log.LogDebug("Finding terminal...");
            MoonpoolExpansionTerminal terminal = __instance.GetComponentInChildren<MoonpoolExpansionTerminal>();
            if (terminal)
            {
                Log.LogDebug("Found terminal...");
                terminal.gameObject.AddComponent<SeaTruckDockRecallerUi>();
                Log.LogDebug("Added GUI component!");
            }
            else
            {
                Log.LogError("No terminal found on MoonpoolExpansion!");
            }
        }

        /// <summary>
        /// Patch the OnDestroy method, removing the instance
        /// from the static list
        /// </summary>
        [HarmonyPatch(nameof(MoonpoolExpansionManager.OnDestroy))]
        [HarmonyPostfix]
        internal static void OnDestroyPostfix(MoonpoolExpansionManager __instance)
        {
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (dockRecaller)
            {
                DockRecallers.RemoveInstance(dockRecaller);
            }
        }

        /// <summary>
        /// Patch the AllowedToDock method, to allow an un-piloted SeaTruck to dock
        /// </summary>
        [HarmonyPatch(nameof(MoonpoolExpansionManager.AllowedToDock))]
        [HarmonyPostfix]
        internal static void AllowedToDockPostfix(MoonpoolExpansionManager __instance, Dockable dockable, ref bool __result)
        {
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (!dockRecaller)
            {
                return;
            }
            __result = !(dockable == null) && !(dockable.truckSegment == null) && !__instance.IsOccupied() && !(__instance.exitingTruck != null)
                       && !__instance.DockingBlockersInTheWay() &&
                       (__instance.isLoading || __instance.IsPowered())
                       && (__instance.isLoading || !__instance.CheckIfSeatruckModulePresent(__instance.tailDockingPosition.position));
            Log.LogDebug(__result ? "Allowed to dock is true." : "Allowed to dock is false.");
        }

        /// <summary>
        /// Keep the Recall Dock status updated - when docking complete
        /// </summary>
        [HarmonyPatch(nameof(MoonpoolExpansionManager.StartDocking))]
        [HarmonyPostfix]
        internal static void StartDockingPostfix(MoonpoolExpansionManager __instance)
        {
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (!dockRecaller)
            {
                return;
            }
            Log.LogDebug("Recall Dock docking noted as complete.");
            dockRecaller.Docked();
        }

        /// <summary>
        /// Keep the Recall Dock status updated - when un-docking complete
        /// </summary>
        [HarmonyPatch(nameof(MoonpoolExpansionManager.StartUndocking))]
        [HarmonyPostfix]
        internal static void StartUndockingPostfix(MoonpoolExpansionManager __instance)
        {
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (!dockRecaller)
            {
                return;
            }
            Log.LogDebug("Recall Dock Undocking noted as complete.");
            dockRecaller.Undocked();
        }
    }
}