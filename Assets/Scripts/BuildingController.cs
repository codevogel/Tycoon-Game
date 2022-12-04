using System;
using System.Collections.Generic;
using UnityEngine;
using static BuildRequest;
using static GridManager;
using static Road;

public class BuildingController : MonoBehaviour
{

    /// <summary>
    /// Reference to the static gridManager;
    /// </summary>
    private static GridManager gridManager;

    [Header("Placeable Buildings")]
    public List<ScriptableObject> placeableBuildings;

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
            if (targetTile.Content == null)
            {
                PlaceObjectAt(targetTile);
            }
            else
            {
                Debug.Log("Selected building: " + targetTile.Root.name);
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
            if (targetTile.Content == null)
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

    private void PlaceBuildingAt(Tile targetTile)
    {
        targetTile.Build(new BuildRequest(placeableBuildings[currentBuildingIndex], currentRotation));
    }

    private void PlaceRoadAt(Tile targetTile)
    {
        Neighbour[] neighbours = gridManager.GetNeighboursFor(targetTile);

        (Road road, int rotation) fittingPiece = GetFittingPiece(GetRoadConnectionFlag(neighbours));
        targetTile.Build(new BuildRequest(fittingPiece.road, fittingPiece.rotation));

        foreach (Neighbour neighbour in neighbours)
        {
            Neighbour[] theirNeighbours = gridManager.GetNeighboursFor(neighbour.tile);
            (Road road, int rotation) theirFittingPiece = GetFittingPiece(GetRoadConnectionFlag(theirNeighbours));
            neighbour.tile.Build(new BuildRequest(theirFittingPiece.road, theirFittingPiece.rotation));
        }
    }

    private int GetRoadConnectionFlag(Neighbour[] neighbours)
    {
        int bitwiseFlag = 0;
        foreach (Neighbour neighbour in neighbours)
        {
            bitwiseFlag += (int)neighbour.inDirection;
        }
        return bitwiseFlag;
    }

    private (Road road, int rotation) GetFittingPiece(int roadConnectionFlag) => roadConnectionFlag switch
    {
        0b00000000 => (new Road(RoadType.CROSS), 0),
        0b00000001 => (new Road(RoadType.END), 0),
        0b00000010 => (new Road(RoadType.END), 1),
        0b00000011 => (new Road(RoadType.CORNER), 0),
        0b00000100 => (new Road(RoadType.END), 2),
        0b00000101 => (new Road(RoadType.STRAIGHT), 0),
        0b00000111 => (new Road(RoadType.TJUNC), 0),
        0b00001000 => (new Road(RoadType.END), 3),
        0b00001001 => (new Road(RoadType.CORNER), 3),
        0b00001010 => (new Road(RoadType.STRAIGHT), 1),
        0b00001011 => (new Road(RoadType.TJUNC), 3),
        0b00001100 => (new Road(RoadType.CORNER), 2),
        0b00001101 => (new Road(RoadType.TJUNC), 2),
        0b00001110 => (new Road(RoadType.TJUNC), 1),
        0b00001111 => (new Road(RoadType.CROSS), 0),
        _ => throw new NotImplementedException("Invalid road connection flag: " + roadConnectionFlag)
    };

    private void RemoveObjectAt(Tile targetTile)
    {
    }
}
