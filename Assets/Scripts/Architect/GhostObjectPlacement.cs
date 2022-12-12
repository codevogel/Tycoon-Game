using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostObjectPlacement : MonoBehaviour
{
    private Placeable currentPlaceable;

    // Update is called once per frame
    void Update()
    {
        
    }
/*
    void DisplayObject()
    {
        GridManager.TileCoordinates coords = GridManager.Instance.GetTileCoordsFromMousePos();
        Tile targetTile = GridManager.Instance.GetTileAt(coords.indices);
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            targetTile.gameObject.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.5f);
            transform.position = hit.point;
        }
    }
    */
}
