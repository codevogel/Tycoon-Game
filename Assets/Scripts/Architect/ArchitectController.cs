using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Placeable;
using static Road;

public class ArchitectController : SingletonBehaviour<ArchitectController>
{
    /// <summary>
    /// List containing presets of the available buildings.
    /// </summary>
    [field: SerializeField]
    private List<BuildingPreset> PlaceableBuildings { get; set; }

    private Tile previousTile;

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

    public GameObject hiddenContent;
    public GameObject ghostContent;

    // Update is called once per frame
    void Update()
    {
        DisplayBuildableTile();

        if (Input.GetMouseButtonDown(0))
        {
            AttemptToPlaceObject();

        }

        if (Input.GetMouseButtonDown(1))
        {
            AttemptToRemoveObject();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            IncrementBuildingPlacementRotation();

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
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
        Tile targetTile = GridManager.Instance.HoverTile;
        if (targetTile != null && !targetTile.HasContent)
        {
            PlaceObjectAt(targetTile);
        }
    }

    /// <summary>
    /// Places the currently selected object at the target tile.
    /// </summary>
    /// <param name="targetTile">The tile at which to place the content on.</param>
    private void PlaceObjectAt(Tile targetTile)
    {
        targetTile.PlaceContent(GetCurrentPlaceable(), rotation: _currentRotation);
        targetTile.AllowContentPlacement.gameObject.SetActive(false);
        targetTile.BlockContentPlacement.gameObject.SetActive(true);
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
            PlaceableType.BUILDING => new Building(GetCurrentBuildingPreset()),
            PlaceableType.ROAD => new Road(),
            _ => throw new KeyNotFoundException("Did not find PlaceableType" + CurrentlyPlacing)
        };
    }

    private BuildingPreset GetCurrentBuildingPreset()
    {
        return PlaceableBuildings[_currentBuildingIndex];
    }

    /// <summary>
    /// Attempts to remove a building at the tile under the mouse.
    /// </summary>
    private void AttemptToRemoveObject()
    {
        Tile targetTile = GridManager.Instance.HoverTile;
        if (targetTile != null)
        {
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
        targetTile.BlockContentPlacement.gameObject.SetActive(false);
        targetTile.AllowContentPlacement.gameObject.SetActive(true);
    }

    /// <summary>
    /// This method places a red blocker on a tile that is already populated with an building or road.
    /// When you move off of a blocked tile the red blocker will be removed
    /// </summary>
    void DisplayBuildableTile()
    {
        Tile targetTile = GridManager.Instance.HoverTile;

        if (previousTile != null && targetTile != previousTile)
        {
            previousTile.AllowContentPlacement.gameObject.SetActive(false);

            if (previousTile.PlaceableHolder.childCount > 0)
            {
                previousTile.BlockContentPlacement.gameObject.SetActive(false);
                //hiddenContent = oldTargetTile.PlaceableHolder.GetChild(0).gameObject;
                //hiddenContent.SetActive(true);
            }
        }
        if (targetTile != null)
        {
            if (targetTile.HasContent)
            {
                //hiddenContent = targetTile.PlaceableHolder.GetChild(0).gameObject;
                //hiddenContent.SetActive(false);
                targetTile.BlockContentPlacement.gameObject.SetActive(true);
            }
            else
            {
                targetTile.BlockContentPlacement.gameObject.SetActive(false);
                targetTile.AllowContentPlacement.gameObject.SetActive(true);
            }
        }
        previousTile = targetTile;
    }
}
