using System.Collections.Generic;
using System.Text;
using Architect.Placeables;
using Buildings.Resources;
using Grid;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PopupScript : MonoBehaviour
    {
        #region Popup vars
        public GameObject popup;
        public TMP_Text popuptext;
        private Tile _selectedTile;
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
            popup.SetActive(clickedOnTile != null && clickedOnTile != _selectedTile && clickedOnTile.HasContent);
            // If shown
            if (popup.activeSelf)
            {
                // Clear selection if selection exists, then show popup
                if (_selectedTile != null)
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
            _selectedTile = selection;
            // If popup should show a building
            if (_selectedTile.Content is Building b)
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
            if (_selectedTile != null && _selectedTile.Content is Building b)
            {
                // Turn off the connections renderer
                b.BuildingConnectionsRenderer.ShowLines(false);
            }
            _selectedTile = null;
        }

        // Updates the contents for the popup
        private void UpdateContents()
        {
            StringBuilder output = new StringBuilder();
            if (_selectedTile.Content is Building selectedBuilding)
            {
                if (_selectedTile.Content is ConstructionSite selectedSite)
                {
                    foreach (KeyValuePair<ResourceType, int> kvp in selectedSite.Input.Contents)
                    {
                        output.AppendFormat("Build progress: {0}/{1} {2}\n", kvp.Value, selectedSite.PresetToConstruct.buildCost[0].amount, kvp.Key);
                    }
                }
                else if (selectedBuilding.Output.Contents.Count > 0)
                {
                    foreach (KeyValuePair<ResourceType, int> kvp in selectedBuilding.Output.Contents)
                    {
                        output.AppendFormat("Resource = {0} Amount  = {1}\n", kvp.Key, kvp.Value);
                    }
                }
            }
            if (output.Length == 0)
            {
                output.Append("No contents");
            }
            popuptext.text = output.ToString();
        }

        /// <summary>
        /// Removes the contents at targetTile.
        /// </summary>
        public void RemoveObjectAt()
        {
            _selectedTile.RemoveContent();
            popup.SetActive(false);
            Buildings.BuildingController.Refresh.Invoke();
        }
    }
}
