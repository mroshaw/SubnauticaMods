using System.Collections.Generic;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.AutoPilot
{
    /// <summary>
    /// MonoBehavior implementing SeaTruck specific AutoPilot behavior
    /// the game.
    /// </summary>
    internal class SeaTruckAutoPilot : AutoPilot
    {
        private PathFinder _pathFinder;
        private List<Waypoint> _recallWaypoints;
        // Lists the layers to INCLUDE in collision avoidance
        private LayerMask _collisionLayerMask;

        private Transform _debugContainer;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (!_pathFinder)
            {
                _pathFinder = GetComponent<PathFinder>();
            }


            // Include layers for obstacle detection
            _collisionLayerMask = LayerMask.GetMask("TerrainCollider", "Default");
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            // _pathFinder.OnPathingStatusChanged.RemoveListener(PathingChangedHandler);
        }

        protected override void Start()
        {
            base.Start();
            GameObject debugGameObject = new GameObject("NAVGRID");
            _debugContainer = debugGameObject.transform;
        }

        internal override bool BeginNavigation(List<Waypoint> waypoints)
        {
            // Generate a path from the SeaTruck to the first point in the Recaller waypoints
            Vector3 targetPosition = waypoints[0].Position;

            _recallWaypoints = waypoints;

            // Creates a grid of cells 10.0 units square, 5 squares to the left, right, top and bottom from the SeaTruck to the target.
            SetAutopilotState(AutoPilotState.CalculatingRoute);
            _pathFinder.GenerateWaypoints(transform.position, targetPosition, 5.0f, 5, _collisionLayerMask, null, null, WaypointsCompleteHandler, true, _debugContainer);

            return true;
        }

        /// <summary>
        /// Manages the Pathing responses from the PathFinder MonoBehaviour
        /// </summary>
        private void WaypointsCompleteHandler(GenerateStatus status, List<Waypoint> waypoints)
        {
            // Confirm we can get the waypoints from the PathFinder
            if (status == GenerateStatus.Success)
            {
                // Append the remaining dock recall waypoints
                for (int i = 0; i < _recallWaypoints.Count; i++)
                {
                    waypoints.Add(_recallWaypoints[i]);
                }

                // Begin moving through the waypoints
                SetAutopilotState(AutoPilotState.Ready);
                base.BeginNavigation(waypoints);
            }
        }
    }
}