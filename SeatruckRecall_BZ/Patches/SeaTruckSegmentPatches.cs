using HarmonyLib;
using DaftAppleGames.SeatruckRecall_BZ.AutoPilot;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Patches
{
    /// <summary>
    /// Harmony patches for the Seatruck
    /// </summary>
    [HarmonyPatch(typeof(SeaTruckSegment))]
    internal class SeaTruckSegmentPatches
    {
        /// <summary>
        /// Patch the Start method, to add the instance
        /// to the static global list
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(SeaTruckSegment.Start))]
        [HarmonyPostfix]
        internal static void StartPostfix(SeaTruckSegment __instance)
        {
            // Check to see if this is the "main" truck segment
            if (__instance.isMainCab)
            {
                // Add the SeatruckRecallListener component
                if (!__instance.gameObject.GetComponent<SeaTruckAutoPilot>())
                {
                    // Add the new AutoPilot component
                    Log.LogInfo("Adding SeaTruckAutopilot component...");
                    SeaTruckAutoPilot newAutoPilot = __instance.gameObject.AddComponent<SeaTruckAutoPilot>();
                    AutoPilots.AddInstance(newAutoPilot);
                    Log.LogInfo(
                    $"Added SeaTruckAutopilot component to {__instance.gameObject.name}!");

                    // Add and configure the MoveMethod
                    WaypointNavigation navMovement;

                    switch (ConfigFile.RecallMoveMethod)
                    {
                        case RecallMoveMethod.Smooth:
                            navMovement = __instance.gameObject.AddComponent<RigidbodyNavMovement>();
                            navMovement.MoveTolerance = 0.2f;
                            navMovement.RotateTolerance = 0.99f;
                            navMovement.SlowDistance = 2.0f;
                            break;

                        case RecallMoveMethod.Fixed:
                            navMovement = __instance.gameObject.AddComponent<TransformNavMovement>();
                            navMovement.MoveTolerance = 0.2f;
                            navMovement.RotateTolerance = 0.99f;
                            break;

                        case RecallMoveMethod.Teleport:
                            navMovement = __instance.gameObject.AddComponent<TeleportNavMovement>();
                            navMovement.MoveTolerance = 0.2f;
                            navMovement.RotateTolerance = 0.99f;
                            break;
                        default:
                            navMovement = __instance.gameObject.AddComponent<TeleportNavMovement>();
                            break;
                    }

                    navMovement.MoveSpeed = ConfigFile.TransitSpeed;
                    navMovement.RotateSpeed = ConfigFile.RotateSpeed;
                }
            }
        }

        /// <summary>
        /// Patch the OnDestroy method, to remove
        /// the instance from the global list
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(SeaTruckSegment.OnDestroy))]
        [HarmonyPostfix]
        internal static void OnDestroyPostfix(SeaTruckSegment __instance)
        {
            SeaTruckAutoPilot autoPilot = __instance.GetComponent<SeaTruckAutoPilot>();
            if (autoPilot)
            {
                AutoPilots.RemoveInstance(autoPilot);
            }
        }

        /// <summary>
        /// Patch UpdateKinematicState, to prevent the Rigidbody being disabled
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(SeaTruckSegment.UpdateKinematicState))]
        [HarmonyPrefix]
        internal static bool UpdateKinematicStatePrefix(SeaTruckSegment __instance)
        {
            SeaTruckSegment firstSegment = __instance.GetFirstSegment();
            SeaTruckAutoPilot autoPilot = firstSegment.GetComponent<SeaTruckAutoPilot>();
            if (autoPilot)
            {
                // Log.LogDebug("UpdateKinematicStatePrefix called...");
                // if (autoPilot.AutoPilotInProgress)
                if (true)
                {
                    UWE.Utils.SetIsKinematicAndUpdateInterpolation(firstSegment.rb, false, false);
                    return false;
                }
            }
            return true;
        }
    }
}