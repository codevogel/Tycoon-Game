using System;
using System.Collections.Generic;
using UnityEngine;
using static GridManager;

public class BuildingController : MonoBehaviour
{

    /// <summary>
    /// Reference to the static gridManager;
    /// </summary>
    private static GridManager gridManager;

    [Header("Placeable Buildings")]
    public List<GameObject> placeableBuildings;

    [Tooltip("Index of building being placed")]
    public int currentBuildingIndex = 0;
    [Tooltip("Current rotation offset of the placed buildings")]
    public int currentRotation = 0;

    // Amount of degrees to turn buildings each step.
    private int rotationStep = 90;

    public bool placingRoads;

    [Header("Road Prefabs")]
    [SerializeField]
    private List<GameObject> roadPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        // Get reference after instance creation in Awake
        gridManager = GridManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AttemptToPlaceObject();
        }
        if (Input.GetMouseButtonDown(1))
        {
            AttemptToRemoveObject();
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            IncrementBuildingPlacementRotation();

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectNextBuilding();
        }
    }


    /// <summary>
    /// Attempts to place a building at the tile under the mouse.
    /// </summary>
    private void AttemptToPlaceObject()
    {
        // Get hovered tile coords
        TileCoordinates coords = gridManager.GetTileCoordsFromMousePos();
        if (coords.inBounds)
        {
            Tile targetTile = gridManager.GetTileAt(coords.indices);
            if (targetTile.Object == null)
            {
                PlaceObjectAt(targetTile);
            }
            else
            {
                Debug.Log("Selected building: " + targetTile.Object.name);
            }
        }
    }

    /// <summary>
    /// Attempts to remove a building at the tile under the mouse.
    /// </summary>
    private void AttemptToRemoveObject()
    {
        TileCoordinates coords = gridManager.GetTileCoordsFromMousePos();
        if (coords.inBounds)
        {
            Tile targetTile = gridManager.GetTileAt(coords.indices);
            if (targetTile.Object == null)
            {
                Debug.Log("Tried removing a building that did not exist");
            }
            else
            {
                RemoveObjectAt(targetTile);
            }
        }
    }

    /// <summary>
    /// Rotates the current building angle by rotationStep.
    /// </summary>
    private void IncrementBuildingPlacementRotation()
    {
        currentRotation = (currentRotation + rotationStep) % 360;
    }

    /// <summary>
    /// Increments (and rolls-over) the currentBuildingIndex.
    /// </summary>
    private void SelectNextBuilding()
    {
        currentBuildingIndex = (currentBuildingIndex + 1) % placeableBuildings.Count;
    }

    /// <summary>
    /// Places the currently selected building at targetTile.
    /// </summary>
    /// <param name="targetTile">The tile to place the building on.</param>
    private void PlaceObjectAt(Tile targetTile)
    {
        if (placingRoads)
        {
            PlaceRoadAt(targetTile);
        }
        else
        {
            PlaceBuildingAt(targetTile);
        }
    }

    private void PlaceRoadAt(Tile targetTile)
    {
        GameObject fittingRoadPiece = FindFittingPieceAndUpdateNeighbours(targetTile);

        //Transform newRoad = Instantiate(fittingRoadPiece, targetTile.ObjectHolder).transform;
        //newRoad.transform.localEulerAngles = new Vector3(0, currentRotation, 0);
        //targetTile.Object = newRoad.gameObject;
    }

    private GameObject FindFittingPieceAndUpdateNeighbours(Tile targetTile)
    {
        Neighbour[] neighbours = gridManager.GetNeighboursFor(targetTile);

        // Set bitwise flag for connected neighbouring roads using enum values
        int roadConnectionFlag = GetRoadConnectionFlag(neighbours);
        Debug.Log(Convert.ToString(roadConnectionFlag, 2));
        return null;
    }

    private int GetRoadConnectionFlag(Neighbour[] neighbours)
    {
        int bitwiseFlag = 0;
        foreach (Neighbour neighbour in neighbours)
        {
            bitwiseFlag += (int)neighbour.direction;

            //if (neighbour.tile.Object.CompareTag("Road"))
            //{
            //    bitwiseFlag += (int)neighbour.direction;
            //}
        }
        return bitwiseFlag;
    }

    private void PlaceBuildingAt(Tile targetTile)
    {
        Transform newBuilding = Instantiate(placeableBuildings[currentBuildingIndex], targetTile.ObjectHolder).transform;
        newBuilding.transform.localEulerAngles = new Vector3(0, currentRotation, 0);
        targetTile.Object = newBuilding.gameObject;
    }

    /// <summary>
    /// Removes the building at targetTile.
    /// </summary>
    /// <param name="targetTile">The tile to remove the building on.</param>
    private void RemoveObjectAt(Tile targetTile)
    {
        Destroy(targetTile.ObjectHolder.GetChild(0).gameObject);
        targetTile.Object = null;
    }
}
