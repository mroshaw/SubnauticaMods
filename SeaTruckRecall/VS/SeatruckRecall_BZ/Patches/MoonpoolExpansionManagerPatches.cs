using DaftAppleGames.SeatruckRecall_BZ.DockRecaller;
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
            AllSeaTruckDockRecallers.AddInstance(newDockRecaller);

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
                AllSeaTruckDockRecallers.RemoveInstance(dockRecaller);
            }
        }

        /// <summary>
        /// Patch the AllowedToDock method, to allow an un-piloted SeaTruck to dock
        /// </summary>
        [HarmonyPatch(nameof(MoonpoolExpansionManager.AllowedToDock))]
        [HarmonyPrefix]
        internal static bool AllowedToDockPrefix(MoonpoolExpansionManager __instance, Dockable dockable, ref bool __result)
        {
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (!dockRecaller)
            {
                return true;
            }

            Log.LogDebug($"IsAllowedToDock: exitingSeaTruck==null is {__instance.exitingTruck == null}");
            Log.LogDebug($"IsAllowedToDock: DockingBlockersInTheWay is {__instance.DockingBlockersInTheWay()}");
            Log.LogDebug($"IsAllowedToDock: isLoading is {__instance.isLoading}");
            Log.LogDebug($"IsAllowedToDock: IsPowered is {__instance.IsPowered()}");
            Log.LogDebug($"IsAllowedToDock: CheckIfSeatruckModulePresent is {__instance.CheckIfSeatruckModulePresent(__instance.tailDockingPosition.position)}");

            __result = __instance.exitingTruck == null &&
                       !__instance.DockingBlockersInTheWay() &&
                       (__instance.isLoading || __instance.IsPowered()) &&
                       (__instance.isLoading || __instance.CheckIfSeatruckModulePresent(__instance.tailDockingPosition.position));
            Log.LogDebug($"IsAllowedToDock is: {__result}");
            return false;
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