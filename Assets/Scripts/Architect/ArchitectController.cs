using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
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

    private TileCoordinates oldCords;
    private Tile oldTargetTile { get; set; }
    private bool shouldUndo { get; set; }

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

    private BuildingPreset CurrentBuildingPreset => PlaceableBuildings[_currentBuildingIndex];

    #region Popup vars
    public GameObject popup;
    public TMP_Text popuptext;
    private Tile _popupTile;
    #endregion
    // Update is called once per frame
    void Update()
    {
        DisplayBuildableTile();

        if (Input.GetMouseButton(0))
        {
            AttemptToPlaceObject();

        }

        if (Input.GetMouseButtonDown(1))
        {
            ShowPopup();
            //Move to button on popup
            //AttemptToRemoveObject();
        }

        //alpha key 1 and 2 do not work?
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
                BuildingController.Refresh.Invoke(); // TODO: not performance friendly
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
        targetTile.allowContentPlacement.gameObject.SetActive(false);
        targetTile.blockContentPlacement.gameObject.SetActive(true);
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
            PlaceableType.BUILDING => new Building(CurrentBuildingPreset),
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
                BuildingController.Refresh.Invoke(); // TODO: not performance friendly
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
    /// <param name="popupTile">The tile at which to remove the contents.</param>
    public void RemoveObjectAt()
    {
        _popupTile.RemoveContent();
        _popupTile.blockContentPlacement.gameObject.SetActive(false);
        popup.SetActive(false);
    }

    /// <summary>
    /// This method places a red blocker on a tile that is already populated with an building or road.
    /// When you move off of a blocked tile the red blocker will be removed
    /// </summary>
    void DisplayBuildableTile()
    {
        TileCoordinates coords = GridManager.Instance.GetTileCoordsFromMousePos();
        Tile targetTile = GridManager.Instance.GetTileAt(coords.indices);

        if (targetTile == null) return;
        if (coords.indices == oldCords.indices) return;

        oldTargetTile = GridManager.Instance.GetTileAt(oldCords.indices);
        oldTargetTile.allowContentPlacement.gameObject.SetActive(false);

        if (oldTargetTile.PlaceableHolder.childCount > 0)
        {
            oldTargetTile.blockContentPlacement.gameObject.SetActive(false);
            //hiddenContent = oldTargetTile.PlaceableHolder.GetChild(0).gameObject;
            //hiddenContent.SetActive(true);
        }

        if (targetTile.HasContent)
        {
            //hiddenContent = targetTile.PlaceableHolder.GetChild(0).gameObject;
            //hiddenContent.SetActive(false);
            targetTile.blockContentPlacement.gameObject.SetActive(true);
        }
        else
        {
            targetTile.blockContentPlacement.gameObject.SetActive(false);
            targetTile.allowContentPlacement.gameObject.SetActive(true);
        }

        oldCords.indices = coords.indices;
    }

    /// <summary>
    /// doet popup laten zien on right click. 
    /// popup geeft informatie over de building/road en buttons om te upgraden/destroyen.
    /// </summary>
    void ShowPopup()
    {
        TileCoordinates coords = GridManager.Instance.GetTileCoordsFromMousePos();
        _popupTile = GridManager.Instance.GetTileAt(coords.indices);
        if (_popupTile == null) return;
        if (_popupTile.HasContent)
        {
            if (_popupTile.Content is Building)
            {
                string output = "";
                popup.SetActive(true);
                Building currentBuilding = (Building)_popupTile.Content;
                Debug.Log(currentBuilding.output.Contents.Count);
                foreach (KeyValuePair<ResourceType, int> kvp in currentBuilding.output.Contents)
                {
                    output += string.Format("Resource = {0}, Amount  = {1}", kvp.Key, kvp.Value);
                    output += "\n";
                }

                popuptext.text = $"{output}";
                Console.WriteLine(output);
            }
            else if (_popupTile.Content is Road)
            {

            }
        }
        else
        {
            return;
        }
    }

    public void SelectBuildingZero()
    {
        CurrentlyPlacing = PlaceableType.BUILDING;
        _currentBuildingIndex = 0;
    }

    public void SelectBuildingOne()
    {
        CurrentlyPlacing = PlaceableType.BUILDING;
        _currentBuildingIndex = 1;
    }

    public void SelectRoad()
    {
        CurrentlyPlacing = PlaceableType.ROAD;
    }
}
