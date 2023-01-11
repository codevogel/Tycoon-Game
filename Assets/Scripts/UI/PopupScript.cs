using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using static GridManager;
using UnityEngine;
using System.Text;

public class PopupScript : MonoBehaviour
{
    #region Popup vars
    public GameObject popup;
    public TMP_Text popuptext;
    private Tile selectedTile;
    #endregion

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ShowOrHidePopup();
        }
        if (popup.activeSelf)
        {
            UpdateContents();
        }
    }

    // Start is called before the first frame update
    /// <summary>
    /// doet popup laten zien on right click. 
    /// popup geeft informatie over de building/road en buttons om te upgraden/destroyen.
    /// </summary>
    void ShowOrHidePopup()
    {
        Tile clickedOnTile = GridManager.Instance.GetTileAt(GridManager.Instance.GetTileCoordsFromMousePos().indices);
        popup.SetActive(clickedOnTile != null && clickedOnTile != selectedTile && clickedOnTile.HasContent);
        if (popup.activeSelf)
        {
            if (selectedTile != null)
            {
                HidePopup();
            }
            ShowPopup(clickedOnTile);
            return;
        }
        HidePopup();
    }

    private void ShowPopup(Tile clickedOnTile)
    {
        selectedTile = clickedOnTile;
        if (selectedTile.Content is Building)
        {
            Building b = (Building)selectedTile.Content;
            b.BuildingConnectionsRenderer.ShowLines(true);
        }
    }

    private void HidePopup()
    {
        if (selectedTile != null && selectedTile.Content is Building)
        {
            Building b = (Building)selectedTile.Content;
            b.BuildingConnectionsRenderer.ShowLines(false);
        }
        selectedTile = null;
    }

    private void UpdateContents()
    {
        StringBuilder output = new StringBuilder();
        Building selectedBuilding = selectedTile.Content as Building;
        if (selectedBuilding.output.Contents.Count > 0)
        {
            foreach (KeyValuePair<ResourceType, int> kvp in selectedBuilding.output.Contents)
            {
                output.AppendFormat("Resource = {0} Amount  = {1}\n", kvp.Key, kvp.Value);
            }
        }
        else
        {
            output.Append("No contents");
        }
        popuptext.text = output.ToString();
    }

    /// <summary>
    /// Removes the contents at targetTile.
    /// </summary>
    /// <param name="popupTile">The tile at which to remove the contents.</param>
    public void RemoveObjectAt()
    {
        selectedTile.RemoveContent();
        selectedTile.blockContentPlacement.gameObject.SetActive(false);
        popup.SetActive(false);
    }
}
