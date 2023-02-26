using DaftAppleGames.SeatruckRecall_BZ.DockRecaller;
using DaftAppleGames.SeatruckRecall_BZ.DockRecaller.Ui;
using HarmonyLib;

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
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(MoonpoolExpansionManager.Start))]
        [HarmonyPostfix]
        internal static void StartPostfix(MoonpoolExpansionManager __instance)
        {
            // Add the SeatruckRecall component
            if (!__instance.gameObject.GetComponent<SeaTruckDockRecaller>())
            {
                SeaTruckDockRecallPlugin.Log.LogDebug("Adding SeaTruckRecaller component...");
                SeaTruckDockRecaller newDockRecaller = __instance.gameObject.AddComponent<SeaTruckDockRecaller>();
                SeaTruckDockRecallPlugin.Log.LogDebug($"Added SeaTruckRecaller component to {__instance.gameObject.name}!");

                SeaTruckDockRecallPlugin.Log.LogDebug("Finding terminal...");
                MoonpoolExpansionTerminal terminal = __instance.GetComponentInChildren<MoonpoolExpansionTerminal>();
                if (terminal)
                {
                    SeaTruckDockRecallPlugin.Log.LogDebug("Found terminal...");
                    SeaTruckDockRecallPlugin.Log.LogDebug("Adding GUI component...");
                    terminal.gameObject.AddComponent<SeaTruckDockRecallerUi>();
                    SeaTruckDockRecallPlugin.Log.LogDebug("Added GUI component!");
                }

                SeaTruckDockRecallPlugin.Log.LogDebug("Registering DockRecaller...");
                SeaTruckDockRecallPlugin.RegisterDockRecaller(newDockRecaller);
                SeaTruckDockRecallPlugin.Log.LogDebug("DockRecaller registered.");
            }
        }

        /// <summary>
        /// Patch the OnDestroy method, removing the instance
        /// from the static list
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(MoonpoolExpansionManager.OnDestroy))]
        [HarmonyPostfix]
        internal static void OnDestroyPostfix(MoonpoolExpansionManager __instance)
        {
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (dockRecaller)
            {
                SeaTruckDockRecallPlugin.Log.LogDebug("Unregistering DockRecaller...");
                SeaTruckDockRecallPlugin.RegisterDockRecaller(dockRecaller);
                SeaTruckDockRecallPlugin.Log.LogDebug("DockRecaller unregistered.");
            }
        }

        [HarmonyPatch(nameof(MoonpoolExpansionManager.AllowedToDock))]
        [HarmonyPostfix]
        internal static void AllowedToDockPostfix(MoonpoolExpansionManager __instance, Dockable dockable, ref bool __result)
        {
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (dockRecaller)
            {
                __result = !(dockable == null) && !(dockable.truckSegment == null) && !__instance.IsOccupied() && !(__instance.exitingTruck != null)
                           
                           && !__instance.DockingBlockersInTheWay() &&
                           (__instance.isLoading || __instance.IsPowered())
                           && (__instance.isLoading || !__instance.CheckIfSeatruckModulePresent(__instance.tailDockingPosition.position));
                if (__result)
                {
                    SeaTruckDockRecallPlugin.Log.LogDebug("Allowed to dock is true.");
                }
                else
                {
                    SeaTruckDockRecallPlugin.Log.LogDebug("Allowed to dock is false.");
                }
            }
        }

        /// <summary>
        /// Keep the Recall Dock status updated - when docking complete
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(MoonpoolExpansionManager.StartDocking))]
        [HarmonyPostfix]
        internal static void StartDockingPostfix(MoonpoolExpansionManager __instance)
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Docking......");
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (dockRecaller)
            {
                SeaTruckDockRecallPlugin.Log.LogDebug("Docking complete.");
                dockRecaller.SetDocked();
            }
        }

        /// <summary>
        /// Keep the Recall Dock status updated - when un-docking complete
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(MoonpoolExpansionManager.StartUndocking))]
        [HarmonyPostfix]
        internal static void StartUndockingPostfix(MoonpoolExpansionManager __instance)
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Undocking......");
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (dockRecaller)
            {
                SeaTruckDockRecallPlugin.Log.LogDebug("Undocking complete.");
                dockRecaller.SetUndocked();
            }
        }
    }
}
