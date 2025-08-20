using System.Collections;
using System.Collections.Generic;
using DaftAppleGames.SeatruckRecall_BZ.AutoPilot;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;

using UnityEngine;
using UnityEngine.Events;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.DockRecaller
{
    // Recaller Status
    internal enum DockRecallState
    {
        None,
        Ready,
        NoneInRange,
        Recalling,
        Stuck,
        Aborted,
        Parking,
        Docked
    }

    // Unity Event to publish DockRecallStatus changes
    internal class DockRecallStateChangedEvent : UnityEvent<DockRecallState>
    {
    }

    /// <summary>
    /// MonoBehaviour class to attach to a SeatruckDock
    /// that implements the recall behaviour
    /// </summary>
    internal class SeaTruckDockRecaller : MonoBehaviour
    {
        // Waypoint names
        private const string MoveToBaseText = "MOVING TO BASE";
        private const string AlignToDockText = "ALIGNING TO DOCK";
        private const string MovingToDockText = "MOVING TO DOCK";

        [SerializeField] private SeaTruckAutoPilot currentSeaTruckAutoPilot;
        // Transform within the dock, that the recall will pull the SeaTruck into it's final docking place
        // If not docked within the timeout, abandon
        [SerializeField] private Vector3 parkingDockConnection;
        [SerializeField] private List<Waypoint> dockingWaypoints;

        // Public properties
        public float MaxRange { get; set; } = 500.0f;

        // Useful internal components
        private MoonpoolExpansionManager _dockingManager;

        // Internal tracking and audit
        private DockRecallState _currentRecallState = DockRecallState.None;

        private const float ParkingTimeout = 5.0f;
        private const float ParkingMoveSpeed = 1.0f;
        private const float ParkingRotateSpeed = 1.0f;

        // Event publishing latest recall state and distance
        internal DockRecallStateChangedEvent OnDockingStateChanged = new DockRecallStateChangedEvent();
        internal WaypointChangedEvent OnDockingWaypointChanged = new WaypointChangedEvent();
        internal SeaTruckAutoPilot.AutopilotStateChangedEvent OnAutoPilotStateChanged = new SeaTruckAutoPilot.AutopilotStateChangedEvent();

        internal UnityEvent OnDocked = new UnityEvent();

        private void Start()
        {
            // Init useful local components
            _dockingManager = GetComponent<MoonpoolExpansionManager>();

            // Set the initial dock status
            SetCurrentDockedStatus();

            // Set the parking position
            parkingDockConnection = gameObject.transform.position + (-gameObject.transform.right * 2.0f);

            // Set up the docking waypoints
            CreateWaypoints();
        }

        public void CurrentSeaTruckDocked()
        {
            SetDockState(DockRecallState.Docked);
            OnDocked?.Invoke();
        }

        public void ReleaseCurrentlyDocked()
        {
            if (currentSeaTruckAutoPilot == null)
            {
                Log.LogDebug("ReleaseCurrentlyDocked called but there is no SeaTruck docked.");
                return;
            }
            ReleaseCurrentSeaTruck();
            SetDockState(DockRecallState.Ready);
        }

        /// <summary>
        /// Public method to cancel in-progress Recall
        /// </summary>
        internal void AbortRecall()
        {
            Log.LogDebug("Aborting Recall...");
            SetDockState(DockRecallState.Aborted);
        }

        /// <summary>
        /// Public method to recall the closest Seatruck
        /// </summary>
        public void RecallClosestSeatruck()
        {
            if (IsDockReady())
            {
                Log.LogDebug("Dock is already occupied or busy!");
                return;
            }

            Log.LogDebug("Finding closest Seatruck...");
            SeaTruckAutoPilot closestAutoPilot = AllAutoPilots.GetClosestAutoPilot(transform.position, MaxRange);
            if (closestAutoPilot == null)
            {
                // Couldn't find a closest Seatruck
                Log.LogDebug("No Seatrucks found!");
                _currentRecallState = DockRecallState.NoneInRange;
                return;
            }

            // Recall the SeaTruck
            SetDockingAutoPilot(closestAutoPilot);
            closestAutoPilot.StartNavigation(dockingWaypoints);
        }

        /// <summary>
        /// Set up the docking waypoints for this dock
        /// </summary>
        private void CreateWaypoints()
        {
            dockingWaypoints = new List<Waypoint>();

            // Waypoint above the entrance to the docking tube
            GameObject aboveDockingTubeWaypoint = new GameObject("Top of End of Tube Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 30.0f) + (gameObject.transform.up * 10.0f)
                }
            };
            dockingWaypoints.Add(new Waypoint(aboveDockingTubeWaypoint.transform.position,
                Quaternion.identity,
                false,
                MoveToBaseText));

            // CreateSphere(aboveDockingTubeWaypoint.transform.position, 2.0f, Color.red);
            Log.LogDebug($"Dock tube above end position: {aboveDockingTubeWaypoint.transform.position}");

            // Waypoint at the end of the docking tube.
            GameObject endOfDockTubeWaypoint = new GameObject("End of Tube Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 50.0f)
                }
            };
            dockingWaypoints.Add(new Waypoint(endOfDockTubeWaypoint.transform.position,
                Quaternion.identity,
                true,
                AlignToDockText));

            // CreateSphere(endOfDockTubeWaypoint.transform.position, 1.5f, Color.yellow);
            Log.LogDebug($"Dock tube end position: {endOfDockTubeWaypoint.transform.position}");

            // Waypoint into the docking tube itself
            GameObject dockingWaypoint = new GameObject("Docking Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 15.0f)
                }
            };
            dockingWaypoints.Add(new Waypoint(dockingWaypoint.transform.position,
                Quaternion.identity,
                true,
                MovingToDockText));

            // CreateSphere(dockingWaypoint.transform.position, 1.0f, Color.green);
            Log.LogDebug($"Dock final position: {dockingWaypoint.transform.position}");
        }

        private void SetDockingAutoPilot(SeaTruckAutoPilot autoPilot)
        {
            if (!autoPilot)
            {
                Log.LogDebug("Attempt to set current AutoPilot to null!");
                return;
            }

            if (currentSeaTruckAutoPilot)
            {
                Log.LogDebug("AutoPilot is already set!");
                return;
            }
            currentSeaTruckAutoPilot = autoPilot;
            autoPilot.OnAutopilotStateChanged.AddListener(AutoPilotStateChangedHandler);
            autoPilot.OnAutopilotWaypointChanged.AddListener(AutopilotWaypointChangedHandler);
        }

        private void ReleaseCurrentSeaTruck()
        {
            currentSeaTruckAutoPilot.ReleaseFromDock();
            currentSeaTruckAutoPilot.OnAutopilotStateChanged.RemoveListener(AutoPilotStateChangedHandler);
            currentSeaTruckAutoPilot.OnAutopilotWaypointChanged.RemoveListener(AutopilotWaypointChangedHandler);
            currentSeaTruckAutoPilot = null;
        }

        private void AutopilotWaypointChangedHandler(Waypoint newWaypoint)
        {
            OnDockingWaypointChanged?.Invoke(newWaypoint);
        }

        /// <summary>
        /// Sets the appropriate docked status
        /// </summary>
        private void SetCurrentDockedStatus()
        {
            SetDockState(IsDockReady() ? DockRecallState.Docked : DockRecallState.Ready);
        }

        /// <summary>
        /// Returns true if Seatruck is already docked here
        /// otherwise false
        /// </summary>
        public bool IsDockReady()
        {
            return _dockingManager.IsOccupied() || currentSeaTruckAutoPilot != null;
        }

        private void AutoPilotWaypointChangedHandler(Waypoint waypoint)
        {
            OnDockingWaypointChanged.Invoke(waypoint);
        }

        /// <summary>
        /// Handle Waypoint change
        /// </summary>
        private void AutoPilotStateChangedHandler(AutoPilotState autoPilotState)
        {
            Log.LogDebug($"DockRecaller.AutoPilotStateChangedHandler: {autoPilotState}.");

            // Autopilot state changes
            switch (autoPilotState)
            {
                // AutoPilot has arrived. Docking isn't guaranteed, so we'll check that and engage
                // the tractor beam if required
                case AutoPilotState.Arrived:
                    SetDockState(DockRecallState.Parking);
                    ParkSeaTruck();
                    break;
                case AutoPilotState.Moving:
                    SetDockState(DockRecallState.Recalling);
                    break;
                case AutoPilotState.Blocked:
                case AutoPilotState.Aborted:
                    SetDockState(DockRecallState.Ready);
                    break;
            }

            OnAutoPilotStateChanged.Invoke(autoPilotState);
        }

        private void SetDockState(DockRecallState newRecallState)
        {
            if (newRecallState == _currentRecallState)
            {
                return;
            }

            Log.LogDebug($"SeaTruckRecaller.SetDockState: state changed from {_currentRecallState} to {newRecallState}.");
            _currentRecallState = newRecallState;
            OnDockingStateChanged?.Invoke(newRecallState);
        }

        /// <summary>
        /// Pulls the SeaTruck towards the dock, forcing it to engage and dock
        /// </summary>
        private void ParkSeaTruck()
        {
            StartCoroutine(ParkSeaTruckAsync());
        }

        private IEnumerator ParkSeaTruckAsync()
        {
            Log.LogDebug("Parking SeaTruck...");

            currentSeaTruckAutoPilot.BeginParking();

            float dockTime = 0.0f;

            if (currentSeaTruckAutoPilot == null)
            {
                Log.LogDebug("Parking cancelled - SeaTruck not set");
                yield break;
            }

            Vector3 dirToTarget = parkingDockConnection - currentSeaTruckAutoPilot.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);

            while (_currentRecallState != DockRecallState.Docked)
            {
                // Rotate
                currentSeaTruckAutoPilot.transform.rotation = Quaternion.Slerp(currentSeaTruckAutoPilot.transform.rotation, targetRotation, Time.deltaTime * ParkingRotateSpeed);
                dockTime += Time.deltaTime;

                // Move
                currentSeaTruckAutoPilot.transform.position = Vector3.Lerp(currentSeaTruckAutoPilot.transform.position, parkingDockConnection, Time.deltaTime * ParkingMoveSpeed);

                if (dockTime > ParkingTimeout)
                {
                    Log.LogDebug("Parking timed out!");
                    SetDockState(DockRecallState.Stuck);
                    yield break;
                }

                yield return null;
            }
            currentSeaTruckAutoPilot.DockingComplete();
            Log.LogDebug("Docked state set: Parking Complete!");
        }

        private void CreateSphere(Vector3 spherePosition, float radius, Color color)
        {
            Transform parent = gameObject.transform;

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(sphere.GetComponent<Collider>());
            sphere.transform.SetParent(gameObject.transform, false);
            sphere.transform.position = spherePosition;

            Vector3 inverseScale = new Vector3(
                1f / parent.lossyScale.x,
                1f / parent.lossyScale.y,
                1f / parent.lossyScale.z
            );

            sphere.transform.localScale = inverseScale * radius;
            sphere.GetComponent<Renderer>().material.color = color;
        }
    }
}