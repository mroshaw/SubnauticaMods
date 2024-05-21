using DaftAppleGames.SeatruckRecall_BZ.DockRecaller;
using DaftAppleGames.SeatruckRecall_BZ.DockRecaller.Ui;
using DaftAppleGames.SeatruckRecall_BZ.Utils;
using HarmonyLib;
using Plugin = DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

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
                Plugin.Log.LogDebug("Adding SeaTruckRecaller component...");
                SeaTruckDockRecaller newDockRecaller = __instance.gameObject.AddComponent<SeaTruckDockRecaller>();
                newDockRecaller.MaxRange = Plugin.MaximumRange.Value;
                Plugin.Log.LogDebug($"Added SeaTruckRecaller component to {__instance.gameObject.name}!");

                Plugin.Log.LogDebug("Finding terminal...");
                MoonpoolExpansionTerminal terminal = __instance.GetComponentInChildren<MoonpoolExpansionTerminal>();
                if (terminal)
                {
                    Plugin.Log.LogDebug("Found terminal...");
                    Plugin.Log.LogDebug("Adding GUI component...");
                    terminal.gameObject.AddComponent<SeaTruckDockRecallerUi>();
                    Plugin.Log.LogDebug("Added GUI component!");
                }

                Plugin.Log.LogDebug("Calling DockRecaller register...");
                ModUtils.RegisterDockRecaller(newDockRecaller);
                Plugin.Log.LogDebug("DockRecaller register call complete.");
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
                Plugin.Log.LogDebug("Calling DockRecaller Un-register...");
                ModUtils.RegisterDockRecaller(dockRecaller);
                Plugin.Log.LogDebug("DockRecaller unregistered call complete.");
            }
        }

        /// <summary>
        /// Patch the AllowedToDock method, to allow an un-piloted SeaTruck to dock
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="dockable"></param>
        /// <param name="__result"></param>
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
                    Plugin.Log.LogDebug("Allowed to dock is true.");
                }
                else
                {
                    Plugin.Log.LogDebug("Allowed to dock is false.");
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
            Plugin.Log.LogDebug("Docking......");
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (dockRecaller)
            {
                Plugin.Log.LogDebug("Docking complete.");
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
            Plugin.Log.LogDebug("Undocking......");
            SeaTruckDockRecaller dockRecaller = __instance.GetComponent<SeaTruckDockRecaller>();
            if (dockRecaller)
            {
                Plugin.Log.LogDebug("Undocking complete.");
                dockRecaller.SetUndocked();
            }
        }
    }
}
