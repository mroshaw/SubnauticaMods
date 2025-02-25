using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static DaftAppleGames.SeatruckRecall_BZ.SeaTruckDockRecallPlugin;

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

        internal NavGrid()
        {
            SetGridStatus(GenerateStatus.Idle);
            SetPathingStatus(GenerateStatus.Idle);
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

        internal IEnumerator GetPathAsync(Vector3 sourcePosition, Vector3 targetPosition, float cellSize, int numCellExtends, bool debug = false)
        {
            if (IsBusy)
            {
                Log.LogWarning("NavGrid is busy!");
                SetPathingStatus(GenerateStatus.Failed);
                yield break;
            }

            yield return GenerateNavGridAsync(sourcePosition, targetPosition, cellSize, numCellExtends, debug);

            if (_gridStatus == GenerateStatus.Success)
            {
                yield return FindPathAsync(sourcePosition, targetPosition, debug);
            }
            else
            {
                Log.LogError("Pathing failed!");
                SetPathingStatus(GenerateStatus.Failed);
            }

            SetPathingStatus(GenerateStatus.Success);
        }

        private IEnumerator GenerateNavGridAsync(Vector3 sourcePosition, Vector3 targetPosition, float cellSize, int numCellExtends, bool debug = false)
        {
            float genTime = Time.time;
            Log.LogDebug($"Started Grid Generation: {genTime}");
            SetGridStatus(GenerateStatus.Generating);

            if (_navGrid != null && _navGrid.Length > 0)
            {
                Log.LogWarning("NavGrid has already been created.");
                SetGridStatus(GenerateStatus.Success);
                yield break;
            }

            if (sourcePosition == targetPosition)
            {
                Log.LogError("NavGrid: sourcePosition and targetPosition are the same!");
                SetGridStatus(GenerateStatus.Failed);
                yield break;
            }

            Vector3 direction = (targetPosition - sourcePosition).normalized;
            float distance = Vector3.Distance(sourcePosition, targetPosition);
            int numCellsForward = Mathf.CeilToInt(distance / cellSize);

            _navGrid = new NavCell[numCellsForward, (numCellExtends * 2) + 1, (numCellExtends * 2) + 1];

            Vector3 right = Vector3.Cross(direction, Vector3.up).normalized;
            Vector3 up = Vector3.Cross(right, direction).normalized;

            Log.LogDebug($"Num Extends: {numCellExtends}");
            Log.LogDebug($"Cell Size: {cellSize}");

            for (int x = 0; x < numCellsForward; x++)
            {
                for (int y = -numCellExtends; y <= numCellExtends; y++)
                {
                    for (int z = -numCellExtends; z <= numCellExtends; z++)
                    {
                        Vector3 cellPosition = sourcePosition + (direction * (x * cellSize)) + (up * (y * cellSize)) + (right * (z * cellSize));

                        bool hasCollider = Physics.CheckBox(cellPosition, Vector3.one * (cellSize * 0.5f));

                        int cellYIndex = y + numCellExtends;
                        int cellZIndex = z + numCellExtends;

                        string cellName = $"(X:{x}, Y:{cellYIndex}, Z:{cellZIndex}";

                        _navGrid[x, cellYIndex, cellZIndex] = new NavCell { Position = cellPosition, HasColliders = hasCollider, Name = cellName };

                        if (debug)
                        {
                            GameObject newCellVis = new GameObject($"Cell Visualiser: {cellName}");
                            CellVisualiser cellVis = newCellVis.AddComponent<CellVisualiser>();
                            cellVis.CreateOrUpdate(_navGrid[x, cellYIndex, cellZIndex], CellType.NavCell);
                            _debugCellVisualisers.Add(_navGrid[x, cellYIndex, cellZIndex], cellVis);
                        }
                    }

                    yield return null;
                }
            }
            Log.LogDebug($"Finished Grid Generation: {Time.time}. Time taken: {Time.time - genTime}");
            SetGridStatus(GenerateStatus.Success);
        }

        private IEnumerator FindPathAsync(Vector3 startPos, Vector3 endPos, bool debug = false)
        {
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
                        UpdatePathVisualisers();
                    }

                    SetPathingStatus(GenerateStatus.Success);
                    Log.LogDebug($"Finished Path Generation: {Time.time}. Time taken: {Time.time - genTime}");
                    yield break;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (NavCell neighbor in GetNeighbors(_navGrid, current))
                {
                    if (closedSet.Contains(neighbor) || neighbor.HasColliders)
                        continue;

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
        }

        private void UpdatePathVisualisers()
        {
            for (int curCell = 0; curCell < _navPath.Count; curCell++)
            {
                // Start
                if (curCell == 0)
                {
                    _debugCellVisualisers[_navPath[curCell]].CreateOrUpdate(_navPath[curCell], CellType.Start);
                }
                else if (curCell == _navPath.Count - 1)
                {
                    _debugCellVisualisers[_navPath[curCell]].CreateOrUpdate(_navPath[curCell], CellType.End);
                }
                else
                {
                    _debugCellVisualisers[_navPath[curCell]].CreateOrUpdate(_navPath[curCell], CellType.Route);
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
            return foundWalkable ? closest : _navGrid[-1, -1, -1]; // Fallback to first cell if no walkable found
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
            if (cellIndex == new Vector3Int(-1, -1, -1)) return neighbors; // Cell not found in grid

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
                        neighbors.Add(neighbor);
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