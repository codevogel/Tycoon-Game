using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using static GridManager;
using UnityEngine;

public class ShowPopUp : MonoBehaviour
{
    #region Popup vars
    public GameObject popup;
    public TMP_Text popuptext;
    private Tile _popupTile;
    #endregion

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ShowPopup();
        }
    }

    // Start is called before the first frame update
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

                currentBuilding.RecipientLines.ShowLines(true);
            }
            else if (_popupTile.Content is Road)
            {

            }
        }
        else
        {
            return;
        }
        //TODO: turn off line renderer when popup is clicked off
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
}
