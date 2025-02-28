using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;
using Object = UnityEngine.Object;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    internal enum GenerateStatus
    {
        Idle,
        Generating,
        Success,
        Failed
    }

    /// <summary>
    /// Implements a dynamic 3D grid of cubes eminating from a "Source" position to a "Target" position.
    /// The grid extends forward all the way to the target, and "numExtends" side-to-side and vertically.
    /// Grid "NavCells" have a position, the center of the cube, and a boolean that is true if any colliders
    /// are present within that cube.
    ///
    /// The GetPath method returns true and a path (list of NavCells) based on a simple A* pathfinding algorithm.
    /// </summary>
    internal class NavGrid
    {
        // Internal NavGrid 3D array
        private NavCell[,,] _navGrid;
        private NavPath _navPath;

        private GenerateStatus _gridStatus = GenerateStatus.Idle;
        private GenerateStatus _pathStatus = GenerateStatus.Idle;

        internal NavPath NavPath => _navPath;

        private bool IsBusy => _gridStatus == GenerateStatus.Generating || _pathStatus == GenerateStatus.Generating;
        internal bool IsPathingReady => _gridStatus == GenerateStatus.Success && _pathStatus == GenerateStatus.Success;
        internal bool HasPathingFailed => _gridStatus == GenerateStatus.Failed || _pathStatus == GenerateStatus.Failed;

        internal GridStatusChangedEvent OnGridStatusChanged = new GridStatusChangedEvent();
        internal PathingStatusChangedEvent OnPathingStatusChanged = new PathingStatusChangedEvent();

        // Cached RayCastHits for collider checks
        private Collider[] _colliderHitCache;
        private int _colliderHitCacheSize = 100;
        private bool _isGridReady;

        internal NavGrid()
        {
            SetGridStatus(GenerateStatus.Idle);
            SetPathingStatus(GenerateStatus.Idle);

            _colliderHitCache = new Collider[_colliderHitCacheSize];
        }

        internal class GridStatusChangedEvent : UnityEvent<GenerateStatus>
        {
        }

        internal class PathingStatusChangedEvent : UnityEvent<GenerateStatus>
        {
        }

        private void SetGridStatus(GenerateStatus newStatus)
        {
            if (_gridStatus == newStatus)
            {
                return;
            }
            _gridStatus = newStatus;
            OnGridStatusChanged.Invoke(newStatus);
        }

        private void SetPathingStatus(GenerateStatus newStatus)
        {
            if (_pathStatus == newStatus)
            {
                return;
            }
            _pathStatus = newStatus;
            OnPathingStatusChanged.Invoke(newStatus);
        }

        // Only used in debugging. Keeps a list of cells so we can tweak the visualisers
        private Dictionary<NavCell, CellVisualiser> _debugCellVisualisers = new Dictionary<NavCell, CellVisualiser>();

        internal IEnumerator GetPathAsync(Vector3 sourcePosition, Vector3 targetPosition, float cellSize, int numCellExtends, LayerMask ignoreLayerMask,
            Action<GenerateStatus> gridCompleteAction = null, Action<GenerateStatus> pathCompleteAction = null,
            bool debug = false, Transform debugContainer = null)
        {
            if (IsBusy)
            {
                Log.LogWarning("NavGrid is busy!");
                pathCompleteAction?.Invoke(GenerateStatus.Generating);
                yield break;
            }

            if (_gridStatus != GenerateStatus.Success)
            {
                Log.LogWarning("NavGrid grid is not ready for pathing!");
                pathCompleteAction?.Invoke(GenerateStatus.Failed);
                yield break;
            }

            yield return GenerateNavGridAsync(sourcePosition, targetPosition, cellSize, numCellExtends, ignoreLayerMask, gridCompleteAction,
                debug, debugContainer);

            if (_gridStatus == GenerateStatus.Success)
            {
                yield return FindPathAsync(sourcePosition, targetPosition, pathCompleteAction, debug, debugContainer);
            }
            else
            {
                Log.LogError("Pathing failed!");
                SetPathingStatus(GenerateStatus.Failed);
            }

            SetPathingStatus(GenerateStatus.Success);
        }

        private Transform GetGridDebugContainer(Transform parentContainer)
        {
            Transform container = parentContainer.Find("DEBUG");
            return container;
        }

        private Transform ResetGridDebugContainer(Transform debugContainer)
        {
            return ResetDebugContainer(debugContainer, "GRID");
        }

        private Transform ResetDebugContainer(Transform parentContainer, string containerName)
        {
            Transform container = parentContainer.Find(containerName);
            if (container)
            {
                Object.Destroy(container.gameObject);
            }
            GameObject newContainer = new GameObject(containerName);
            newContainer.transform.SetParent(parentContainer, true);
            return newContainer.transform;
        }

        internal IEnumerator GenerateNavGridAsync(Vector3 sourcePosition, Vector3 targetPosition, float cellSize, int numCellExtends, LayerMask ignoreLayerMask,
            Action<GenerateStatus> gridCompleteAction = null,
           bool debug = false,  Transform debugContainer = null)
        {
            if (IsBusy)
            {
                Log.LogWarning("NavGrid is busy!");
                gridCompleteAction?.Invoke(GenerateStatus.Generating);
                yield break;
            }

            int totalCells = 0;
            int totalBlockedCells = 0;
            int totalClearCells = 0;

            _debugCellVisualisers = new Dictionary<NavCell, CellVisualiser>();
            Transform gridDebugContainer = debug ? ResetGridDebugContainer(debugContainer) : null;

            float genTime = Time.time;
            Log.LogDebug($"Started Grid Generation: {genTime}");
            SetGridStatus(GenerateStatus.Generating);

            if (sourcePosition == targetPosition)
            {
                Log.LogError("NavGrid: sourcePosition and targetPosition are the same!");
                SetGridStatus(GenerateStatus.Failed);
                gridCompleteAction?.Invoke(GenerateStatus.Failed);
                yield break;
            }

            Vector3 direction = (targetPosition - sourcePosition).normalized;
            float distance = Vector3.Distance(sourcePosition, targetPosition);
            int numCellsForward = Mathf.CeilToInt(distance / cellSize);

            Log.LogDebug($"Num Extends: {numCellExtends}");
            Log.LogDebug($"Cell Size: {cellSize}");
            Log.LogDebug($"NavGrid dimensions: x:{numCellsForward}, y:{numCellExtends * 2}, z:{numCellExtends * 2}. Total cells: {numCellsForward * (numCellExtends * 2)}");

            _navGrid = new NavCell[numCellsForward, (numCellExtends * 2) + 1, (numCellExtends * 2) + 1];

            Vector3 right = Vector3.Cross(direction, Vector3.up).normalized;
            Vector3 up = Vector3.Cross(right, direction).normalized;

            for (int x = 0; x < numCellsForward; x++)
            {
                for (int y = -numCellExtends; y <= numCellExtends; y++)
                {
                    for (int z = -numCellExtends; z <= numCellExtends; z++)
                    {
                        Vector3 cellPosition = sourcePosition + (direction * (x * cellSize)) + (up * (y * cellSize)) + (right * (z * cellSize));

                        // bool hasCollider = Physics.CheckBox(cellPosition, Vector3.one * (cellSize * 0.5f), Quaternion.identity, ~ignoreLayerMask);
                        // Check for colliders inside grid box
                        // RaycastHit[] hits = Physics.BoxCastAll(cellPosition, Vector3.one * (cellSize * 0.5f), Vector3.zero, Quaternion.identity, 0f, ~ignoreLayerMask, QueryTriggerInteraction.Ignore);
                        int numColliderHits = Physics.OverlapBoxNonAlloc(cellPosition, Vector3.one * (cellSize * 0.5f), _colliderHitCache, Quaternion.identity, ~ignoreLayerMask, QueryTriggerInteraction.Ignore);
                        bool hasCollider = HasValidColliders(numColliderHits, _colliderHitCache);

                        int cellYIndex = y + numCellExtends;
                        int cellZIndex = z + numCellExtends;

                        string cellName = $"(X:{x}, Y:{cellYIndex}, Z:{cellZIndex}";

                        _navGrid[x, cellYIndex, cellZIndex] = new NavCell { Position = cellPosition, HasColliders = hasCollider, Name = cellName };

                        totalCells++;

                        if (hasCollider)
                        {
                            totalBlockedCells++;
                        }
                        else
                        {
                            totalClearCells++;
                        }

                        if (debug)
                        {
                            GameObject newCellVis = new GameObject($"Cell Visualiser: {cellName}");
                            newCellVis.transform.SetParent(gridDebugContainer, true);
                            CellVisualiser cellVis = newCellVis.AddComponent<CellVisualiser>();
                            cellVis.CreateOrUpdate(_navGrid[x, cellYIndex, cellZIndex], CellType.NavCell, gridDebugContainer);
                            _debugCellVisualisers.Add(_navGrid[x, cellYIndex, cellZIndex], cellVis);
                        }
                    }

                    yield return null;
                }
            }
            Log.LogDebug($"Finished Grid Generation: {Time.time}. Time taken: {Time.time - genTime}");
            Log.LogDebug($"Cells created: {totalCells}, Blocked cells: {totalBlockedCells}, Clear cells: {totalClearCells}");
            SetGridStatus(GenerateStatus.Success);
            gridCompleteAction?.Invoke(GenerateStatus.Success);
        }

        private static bool HasValidColliders(int numColliders, Collider[] allColliders)
        {
            for (int curColliderIndex = 0; curColliderIndex < numColliders; curColliderIndex++)
            {
                Log.LogDebug($"Found collider: {allColliders[curColliderIndex].name} on layer named: {LayerMask.LayerToName(allColliders[curColliderIndex].gameObject.layer)}");
                if (allColliders[curColliderIndex].gameObject.transform.parent && allColliders[curColliderIndex].gameObject.transform.parent.GetComponentInChildren<Creature>())
                {
                    // We want to ignore these
                    Log.LogDebug("NavGrid: found Creature collider, ignoring...");
                    continue;
                }

                return true;
            }
            return false;
        }

        internal IEnumerator FindPathAsync(Vector3 startPos, Vector3 endPos,
            Action<GenerateStatus> pathCompleteAction = null,
            bool debug = false, Transform debugContainer = null)
        {

            if (IsBusy)
            {
                Log.LogWarning("NavGrid is busy!");
                pathCompleteAction?.Invoke(GenerateStatus.Generating);
                yield break;
            }

            if (_gridStatus != GenerateStatus.Success)
            {
                Log.LogWarning("NavGrid grid is not ready for pathing!");
                pathCompleteAction?.Invoke(GenerateStatus.Failed);
                yield break;
            }

            Transform pathDebugContainer = debug ? GetGridDebugContainer(debugContainer) : null;

            float genTime = Time.time;
            Log.LogDebug($"Started Path Generation: {genTime}");
            SetPathingStatus(GenerateStatus.Generating);

            _navPath = new NavPath();
            HashSet<NavCell> openSet = new HashSet<NavCell>();
            HashSet<NavCell> closedSet = new HashSet<NavCell>();
            Dictionary<NavCell, NavCell> cameFrom = new Dictionary<NavCell, NavCell>();
            Dictionary<NavCell, float> gScore = new Dictionary<NavCell, float>();
            Dictionary<NavCell, float> fScore = new Dictionary<NavCell, float>();

            NavCell startCell = FindClosestWalkableCell(startPos);
            NavCell targetCell = FindClosestWalkableCell(endPos);

            openSet.Add(startCell);
            gScore[startCell] = 0;
            fScore[startCell] = Heuristic(startCell.Position, targetCell.Position);

            while (openSet.Count > 0)
            {
                NavCell current = GetLowestFScore(openSet, fScore);

                if (current.Position == targetCell.Position)
                {
                    // Reached our destination, so pathing is complete
                    _navPath = ReconstructPath(cameFrom, current);

                    if (debug)
                    {
                        UpdatePathVisualisers(pathDebugContainer);
                    }

                    SetPathingStatus(GenerateStatus.Success);
                    pathCompleteAction?.Invoke(GenerateStatus.Success);
                    Log.LogDebug($"Finished Path Generation: {Time.time}. Time taken: {Time.time - genTime}");
                    yield break;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (NavCell neighbor in GetNeighbors(_navGrid, current))
                {
                    if (closedSet.Contains(neighbor) || neighbor.HasColliders)
                    {
                        continue;
                    }

                    float tentativeGScore = gScore[current] + Vector3.Distance(current.Position, neighbor.Position);

                    if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor.Position, targetCell.Position);
                        openSet.Add(neighbor);
                    }
                }

                yield return null;
            }

            // No path found
            Log.LogDebug($"Finished Path Generation: {Time.time}. Time taken: {Time.time - genTime}");
            SetPathingStatus(GenerateStatus.Failed);
            pathCompleteAction?.Invoke(GenerateStatus.Failed);
        }

        private void UpdatePathVisualisers(Transform pathDebugContainer)
        {
            for (int curCell = 0; curCell < _navPath.Count; curCell++)
            {
                // Start
                if (curCell == 0)
                {
                    _debugCellVisualisers[_navPath[curCell]].CreateOrUpdate(_navPath[curCell], CellType.Start, null);
                }
                else if (curCell == _navPath.Count - 1)
                {
                    _debugCellVisualisers[_navPath[curCell]].CreateOrUpdate(_navPath[curCell], CellType.End, null);
                }
                else
                {
                    _debugCellVisualisers[_navPath[curCell]].CreateOrUpdate(_navPath[curCell], CellType.Route, null);
                }
            }
        }

        private NavCell FindClosestWalkableCell(Vector3 position)
        {
            NavCell closest = _navGrid[0, 0, 0];
            float minDistance = float.MaxValue;
            bool foundWalkable = false;

            foreach (var cell in _navGrid)
            {
                if (cell.HasColliders || string.IsNullOrEmpty(cell.Name))
                {
                    continue; // Skip blocked cells
                }

                float dist = Vector3.Distance(position, cell.Position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = cell;
                    foundWalkable = true;
                }
            }

            if (foundWalkable)
            {
                return closest;
            }

            Log.LogError("Couldn't find closest walkable cell!");
            return _navGrid[0, 0, 0]; // Fallback to first cell if no walkable found
        }


        private static float Heuristic(Vector3 a, Vector3 b)
        {
            return Vector3.Distance(a, b);
        }

        private static NavCell GetLowestFScore(HashSet<NavCell> openSet, Dictionary<NavCell, float> fScore)
        {
            NavCell best = default;
            float minScore = float.MaxValue;

            foreach (var cell in openSet)
            {
                if (fScore[cell] < minScore)
                {
                    minScore = fScore[cell];
                    best = cell;
                }
            }

            return best;
        }

        private static List<NavCell> GetNeighbors(NavCell[,,] grid, NavCell cell)
        {
            List<NavCell> neighbors = new List<NavCell>();

            int gridX = grid.GetLength(0);
            int gridY = grid.GetLength(1);
            int gridZ = grid.GetLength(2);

            Vector3Int cellIndex = GetCellIndex(grid, cell);
            if (cellIndex == new Vector3Int(-1, -1, -1)) {
                return neighbors; // Cell not found in grid
}

            int[][] directions =
            {
                new[] { 1, 0, 0 }, new[] { -1, 0, 0 }, // Forward, Backward
                new[] { 0, 1, 0 }, new[] { 0, -1, 0 }, // Up, Down
                new[] { 0, 0, 1 }, new[] { 0, 0, -1 } // Left, Right
            };

            foreach (int[] dir in directions)
            {
                int newX = cellIndex.x + dir[0];
                int newY = cellIndex.y + dir[1];
                int newZ = cellIndex.z + dir[2];

                if (newX >= 0 && newX < gridX &&
                    newY >= 0 && newY < gridY &&
                    newZ >= 0 && newZ < gridZ)
                {
                    NavCell neighbor = grid[newX, newY, newZ];
                    if (!neighbor.HasColliders) // Only add walkable cells
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }


        private static Vector3Int GetCellIndex(NavCell[,,] grid, NavCell cell)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    for (int z = 0; z < grid.GetLength(2); z++)
                    {
                        if (grid[x, y, z].Position == cell.Position)
                        {
                            return new Vector3Int(x, y, z);
                        }
                    }
                }
            }

            return new Vector3Int(-1, -1, -1); // Not found
        }

        private static NavPath ReconstructPath(Dictionary<NavCell, NavCell> cameFrom, NavCell current)
        {
            NavPath path = new NavPath();
            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Reverse();
            return path;
        }
    }
}