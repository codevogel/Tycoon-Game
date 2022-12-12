using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : SingletonBehaviour<GridManager>
{
    public Tile _tilePrefab;

    [SerializeField]
    private Vector2Int gridSize = new Vector2Int(5, 5);
    [SerializeField]
    private Vector2 tileWidth = new Vector2Int(1,1);

    private Tile[,] grid;

    void Start()
    {
        CreateGrid();
        PopulateGrid();
    }

    #region Initialize Grid Population
    /// <summary>
    /// Initialize the grid.
    /// </summary>
    private void CreateGrid()
    {
        grid = new Tile[gridSize.x, gridSize.y];
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
            return grid[coords.y, coords.x];
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
                Tile newTile = Instantiate(_tilePrefab, currentPosition, _tilePrefab.transform.rotation, transform);
                newTile.GridPosition = new Vector2Int(x, z);
                grid[z, x] = newTile;
                currentPosition.x += tileWidth.x;
            }
            currentPosition.x = startPosition.x;
            currentPosition.z += tileWidth.y;
        }
    }
    #endregion

    #region Public Getters
    /// <summary>
    /// Attempts to get the tile indeces from the hovered tile
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
    /// Converts the mouse Screen position to a world position 
    /// and than casts a ray to find if the mouse is hovering over a tile.
    /// </summary>
    /// <param name="hit">The RaycastHit struct to store the hit information in.</param>
    /// <returns>Whether a floor point was found.</returns>
    public bool GetFloorPointFromMouse(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Floor"));
    }

    /// <summary>
    /// Convert a world position to the corresponding tile indices.
    /// </summary>
    /// <param name="point">The world position point to convert.</param>
    /// <returns>The corresponding indices.</returns>
    private Vector2Int WorldPosToGridIndices(Vector3 point)
    {
        return new Vector2Int((int)(point.x / tileWidth.x), (int)(point.z / tileWidth.y));
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Get tile under mouse
        Tile hoverTile = TryGetTileFromMousePos();

        if (Application.isPlaying)
        {
            // Draw tile gizmos
            foreach (Tile tile in grid)
            {
                Gizmos.color = tile == hoverTile ? Color.yellow : Color.green;
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
        NORTH = 1, EAST = 2, SOUTH = 4, WEST = 8
    }
}
