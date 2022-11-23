using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public GameObject tilePrefab;
    public Vector2Int gridSize;
    public Vector2 tileWidth;

    public Tile[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        PopulateGrid();
    }

    private void CreateGrid()
    {
        grid = new Tile[gridSize.x, gridSize.y];
    }

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

    public bool GetFloorPointFromMouse(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Floor"));
    }

    public Vector2Int GetCoordsFromMousePos()
    {
        RaycastHit hit;
        if (GetFloorPointFromMouse(out hit))
        {
            return WorldPosToGridCoords(hit.point);
        }
        return new Vector2Int(-1, -1);
    }

    private Vector2Int WorldPosToGridCoords(Vector3 point)
    {
        return new Vector2Int((int)(point.x / tileWidth.x), (int)(point.z / tileWidth.y));

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Tile hoverTile = null;
        Vector2Int hoverTileIndices = GetCoordsFromMousePos();
        if (hoverTileIndices.x > -1 && hoverTileIndices.y > -1)
        {
            hoverTile = grid[hoverTileIndices.y, hoverTileIndices.x];
        }

        if (Application.isPlaying)
        {
            foreach (Tile tile in grid)
            {
                Gizmos.color = tile == hoverTile ? Color.yellow : Color.green;
                Gizmos.DrawWireCube(tile.go.transform.position + new Vector3(0, tileWidth.y / 2, 0), new Vector3(tileWidth.x, tileWidth.y, tileWidth.y));
            }
        }

        RaycastHit hit;
        if (GetFloorPointFromMouse(out hit))
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(hit.point, .25f);
        }
    }
}
