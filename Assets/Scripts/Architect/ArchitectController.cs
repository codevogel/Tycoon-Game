using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GridManager;
using static Placeable;
using static Road;

public class ArchitectController : SingletonBehaviour<ArchitectController>
{
    /// <summary>
    /// List containing presets of the available buildings.
    /// </summary>
    [field: SerializeField]
    private List<BuildingPreset> PlaceableBuildings { get; set; }

    /// <summary>
    /// Current index of preset of building to place.
    /// </summary>
    private int _currentBuildingIndex = 0;
    private int _currentRotation = 0;

    /// <summary>
    /// Type which the ArchitectController should currently place.
    /// </summary>
    [field: SerializeField]
    public PlaceableType CurrentlyPlacing { get; private set; }

    /// <summary>
    /// Available road pieces. Should be ordered by RoadType!
    /// </summary>
    [InfoBox("Test")]
    [field: SerializeField]
    public List<RoadPreset> Roads { get; private set; }

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

    /// <summary>
    /// Places the currently selected object at the target tile.
    /// </summary>
    /// <param name="targetTile">The tile at which to place the content on.</param>
    private void PlaceObjectAt(Tile targetTile)
    {
        targetTile.PlaceContent(GetCurrentPlaceable(), rotation: _currentRotation);
    }

    /// <summary>
    /// Get the Placeable that should be placed.
    /// </summary>
    /// <returns>The placeable that should be placed.</returns>
    /// <exception cref="KeyNotFoundException">Throws a KeyNotfoundException when an unsupported case is reached.</exception>
    private Placeable GetCurrentPlaceable()
    {
        return CurrentlyPlacing switch
        {
            PlaceableType.BUILDING => new Building(PlaceableBuildings[_currentBuildingIndex]),
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
        _currentRotation = (_currentRotation + 1) % 4;
    }

    /// <summary>
    /// Increments (and rolls-over) the currentBuildingIndex.
    /// </summary>
    private void SelectNextBuilding()
    {
        _currentBuildingIndex = (_currentBuildingIndex + 1) % PlaceableBuildings.Count;
    }


    /// <summary>
    /// Removes the contents at targetTile.
    /// </summary>
    /// <param name="targetTile">The tile at which to remove the contents.</param>
    private void RemoveObjectAt(Tile targetTile)
    {
        targetTile.RemoveContent();
    }
}
