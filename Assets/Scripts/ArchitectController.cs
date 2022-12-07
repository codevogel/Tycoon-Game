using System;
using System.Collections.Generic;
using UnityEngine;
using static BuildRequest;
using static GridManager;
using static Road;

public class ArchitectController : SingletonBehaviour<ArchitectController>
{
    [field: SerializeField]
    public List<BuildingPreset> PlaceableBuildings { get; set; }

    [Tooltip("Index of building being placed")]
    [field: SerializeField]
    private int CurrentBuildingIndex { get; set; }

    [Tooltip("Current rotation offset of the placed buildings")]
    private int CurrentRotation { get; set; }

    // Amount of degrees to turn buildings each step.
    private int RotationStep { get; set; } = 90;

    [field: SerializeField]
    public PlaceableType CurrentlyPlacing { get; private set; }

    [field: SerializeField]
    public List<RoadPreset> Roads;

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
        TileCoordinates coords = GridManager.Instance.GetTileCoordsFromMousePos();
        if (coords.inBounds)
        {
            Tile targetTile = GridManager.Instance.GetTileAt(coords.indices);
            if (targetTile.HasContent)
            {
                Debug.Log("Selected building: " + targetTile.Root.name);
            }
            else
            {
                PlaceObjectAt(targetTile);
            }
        }
    }

    private void PlaceObjectAt(Tile targetTile)
    {
        targetTile.PlaceContent(GetCurrentPlaceable(), rotation: CurrentRotation);
    }

    private Placeable GetCurrentPlaceable()
    {
        return CurrentlyPlacing switch
        {
            PlaceableType.BUILDING => new Building(PlaceableBuildings[CurrentBuildingIndex]),
            PlaceableType.ROAD => new Road(),
            _ => throw new KeyNotFoundException("Did not find PlaceableType" + CurrentlyPlacing)
        };
    }

    /// <summary>
    /// Attempts to remove a building at the tile under the mouse.
    /// </summary>
    private void AttemptToRemoveObject()
    {
        TileCoordinates coords = GridManager.Instance.GetTileCoordsFromMousePos();
        if (coords.inBounds)
        {
            Tile targetTile = GridManager.Instance.GetTileAt(coords.indices);
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
        CurrentRotation = (CurrentRotation + RotationStep) % 360;
    }

    /// <summary>
    /// Increments (and rolls-over) the currentBuildingIndex.
    /// </summary>
    private void SelectNextBuilding()
    {
        CurrentBuildingIndex = (CurrentBuildingIndex + 1) % PlaceableBuildings.Count;
    }



    private void RemoveObjectAt(Tile targetTile)
    {
    }
}
