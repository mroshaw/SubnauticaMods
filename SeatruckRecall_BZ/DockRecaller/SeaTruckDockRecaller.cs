using System.Collections.Generic;
using DaftAppleGames.SeatruckRecall_BZ.AutoPilot;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using UnityEngine;
using UnityEngine.Events;

namespace DaftAppleGames.SeatruckRecall_BZ.DockRecaller
{
    // Recaller Status
    internal enum DockRecallStatus
    {
        None,
        Docked,
        DockClear,
        DockBlocked,
        RecallInProgress,
        RecallAborted,
        NoSeaTrucks,
        NoneInRange,
        FoundClosestSeaTruck
    }

    // Unity Event to publish DockRecallStatus changes
    internal class DockRecallStatusChangedEvent : UnityEvent<DockRecallStatus>
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

        // Useful internal components
        private MoonpoolExpansionManager _dockingManager;

        // Internal tracking and audit
        private DockRecallStatus _currentRecallStatus = DockRecallStatus.None;
        private DockRecallStatus _previousRecallStatus = DockRecallStatus.None;
        private AutoPilot.BaseAutoPilot _currentRecallAutoPilot;

        // Internal fields
        private List<Waypoint> _dockingWaypoints;

        // Event publishing latest recall state and distance
        internal DockRecallStatusChangedEvent OnDockRecallStatusChangedEvent = new DockRecallStatusChangedEvent();

        /// <summary>
        /// Initialise the component
        /// </summary>
        internal void Start()
        {
            // Init useful local components
            _dockingManager = GetComponent<MoonpoolExpansionManager>();

            // Set the initial dock status
            SetDockedStatus();

            // Set up the docking waypoints
            CreateWaypoints();

            // Check if dock entrance is clear
            IsDockEntranceClear();
        }

        /// <summary>
        /// Set up the docking waypoints for this dock
        /// </summary>
        private void CreateWaypoints()
        {
            _dockingWaypoints = new List<Waypoint>();

            // Waypoint above the entrance to the docking tube
            GameObject aboveDockingTubeWaypoint = new GameObject("Top of End of Tube Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 30.0f) + (gameObject.transform.up * 10.0f)
                }
            };
            _dockingWaypoints.Add(new Waypoint(aboveDockingTubeWaypoint.transform,
                WaypointAction.RotateBeforeMove,
                MoveToBaseText));

            SeaTruckDockRecallPlugin.Log.LogDebug($"Dock tube above end position: {aboveDockingTubeWaypoint.transform.position}");

            // Waypoint at the end of the docking tube.
            GameObject endOfDockTubeWaypoint = new GameObject("End of Tube Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 50.0f)
                }
            };
            _dockingWaypoints.Add(new Waypoint(endOfDockTubeWaypoint.transform,
                WaypointAction.RotateBeforeMove,
                AlignToDockText));

            SeaTruckDockRecallPlugin.Log.LogDebug($"Dock tube end position: {endOfDockTubeWaypoint.transform.position}");

            // Waypoint into the docking tube itself
            GameObject dockingWaypoint = new GameObject("Docking Waypoint")
            {
                transform =
                {
                    position = gameObject.transform.position + (-gameObject.transform.right * 13.7f)
                }
            };
            _dockingWaypoints.Add(new Waypoint(dockingWaypoint.transform,
                WaypointAction.RotateBeforeMove,
                MovingToDockText));

            SeaTruckDockRecallPlugin.Log.LogDebug($"Dock final position: {dockingWaypoint.transform.position}");
        }

        /// <summary>
        /// Handle updating current state, if recall is underway
        /// </summary>
        internal void Update()
        {
            // DEBUG ONLY!!!
            if (SeaTruckDockRecallPlugin.RecallKeyboardShortcut.Value.IsDown())
            {
                RecallClosestSeatruck();
            }

            if (UpdateRecallStatus())
            {
                RecallStatusChanged();
            }
        }

        /// <summary>
        /// Checks for changes in the recall status
        /// </summary>
        /// <returns></returns>
        private bool UpdateRecallStatus()
        {
            if (_previousRecallStatus == _currentRecallStatus)
            {
                return false;
            }
            _previousRecallStatus = _currentRecallStatus;
            return true;
        }

        /// <summary>
        /// Called whenever state changes
        /// </summary>
        private void RecallStatusChanged()
        {
            // Update any listeners
            if (OnDockRecallStatusChangedEvent != null)
            {
                OnDockRecallStatusChangedEvent.Invoke(_currentRecallStatus);
            }
        }

        /// <summary>
        /// Sets the appropriate docked status
        /// </summary>
        private void SetDockedStatus()
        {
            if (IsDockOccupied())
            {
                _currentRecallStatus = DockRecallStatus.Docked;
            }
            else
            {
                _currentRecallStatus = DockRecallStatus.DockClear;
            }
        }

        /// <summary>
        /// Returns true if Seatruck is already docked here
        /// otherwise false
        /// </summary>
        /// <returns></returns>
        private bool IsDockOccupied()
        {
            return _dockingManager.IsOccupied();
        }

        /// <summary>
        /// Set the current status to Docked
        /// </summary>
        internal void SetDocked()
        {
            _currentRecallStatus = DockRecallStatus.Docked;
        }

        /// <summary>
        /// Set the current status to DockClear
        /// </summary>
        internal void SetUndocked()
        {
            _currentRecallStatus = DockRecallStatus.DockClear;
        }

        /// <summary>
        /// Public method to recall the closest Seatruck
        /// </summary>
        /// <returns></returns>
        internal bool RecallClosestSeatruck()
        {
            SeaTruckDockRecallPlugin.Log.LogDebug("Finding closest Seatruck...");
            _currentRecallAutoPilot = GetClosestSeaTruck();
            if (_currentRecallAutoPilot == null)
            {
                // Couldn't find a closest Seatruck
                SeaTruckDockRecallPlugin.Log.LogDebug("No Seatrucks found!");

                return false;
            }

            // TODO
            // Set the Recall status
            SeaTruckDockRecallPlugin.Log.LogDebug("Recalling to dock...");
            _currentRecallStatus = DockRecallStatus.RecallInProgress;

            // Recall the SeaTruck
            _currentRecallAutoPilot.BeginNavigation(_dockingWaypoints);

            return true;
        }

        /// <summary>
        /// Uses an OverlapSphere to see if there is enough room in front of the dock
        /// for the autopilot to manoeuvre
        /// </summary>
        /// <returns></returns>
        private bool IsDockEntranceClear()
        {
            Vector3 entrancePosition =  gameObject.transform.position + (-gameObject.transform.right * 50.0f);
            Collider[] allColliders = Physics.OverlapSphere(entrancePosition, 25.0f);
            foreach (Collider collider in allColliders)
            {
                SeaTruckDockRecallPlugin.Log.LogInfo($"Found this Collider in dock entrance sphere: {collider.gameObject.name}");
            }

            GameObject testSphereGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            testSphereGo.name = "EntranceSphere";
            testSphereGo.transform.position = entrancePosition;
            testSphereGo.transform.localScale = new Vector3(25.0f, 25.0f, 25.0f);


            return true;
        }

        /// <summary>
        /// Return the registered Seatruck that's closest to the Dock
        /// </summary>
        /// <returns></returns>
        private BaseAutoPilot GetClosestSeaTruck()
        {
            Transform currentTransform = transform;
            AutoPilot.BaseAutoPilot closestSeaTruck = null;
            float closestDistance = 0.0f;
            List<AutoPilot.BaseAutoPilot> allSeaTrucks = SeaTruckDockRecallPlugin.GetAllAutoPilots();

            float maxRange = SeaTruckDockRecallPlugin.MaximumRange.Value;

            // Don't need to do anything if there are no Seatrucks
            if (allSeaTrucks.Count == 0)
            {
                SeaTruckDockRecallPlugin.Log.LogInfo("No Seatrucks registered.");
                _currentRecallStatus = DockRecallStatus.NoSeaTrucks;

                return null;
            }

            SeaTruckDockRecallPlugin.Log.LogInfo($"Looking for SeaTrucks. Max range is: {maxRange}");

            // Loop through each seatruck, find out which is closest
            foreach (AutoPilot.BaseAutoPilot seatruck in allSeaTrucks)
            {
                // Check if already docked
                SeaTruckSegment segment = seatruck.GetComponent<SeaTruckSegment>();
                if (segment.isDocked)
                {
                    SeaTruckDockRecallPlugin.Log.LogInfo($"Seatruck {seatruck.gameObject.name} is already docked. Skipping...");
                    continue;
                }

                SeaTruckDockRecallPlugin.Log.LogInfo($"Checking distance to {seatruck.gameObject.name}...");
                float currDistance = Vector3.Distance(currentTransform.position, seatruck.gameObject.transform.position);
                {
                    SeaTruckDockRecallPlugin.Log.LogInfo($"Distance is: {currDistance}, closest so far is: {closestDistance}");
                    if ((closestDistance == 0 || currDistance < closestDistance) && currDistance <= maxRange)
                    {
                        SeaTruckDockRecallPlugin.Log.LogInfo("New closest Seatruck found!");
                        closestDistance = currDistance;
                        closestSeaTruck = seatruck;;
                    }
                }
            }
            
            // Check to see if we've found anything in range
            if (closestSeaTruck == null)
            {
                SeaTruckDockRecallPlugin.Log.LogInfo($"No SeaTrucks found within range!");
                _currentRecallStatus = DockRecallStatus.NoneInRange;
            }
            else
            {
                SeaTruckDockRecallPlugin.Log.LogInfo($"Closest Seatruck found: {closestSeaTruck.gameObject.name} at {closestDistance}");
                _currentRecallStatus = DockRecallStatus.FoundClosestSeaTruck;
            }
            
            return closestSeaTruck;
        }
    }
}
