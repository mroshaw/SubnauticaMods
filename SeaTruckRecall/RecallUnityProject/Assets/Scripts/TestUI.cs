using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    public class TestUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text gridStatusText;
        [SerializeField] private TMP_Text pathingStatusText;
        [SerializeField] private TMP_Text waypointStatusText;

        [SerializeField] TMP_InputField forceText;

        [SerializeField] private WaypointNavigation navSystem;
        [SerializeField] private PathFinder pathFinder;

        [SerializeField] private Transform waypoint1;
        [SerializeField] private Transform waypoint2;
        [SerializeField] private Transform waypoint3;
        [SerializeField] private Transform waypoint4;
        [SerializeField] private Transform waypoint5;

        private List<Waypoint> _waypoints;

        public void NavigateInternalWaypoints()
        {
            List<Waypoint> waypoints = new List<Waypoint>();

            waypoints.Add(new Waypoint(waypoint1.position, waypoint1.rotation, true, "Waypoint 1"));
            waypoints.Add(new Waypoint(waypoint2.position, waypoint2.rotation, true, "Waypoint 2"));
            waypoints.Add(new Waypoint(waypoint3.position, waypoint3.rotation, true, "Waypoint 3"));
            waypoints.Add(new Waypoint(waypoint4.position, waypoint4.rotation, true, "Waypoint 4"));
            waypoints.Add(new Waypoint(waypoint5.position, waypoint5.rotation, true, "Waypoint 5"));

            navSystem.SetWayPoints(waypoints);
            navSystem.StartWaypointNavigation();
        }

        private void OnEnable()
        {
            pathFinder.OnWaypointStatusChanged.AddListener(WaypointStatusChangedHandler);
            pathFinder.OnGridStatusChanged.AddListener(GridStatusChangedHandler);
            pathFinder.OnPathingStatusChanged.AddListener(PathingStatusChangedHandler);
        }

        private void OnDisable()
        {
            pathFinder.OnWaypointStatusChanged.RemoveListener(WaypointStatusChangedHandler);
            pathFinder.OnGridStatusChanged.RemoveListener(GridStatusChangedHandler);
            pathFinder.OnPathingStatusChanged.RemoveListener(PathingStatusChangedHandler);
        }

        public void GenerateGridPathAndNavigate()
        {
            pathFinder.GenerateWaypoints(WaypointsReady);
        }

        public void StartNav()
        {
            navSystem.StartWaypointNavigation(_waypoints);
        }

        public void RefreshGrid()
        {
            pathFinder.RefreshNavGrid();
        }

        public void RefreshPath()
        {
            pathFinder.GenerateWaypoints(SetWaypoints);
        }

        private void SetWaypoints(GenerateStatus waypointStatus, List<Waypoint> waypoints)
        {
            if (waypointStatus != GenerateStatus.Success)
            {
                Log.LogError("Failed to generate waypoints!");
                return;
            }

            _waypoints = waypoints;
        }

        private void WaypointsReady(GenerateStatus waypointStatus, List<Waypoint> waypoints)
        {
            if (waypointStatus != GenerateStatus.Success)
            {
                Log.LogError("Failed to generate waypoints!");
                return;
            }
            navSystem.StartWaypointNavigation(waypoints);
        }

        #region UI state handlers
        private void GridStatusChangedHandler(GenerateStatus status)
        {
            gridStatusText.text = status.ToString();
        }

        private void PathingStatusChangedHandler(GenerateStatus status)
        {
            pathingStatusText.text = status.ToString();
        }

        private void WaypointStatusChangedHandler(GenerateStatus status)
        {
            waypointStatusText.text = status.ToString();
        }
        #endregion
        #region Utility methods
        public void ApplyForce()
        {
            float forceToApply = float.Parse(forceText.text);
            Rigidbody rigidBody = navSystem.GetComponent<Rigidbody>();

            Vector3 forwardForce = rigidBody.transform.forward * forceToApply;

            rigidBody.AddForce(forwardForce);
        }

        public void ConfigureRigidBodies()
        {
            RigidbodyNavMovement rbNav = navSystem as RigidbodyNavMovement;
            rbNav.ConfigureRigidBodies();
        }

        public void RestoreRigidBodies()
        {
            RigidbodyNavMovement rbNav = navSystem as RigidbodyNavMovement;
            rbNav.RestoreRigidBodies();
        }
        #endregion
    }
}