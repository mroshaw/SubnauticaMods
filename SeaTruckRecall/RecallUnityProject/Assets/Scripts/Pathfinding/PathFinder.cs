using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal class PathFinder : MonoBehaviour
    {
        [SerializeField] private Transform targetTransformOverride;
        [SerializeField] private float navGridCellSizeOverride = 10f;
        [SerializeField] private int navGridCellExtendsOverride = 5;
        [SerializeField] private LayerMask navGridOverrideIgnoreLayerMask;
        [SerializeField] private bool navGridDebugOverride = true;
        [SerializeField] private Transform navGridDebugContainerOverride;

        private NavGrid _navGrid;
        private GenerateStatus _waypointStatus;
        internal List<Waypoint> Waypoints { get; private set; }

        internal NavGrid.GridStatusChangedEvent OnGridStatusChanged = new NavGrid.GridStatusChangedEvent();
        internal NavGrid.PathingStatusChangedEvent OnPathingStatusChanged = new NavGrid.PathingStatusChangedEvent();
        internal WaypointsStatusChangedEvent OnWaypointStatusChanged = new WaypointsStatusChangedEvent();

        internal class WaypointsStatusChangedEvent : UnityEvent<GenerateStatus>
        {
        }

        private void OnEnable()
        {
            if (_navGrid == null)
            {
                _navGrid = new NavGrid();
            }
            _navGrid.OnPathingStatusChanged.AddListener(PathingStatusChangedHandler);
            _navGrid.OnGridStatusChanged.AddListener(GridStatusChangedHandler);
            _navGrid.OnPathingStatusChanged.AddListener(PathingStatusChangedHandler);
        }

        private void OnDisable()
        {
            _navGrid.OnPathingStatusChanged.RemoveListener(PathingStatusChangedHandler);
            _navGrid.OnGridStatusChanged.RemoveListener(GridStatusChangedHandler);
            _navGrid.OnPathingStatusChanged.RemoveListener(PathingStatusChangedHandler);
        }

        private void Start()
        {
            SetWaypointsStatus(GenerateStatus.Idle);
        }

        private void SetWaypointsStatus(GenerateStatus newStatus)
        {
            if (_waypointStatus == newStatus)
            {
                return;
            }
            _waypointStatus = newStatus;
            OnWaypointStatusChanged.Invoke(newStatus);
        }

        private void GridStatusChangedHandler(GenerateStatus status)
        {
            OnGridStatusChanged.Invoke(status);
        }

        private void PathingStatusChangedHandler(GenerateStatus status)
        {
            OnPathingStatusChanged?.Invoke(status);
            if (status == GenerateStatus.Success)
            {
                SetWaypointsFromPath(_navGrid.NavPath);
                SetWaypointsStatus(GenerateStatus.Success);
            }
            if(status == GenerateStatus.Failed)
            {
                SetWaypointsStatus(GenerateStatus.Failed);
            }
        }

        internal void GenerateWaypoints(Vector3 sourcePosition, Vector3 targetPosition, float cellSize, int cellExtends, LayerMask ignoreLayerMask,
            Action<GenerateStatus> gridCompleteAction = null, Action<GenerateStatus> pathCompleteAction = null, Action<GenerateStatus, List<Waypoint>> waypointsCompleteAction = null,
            bool debug = false, Transform debugContainer = null)
        {
            StartCoroutine(GenerateWaypointsAsync(sourcePosition, targetPosition, cellSize, cellExtends, ignoreLayerMask, gridCompleteAction, pathCompleteAction, waypointsCompleteAction,
                debug, debugContainer));
        }

        private IEnumerator GenerateWaypointsAsync(Vector3 sourcePosition, Vector3 targetPosition, float cellSize, int cellExtends, LayerMask ignoreLayerMask,
            Action<GenerateStatus> gridCompleteAction = null, Action<GenerateStatus> pathCompleteAction = null, Action<GenerateStatus, List<Waypoint>> waypointsCompleteAction = null,
            bool debug = false, Transform debugContainer = null)
        {
            SetWaypointsStatus(GenerateStatus.Generating);
            yield return _navGrid.GetPathAsync(sourcePosition, targetPosition, cellSize, cellExtends, ignoreLayerMask, gridCompleteAction, pathCompleteAction,
                debug, debugContainer);
            if (_navGrid.IsPathingReady)
            {
                SetWaypointsFromPath(_navGrid.NavPath);
                waypointsCompleteAction?.Invoke(GenerateStatus.Success, Waypoints);
            }
            else
            {
                waypointsCompleteAction?.Invoke(GenerateStatus.Failed, null);
            }
        }

        internal void RefreshNavGrid()
        {
            RefreshNavGrid(transform.position, targetTransformOverride.position, navGridCellSizeOverride, navGridCellExtendsOverride,navGridOverrideIgnoreLayerMask,
                navGridDebugOverride, navGridDebugContainerOverride);
        }

        private void RefreshNavGrid(Vector3 sourcePosition, Vector3 targetPosition, float cellSize, int cellExtends, LayerMask ignoreLayerMask,
            bool debug = false, Transform debugContainer = null)
        {
            StartCoroutine(_navGrid.GenerateNavGridAsync(sourcePosition, targetPosition, cellSize, cellExtends, ignoreLayerMask, null,
                debug, debugContainer));
        }

        internal void RefreshPath()
        {
            RefreshPath(transform.position, targetTransformOverride.position, navGridCellSizeOverride, navGridCellExtendsOverride,navGridOverrideIgnoreLayerMask,
                navGridDebugOverride, navGridDebugContainerOverride);
        }

        private void RefreshPath(Vector3 sourcePosition, Vector3 targetPosition, float cellSize, int cellExtends, LayerMask ignoreLayerMask,
            bool debug = false, Transform debugContainer = null)
        {
            StartCoroutine(_navGrid.FindPathAsync(sourcePosition, targetPosition, null, debug, debugContainer));
        }

        /// <summary>
        /// Overload to use Unity serialized values for testing
        /// </summary>
        /// <returns></returns>
        internal void GenerateWaypoints(Action<GenerateStatus, List<Waypoint>> waypointCompleteAction = null)
        {
            GenerateWaypoints(transform.position, targetTransformOverride.position, navGridCellSizeOverride, navGridCellExtendsOverride, navGridOverrideIgnoreLayerMask,
                null, null, waypointCompleteAction,
                navGridDebugOverride, navGridDebugContainerOverride);
        }

        /// <summary>
        /// Try to establish a path from source to target, return as a list of Waypoints
        /// </summary>
        private void SetWaypointsFromPath(NavPath navPath, bool debug = false)
        {
            Waypoints = new List<Waypoint>();
            int curWaypoint = 1;
            foreach (NavCell navCell in navPath)
            {
                Waypoints.Add(CreateWaypointFromNavCell(navCell, curWaypoint));
                curWaypoint++;
            }
        }

        private Waypoint CreateWaypointFromNavCell(NavCell navCell, int waypointIndex)
        {
            Waypoint newWaypoint = new Waypoint(navCell.Position, Quaternion.identity, true, $"Waypoint: {waypointIndex}");
            return newWaypoint;
        }
    }
}