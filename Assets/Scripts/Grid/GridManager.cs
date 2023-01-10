using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : SingletonBehaviour<GridManager>
{
    [field: SerializeField] public GameObject _tilePrefab;
    [SerializeField] public BuildingPreset wall;

    [SerializeField] private Vector2Int gridSize = new Vector2Int(5, 5);
    [SerializeField] private Vector2 tileWidth = new Vector2Int(1, 1);

    private Tile[,] grid;

    public Vector2Int Bounds
    {
        get { return new Vector2Int(gridSize.x, gridSize.y); }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        PopulateGrid();
        PlaceWallsGrid();
    }

    #region Grid Population

    /// <summary>
    /// Initialize the grid.
    /// </summary>
    private void CreateGrid()
    {
        grid = new Tile[gridSize.x, gridSize.y];
    }

    /// <summary>
    /// Gets the tile at coords.
    /// </summary>
    /// <param name="coords">The coordinates for the tile</param>
    /// <returns>The tile at coords. Null if tile was not found or not in bounds.</returns>
    internal Tile GetTileAt(Vector2Int coords)
    {
        //Debug.Log("getting tile at " + coords);
        if (coords.x >= 0 && coords.x < gridSize.x && coords.y >= 0 && coords.y < gridSize.y)
        {
            return grid[coords.y, coords.x];
        }

        return null;
    }

    /// <summary>
    /// Populates the grid with Tile objects.
    /// </summary>
    private void PopulateGrid()
    {
        Vector3 startPosition = Vector3.zero + new Vector3(tileWidth.x / 2, 0, tileWidth.y / 2);
        Vector3 currentPosition = startPosition;
        // Treat y as z 
        for (int indexZ = 0; indexZ < gridSize.y; indexZ++)
        {
            for (int indexX = 0; indexX < gridSize.x; indexX++)
            {
                Tile newTile = new Tile(_tilePrefab, currentPosition, new Vector2Int(indexX, indexZ));
                newTile.Root.parent = this.transform;
                grid[indexZ, indexX] = newTile;
                currentPosition.x += tileWidth.x;
            }

            currentPosition.x = startPosition.x;
            currentPosition.z += tileWidth.y;
        }
    }

    private void PlaceWallsGrid()
    {
        foreach (var tile in grid)
        {
            if (tile.Indices.x == 0 || tile.Indices.y == 0 ||
                tile.Indices.x == gridSize.x - 1 || tile.Indices.y == gridSize.y - 1)
            {
                tile.PlaceContent(new Building(wall), 0);
            }
        }
    }

    #endregion

    #region Public Getters

    /// <summary>
    /// Attempts to get the tile indeces from the hovered tile
    /// </summary>
    /// <returns>A TileCoordinates struct that includes the coords and whether the coords were in bounds.</returns>
    public TileCoordinates GetTileCoordsFromMousePos()
    {
        RaycastHit hit;
        if (GetFloorPointFromMouse(out hit))
        {
            return new TileCoordinates(true, WorldPosToGridIndices(hit.point));
        }

        return new TileCoordinates(false, new Vector2Int(-1, -1));
    }

    public Neighbour[] GetNeighboursFor(Tile tile)
    {
        List<Neighbour> neighbours = new List<Neighbour>();
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (NeighbourDirectionIsAllowed(tile.Indices, direction))
            {
                neighbours.Add(GetNeighbourInDirection(tile.Indices, direction));
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
    /// Get the current floor point under the hit.
    /// </summary>
    /// <param name="hit">The RaycastHit struct to store the hit information in.</param>
    /// <returns>Whether a floor point was found.</returns>
    public bool GetFloorPointFromMouse(out RaycastHit hit)
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

    #region Dev

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Get tile under mouse
        Tile hoverTile = null;
        TileCoordinates hoverTileIndices = GetTileCoordsFromMousePos();
        if (hoverTileIndices.inBounds)
        {
            hoverTile = grid[hoverTileIndices.indices.y, hoverTileIndices.indices.x];
        }

        if (Application.isPlaying)
        {
            // Draw tile gizmos
            foreach (Tile tile in grid)
            {
                Gizmos.color = tile == hoverTile ? Color.yellow : Color.green;
                Gizmos.DrawWireCube(tile.Root.position + new Vector3(0, tileWidth.y / 2, 0),
                    new Vector3(tileWidth.x, tileWidth.y, tileWidth.y));
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
    /// Struct that stores grid coordinates along with
    /// whether they are in bounds
    /// </summary>
    public struct TileCoordinates
    {
        /// <summary>
        /// Whether the coordinates are in bounds of the grid
        /// </summary>
        public bool inBounds;

        /// <summary>
        /// The index coordinates
        /// </summary>
        public Vector2Int indices;

        public TileCoordinates(bool exist, Vector2Int indices)
        {
            this.inBounds = exist;
            this.indices = indices;
        }
    }

    /// <summary>
    /// A struct containing the neighbouring tile and in what
    /// direction this neighbour is connected to the host tile.
    /// </summary>
    public struct Neighbour
    {
        public Tile tile;

        /// <summary>
        /// The direction in which this neighbour is connected to the host tile.
        /// </summary>
        public Direction inDirection;

        public Neighbour(Tile tile, Direction inDirection)
        {
            this.tile = tile;
            this.inDirection = inDirection;
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