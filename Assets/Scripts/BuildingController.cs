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
            AttemptToPlaceBuilding();
        }
        if (Input.GetMouseButtonDown(1))
        {
            AttemptToRemoveBuilding();
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
    private void AttemptToPlaceBuilding()
    {
        // Get hovered tile coords
        TileCoordinates coords = gridManager.GetTileCoordsFromMousePos();
        if (coords.inBounds)
        {
            Tile targetTile = gridManager.GetTileAt(coords.indices);
            if (targetTile.Building == null)
            {
                PlaceBuildingAt(targetTile);
            }
            else
            {
                Debug.Log("Selected building: " + targetTile.Building.name);
            }
        }
    }

    /// <summary>
    /// Attempts to remove a building at the tile under the mouse.
    /// </summary>
    private void AttemptToRemoveBuilding()
    {
        TileCoordinates coords = gridManager.GetTileCoordsFromMousePos();
        if (coords.inBounds)
        {
            Tile targetTile = gridManager.GetTileAt(coords.indices);
            if (targetTile.Building == null)
            {
                Debug.Log("Tried removing a building that did not exist");
            }
            else
            {
                RemoveBuildingAt(targetTile);
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
    private void PlaceBuildingAt(Tile targetTile)
    {
        Transform newBuilding = Instantiate(placeableBuildings[currentBuildingIndex], targetTile.BuildingHolder).transform;
        newBuilding.transform.localEulerAngles = new Vector3(0, currentRotation, 0);
        targetTile.Building = newBuilding.gameObject;
    }

    /// <summary>
    /// Removes the building at targetTile.
    /// </summary>
    /// <param name="targetTile">The tile to remove the building on.</param>
    private void RemoveBuildingAt(Tile targetTile)
    {
        Destroy(targetTile.BuildingHolder.GetChild(0).gameObject);
        targetTile.Building = null;
    }
}
