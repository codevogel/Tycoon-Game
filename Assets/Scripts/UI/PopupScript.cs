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
        Tile clickedOnTile = GridManager.Instance.GetTileAt(GridManager.Instance.HoverTile.GridPosition);
        // Determine whether popup should be shown
        popup.SetActive(clickedOnTile != null && clickedOnTile != selectedTile && clickedOnTile.HasContent);
        // If shown
        if (popup.activeSelf)
        {
            // Clear selection if selection exists, then show popup
            if (selectedTile != null)
            {
                ClearSelection();
            }
            ShowPopup(clickedOnTile);
            return;
        }
        // Clear the selection
        ClearSelection();
    }

    /// <summary>
    /// Show the popup for a tile
    /// </summary>
    /// <param name="selection">The tile to show the popup for</param>
    private void ShowPopup(Tile selection)
    {
        selectedTile = selection;
        // If popup should show a building
        if (selectedTile.Content is Building b)
        {
            // Turn on connection renderer
            b.BuildingConnectionsRenderer.ShowLines(true);
        }
    }

    /// <summary>
    /// Hides the popup for a tile
    /// </summary>
    private void ClearSelection()
    {
        // If popup was showing a building
        if (selectedTile != null && selectedTile.Content is Building b)
        {
            // Turn off the connections renderer
            b.BuildingConnectionsRenderer.ShowLines(false);
        }
        selectedTile = null;
    }

    // Updates the contents for the popup
    private void UpdateContents()
    {
        StringBuilder output = new StringBuilder();
        if (selectedTile.Content is Building selectedBuilding)
        {
            if (selectedBuilding.output.Contents.Count > 0)
            {
                foreach (KeyValuePair<ResourceType, int> kvp in selectedBuilding.output.Contents)
                {
                    output.AppendFormat("Resource = {0} Amount  = {1}\n", kvp.Key, kvp.Value);
                }
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
        popup.SetActive(false);
        BuildingController.Refresh.Invoke();
    }
}
