using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBuildOnTile : MonoBehaviour
{
    private GridManager.TileCoordinates oldCords;
    private Tile oldTargetTile { get; set; }
    
    private void Update()
    {
        DisplayBuildableTile();
    }

    /// <summary>
    /// This method places a red blocker on a tile that is already populated with an building or road.
    /// When you move off of a blocked tile the red blocker will be removed
    /// </summary>
    void DisplayBuildableTile()
    {
        GridManager.TileCoordinates coords = GridManager.Instance.GetTileCoordsFromMousePos();
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
}
