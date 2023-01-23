using System;
using System.Collections.Generic;
using Architect.Placeables.Presets;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace Grid
{
    public class GridManager : SingletonBehaviour<GridManager>
    {
        public Tile tilePrefab;
        public TilePreset[] tilePresets;

        [SerializeField] private BuildingPreset wall;
        [SerializeField] private BuildingPreset gate;
        [SerializeField] private BuildingPreset corner;
        [SerializeField] private Vector2Int gridSize = new(5, 5);
        [SerializeField] private Vector2 tileWidth = new Vector2Int(1,1);

        private Tile[,] _grid;
        private Tile _hoverTile;

        public Tile HoverTile { get => _hoverTile; }

        void Start()
        {
            CreateGrid();
            PopulateGrid();
            PlaceWallsGrid();
        }

        private void Update()
        {
            _hoverTile = TryGetTileFromMousePos();
        }

        #region Initialize Grid Population
        /// <summary>
        /// Initialize the grid.
        /// </summary>
        private void CreateGrid()
        {
            _grid = new Tile[gridSize.x, gridSize.y];
        }

        /// <summary>
        /// Gets the tile at coords if it is within the grid.
        /// </summary>
        /// <param name="coords">The coordinates for the tile you want to get</param>
        /// <returns>The tile that is at the given coords. Null if tile was not found or not within the grid.</returns>
        internal Tile GetTileAt(Vector2Int coords)
        {
            if (coords.x >= 0 && coords.x < gridSize.x && coords.y >= 0 && coords.y < gridSize.y)
            {
                return _grid[coords.y, coords.x];
            }

            return null;
        }

        /// <summary>
        /// Populates the grid with Tile objects which instantiate a tileGameObject for every position in the Grid.
        /// </summary>
        private void PopulateGrid()
        {
            Vector3 startPosition = Vector3.zero + new Vector3(tileWidth.x / 2, 0, tileWidth.y / 2);
            Vector3 currentPosition = startPosition;
            // Treat y as z because in Unity we look along the Y axis to the tiles and the tiles are placed along the x and z axis
            for (int z = 0; z < gridSize.y; z++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    Tile newTile = Instantiate(tilePrefab, currentPosition, tilePrefab.transform.rotation, transform);
                    TilePreset preset = tilePresets[UnityEngine.Random.Range(0,tilePresets.Length)];
                    newTile.Initialize(new Vector2Int(x, z), preset, tileWidth);
                    _grid[z, x] = newTile;
                    currentPosition.x += tileWidth.x;
                }

                currentPosition.x = startPosition.x;
                currentPosition.z += tileWidth.y;
            }
        }

        private void PlaceWallsGrid()
        {
            foreach (var tile in _grid)
            {
                if ((tile.GridPosition.x == 0 || tile.GridPosition.x == gridSize.x - 1) && tile.GridPosition.y != Mathf.Ceil(gridSize.y / 2))
                {
                    tile.PlaceContent(new Building(wall), 0);
                } else if ((tile.GridPosition.y == 0 || tile.GridPosition.y == gridSize.y - 1) && tile.GridPosition.x != Mathf.Ceil(gridSize.x / 2))
                {
                    tile.PlaceContent(new Building(wall), 1);
                }
            }

            PlaceGates();
        }

        private void PlaceGates()
        {
            foreach (var tile in _grid)
            {
                if (tile.GridPosition.x == Mathf.Ceil(gridSize.x / 2) &&
                    (tile.GridPosition.y == 0 || tile.GridPosition.y == gridSize.y - 1))
                    tile.PlaceContent(new Building(gate), 0);
                if (tile.GridPosition.y == Mathf.Ceil(gridSize.y / 2) &&
                    (tile.GridPosition.x == 0 || tile.GridPosition.x == gridSize.x - 1))
                    tile.PlaceContent(new Building(gate), 1);
            }
        }

        #endregion

        #region Public Getters
        public Neighbour[] GetNeighboursFor(Tile tile)
        {
            List<Neighbour> neighbours = new List<Neighbour>();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (NeighbourDirectionIsAllowed(tile.GridPosition, direction))
                {
                    neighbours.Add(GetNeighbourInDirection(tile.GridPosition, direction));
                }
            }

            return neighbours.ToArray();
        }

        private Neighbour GetNeighbourInDirection(Vector2Int indices, Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return new Neighbour(GetTileAt(new Vector2Int(indices.x, indices.y + 1)), direction);
                case Direction.EAST:
                    return new Neighbour(GetTileAt(new Vector2Int(indices.x + 1, indices.y)), direction);
                case Direction.SOUTH:
                    return new Neighbour(GetTileAt(new Vector2Int(indices.x, indices.y - 1)), direction);
                case Direction.WEST:
                    return new Neighbour(GetTileAt(new Vector2Int(indices.x - 1, indices.y)), direction);
                default:
                    throw new ArgumentException("Unsupported direction was passed to this function.");
            }
        }

        private bool NeighbourDirectionIsAllowed(Vector2Int currentIndices, Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return currentIndices.y + 1 < gridSize.y;
                case Direction.EAST:
                    return currentIndices.x + 1 < gridSize.x;
                case Direction.SOUTH:
                    return currentIndices.y - 1 >= 0;
                case Direction.WEST:
                    return currentIndices.x - 1 >= 0;
                default:
                    throw new ArgumentException("Unsupported direction was passed to this function.");
            }
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Attempts to get the tile over which the mouse hovers
        /// </summary>
        /// <returns>A TileCoordinates struct that includes the coords and whether the coords were in bounds.</returns>
        public Tile TryGetTileFromMousePos()
        {
            RaycastHit hit;
            if (GetFloorPointFromMouse(out hit))
            {
                return GetTileAt(WorldPosToGridIndices(hit.point));
            }
            return null;
        }
        /// <summary>
        /// Converts the mouse Screen position to a world position 
        /// and than casts a ray to find if the mouse is hovering over a tile.
        /// </summary>
        /// <param name="hit">The RaycastHit struct to store the hit information in.</param>
        /// <returns>Whether a floor point was found.</returns>
        private bool GetFloorPointFromMouse(out RaycastHit hit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (EventSystem.current.IsPointerOverGameObject()) //stops casts when over hovering over GUI
            {
                hit = default;
                return false;
            }

            return Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Floor"));
        }

        /// <summary>
        /// Convert a world position to the corresponding tile indices.
        /// </summary>
        /// <param name="point">The world position point to convert.</param>
        /// <returns>The corresponding indices.</returns>
        private Vector2Int WorldPosToGridIndices(Vector3 point)
        {
            return new Vector2Int((int) (point.x / tileWidth.x), (int) (point.z / tileWidth.y));
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            if (Application.isPlaying)
            {
                // Draw tile gizmos
                foreach (Tile tile in _grid)
                {
                    Gizmos.color = tile == _hoverTile ? Color.yellow : Color.green;
                    Gizmos.DrawWireCube(tile.transform.position + new Vector3(0, tileWidth.y / 2, 0), new Vector3(tileWidth.x, tileWidth.y, tileWidth.y));
                }
            }

            // Draw point on floor below mouse
            RaycastHit hit;
            if (GetFloorPointFromMouse(out hit))
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(hit.point, .25f);
            }
        }
        #endregion

        /// <summary>
        /// A struct containing the neighbouring tile and in what
        /// direction this neighbour is connected to the host tile.
        /// </summary>
        public struct Neighbour
        {
            public Tile Tile;

            /// <summary>
            /// The direction in which this neighbour is connected to the host tile.
            /// </summary>
            public Direction InDirection;

            public Neighbour(Tile tile, Direction inDirection)
            {
                Tile = tile;
                InDirection = inDirection;
            }
        }

        /// <summary>
        /// Represents the directions a tile can attach to other tiles.
        /// In bitflag order of NESW.
        /// </summary>
        public enum Direction
        {
            NORTH = 1,
            EAST = 2,
            SOUTH = 4,
            WEST = 8
        }
    }
}