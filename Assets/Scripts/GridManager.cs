using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : SingletonBehaviour<GridManager>
{

    public GameObject tilePrefab;
    public Vector2Int gridSize;
    public Vector2 tileWidth;

    private Tile[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        PopulateGrid();
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
                Tile newTile = new Tile(Instantiate(tilePrefab, currentPosition, Quaternion.identity, this.transform));
                grid[indexZ, indexX] = newTile;
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
    public TileCoordinates GetTileCoordsFromMousePos()
    {
        RaycastHit hit;
        if (GetFloorPointFromMouse(out hit))
        {
            return new TileCoordinates(true, WorldPosToGridIndices(hit.point));
        }
        return new TileCoordinates(false, new Vector2Int(-1, -1));
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
                Gizmos.DrawWireCube(tile.TileObject.transform.position + new Vector3(0, tileWidth.y / 2, 0), new Vector3(tileWidth.x, tileWidth.y, tileWidth.y));
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
}
