using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBuildOnTile : MonoBehaviour
{
    private Tile oldTile;
    
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
        Tile targetTile = GridManager.Instance.HoverTile;

        if (oldTile != null)
        {
            oldTile.AllowContentPlacement.gameObject.SetActive(false);
            oldTile.BlockContentPlacement.gameObject.SetActive(false);
            oldTile = null;
        }

        if (targetTile == null || targetTile == oldTile) return;

        if (targetTile.HasContent)
        {
            targetTile.BlockContentPlacement.gameObject.SetActive(true);
        }
        else
        {
            targetTile.AllowContentPlacement.gameObject.SetActive(true);
        }

        oldTile = targetTile;
    }
}
