using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.SeatruckRecall_BZ.Navigation
{
    public class NavGrid
    {
        public List<NavCell> GetPath(Transform sourceTransform, Transform targetTransform, float cellSize, int numCellExtends)
        {
            NavCell[,,] navGrid = GenerateNavGrid(sourceTransform, targetTransform, cellSize, numCellExtends);
            List<NavCell> path = FindPath(navGrid, sourceTransform.position, targetTransform.position);
            return path;
        }

        private static NavCell[,,] GenerateNavGrid(Transform sourceTransform, Transform targetTransform, float cellSize, int numCellExtends)
        {
            Vector3 direction = (targetTransform.position - sourceTransform.position).normalized;
            float distance = Vector3.Distance(sourceTransform.position, targetTransform.position);
            int numCellsForward = Mathf.CeilToInt(distance / cellSize);
            int numCellsSide = numCellExtends;
            int numCellsVertical = numCellExtends;

            NavCell[,,] grid = new NavCell[numCellsForward + 1, numCellsSide + 1, numCellsVertical + 1];

            Vector3 right = Vector3.Cross(direction, Vector3.up).normalized;
            Vector3 up = Vector3.Cross(right, direction).normalized;

            for (int x = 0; x <= numCellsForward; x++)
            {
                for (int y = -numCellsVertical / 2; y <= numCellsVertical / 2; y++)
                {
                    for (int z = -numCellsSide / 2; z <= numCellsSide / 2; z++)
                    {
                        Vector3 cellPosition = sourceTransform.position + (direction * x * cellSize) + (up * y * cellSize) + (right * z * cellSize);

                        bool hasCollider = Physics.CheckBox(cellPosition, Vector3.one * (cellSize * 0.5f));

                        string cellName = $"(X:{x}, Y:{y + numCellsVertical / 2}, Z:{z + numCellsSide / 2}";

                        grid[x, y + numCellsVertical / 2, z + numCellsSide / 2] = new NavCell { Position = cellPosition, hasColliders = hasCollider, Name = cellName };
                        GameObject newCellVis = new GameObject($"Cell Visualiser: {cellName}");
                        CellVisualiser cellVis = newCellVis.AddComponent<CellVisualiser>();
                        cellVis.Init(grid[x, y + numCellsVertical / 2, z + numCellsSide / 2]);
                    }
                }
            }

            return grid;
        }

        public static List<NavCell> FindPath(NavCell[,,] grid, Vector3 startPos, Vector3 endPos)
        {
            List<NavCell> path = new List<NavCell>();
            HashSet<NavCell> openSet = new HashSet<NavCell>();
            HashSet<NavCell> closedSet = new HashSet<NavCell>();
            Dictionary<NavCell, NavCell> cameFrom = new Dictionary<NavCell, NavCell>();
            Dictionary<NavCell, float> gScore = new Dictionary<NavCell, float>();
            Dictionary<NavCell, float> fScore = new Dictionary<NavCell, float>();

            NavCell startCell = FindClosestWalkableCell(grid, startPos);
            Debug.Log($"Starting Cell at: {startCell.Position}");
            NavCell targetCell = FindClosestWalkableCell(grid, endPos);
            Debug.Log($"Target Cell at: {targetCell.Position}");

            openSet.Add(startCell);
            gScore[startCell] = 0;
            fScore[startCell] = Heuristic(startCell.Position, targetCell.Position);

            while (openSet.Count > 0)
            {
                // Debug.Log($"Openset: {openSet.Count}");

                NavCell current = GetLowestFScore(openSet, fScore);

                if (current.Position == targetCell.Position)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (NavCell neighbor in GetNeighbors(grid, current))
                {
                    if (closedSet.Contains(neighbor) || neighbor.hasColliders)
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
            }

            return path; // Return an empty path if no path found
        }

        private static NavCell FindClosestCell(NavCell[,,] grid, Vector3 position)
        {
            NavCell closest = grid[0, 0, 0];
            float minDistance = float.MaxValue;

            foreach (var cell in grid)
            {
                float dist = Vector3.Distance(position, cell.Position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = cell;
                }
            }

            return closest;
        }

        private static NavCell FindClosestWalkableCell(NavCell[,,] grid, Vector3 position)
        {
            NavCell closest = grid[0, 0, 0];
            float minDistance = float.MaxValue;
            bool foundWalkable = false;

            foreach (var cell in grid)
            {
                if (cell.hasColliders || string.IsNullOrEmpty(cell.Name))
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

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = closest.Position;
            sphere.transform.localScale = new Vector3(2f, 2f, 2f);
            sphere.GetComponent<Renderer>().material.color = Color.blue;


            return foundWalkable ? closest : grid[-1, -1, -1]; // Fallback to first cell if no walkable found
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
                new int[] { 1, 0, 0 }, new int[] { -1, 0, 0 }, // Forward, Backward
                new int[] { 0, 1, 0 }, new int[] { 0, -1, 0 }, // Up, Down
                new int[] { 0, 0, 1 }, new int[] { 0, 0, -1 } // Left, Right
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
                    if (!neighbor.hasColliders) // Only add walkable cells
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


        private static List<NavCell> ReconstructPath(Dictionary<NavCell, NavCell> cameFrom, NavCell current)
        {
            List<NavCell> path = new List<NavCell>();
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