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
        Vector3 startPosition = Vector3.zero;
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


    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (Tile tile in grid)
            {
                Gizmos.DrawWireCube(tile.go.transform.position + new Vector3(0, tileWidth.y / 2, 0), new Vector3(tileWidth.x, tileWidth.y, tileWidth.y));
            }
        }
    }
}
