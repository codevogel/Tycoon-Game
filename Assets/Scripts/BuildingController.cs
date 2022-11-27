using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridManager;

public class BuildingController : MonoBehaviour
{

    private GridManager gridManager;

    public List<GameObject> placeableBuildings;

    public int currentBuildingIndex = 0;
    public int currentRotation = 0;


    // Start is called before the first frame update
    void Start()
    {
        gridManager = GridManager.instance;
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



    private void AttemptToPlaceBuilding()
    {
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



    private void IncrementBuildingPlacementRotation()
    {
        currentRotation = (currentRotation + 90) % 360;
    }

    private void SelectNextBuilding()
    {
        currentBuildingIndex = (currentBuildingIndex + 1) % placeableBuildings.Count;
    }

    private void PlaceBuildingAt(Tile targetTile)
    {
        Transform newBuilding = Instantiate(placeableBuildings[currentBuildingIndex], targetTile.BuildingHolder).transform;
        newBuilding.transform.localEulerAngles = new Vector3(0, currentRotation, 0);
        targetTile.Building = newBuilding.gameObject;
    }

    private void RemoveBuildingAt(Tile targetTile)
    {
        Destroy(targetTile.BuildingHolder.GetChild(0).gameObject);
        targetTile.Building = null;
    }
}
