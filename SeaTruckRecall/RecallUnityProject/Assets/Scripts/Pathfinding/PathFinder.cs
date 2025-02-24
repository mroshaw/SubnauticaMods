using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{

    internal class PathFinder : MonoBehaviour
    {
        public Transform targetTransform;

        public List<Vector3> waypointPositions;
        public List<Waypoint> waypoints;

        public float cellSize = 5.0f;
        public int cellExtends = 10;

        private NavGrid _navGrid;

        private void Start()
        {
            _navGrid = new NavGrid();
        }

        public void GenerateNavGrid()
        {

        }

        public void Nav()
        {
            GetComponent<WaypointNavigation>().SetWayPoints(waypoints);
            GetComponent<WaypointNavigation>().StartWaypointNavigation();
        }

        public void GeneratePositionWaypoints()
        {
            waypointPositions = new List<Vector3>();
            List<NavCell> navCells = _navGrid.GetPath(transform,  targetTransform,  cellSize, cellExtends);

            if (navCells.Count == 0)
            {
                Debug.Log("No route found!");
            }

            foreach (NavCell navCell in navCells)
            {
                waypointPositions.Add(navCell.Position);
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = navCell.Position;
                sphere.transform.localScale = new Vector3(1f, 1f, 1f);
                sphere.GetComponent<Renderer>().material.color = Color.yellow;
            }
        }

        public void GenerateWaypoints()
        {
            waypoints = new List<Waypoint>();

            List<NavCell> navCells = _navGrid.GetPath(transform,  targetTransform,  cellSize, cellExtends);

            int curWaypoint = 1;

            foreach (NavCell navCell in navCells)
            {
                Waypoint newWaypoint = new Waypoint(navCell.Position, Quaternion.identity, true, $"Waypoint: {curWaypoint}");
                waypoints.Add(newWaypoint);
                curWaypoint++;

                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Destroy(sphere.GetComponent<Collider>());
                sphere.transform.localPosition =navCell.Position;
                sphere.transform.localScale = new Vector3(2, 2, 2);

                sphere.GetComponent<Renderer>().material.color = Color.yellow;

            }
        }
    }
}