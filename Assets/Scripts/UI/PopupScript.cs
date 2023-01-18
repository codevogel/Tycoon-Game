using System;
using System.Collections.Generic;
using System.Text;
using Agency;
using Architect.Placeables;
using Buildings.Resources;
using Grid;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class PopupScript : MonoBehaviour
    {
        #region Popup vars
        public GameObject popup;
        public TMP_Text popuptext;
        private Tile _selectedTile;
        private DeliveryAgent _selectedAgent;

        private bool SomethingSelected { get => _selectedTile != null || _selectedAgent != null; }
        #endregion

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, LayerMask.GetMask("Clickbox")))
                {
                    ShowOrHidePopup(hit.collider.gameObject.GetComponentInParent<DeliveryAgent>());
                }
                else
                {
                    ShowOrHidePopup(GridManager.Instance.GetTileAt(GridManager.Instance.HoverTile.GridPosition));
                }
            }
            if (popup.activeSelf)
            {
                UpdateContents();
            }
        }


        private void ShowOrHidePopup<T>(T newSelection)
        {
            if (SomethingSelected)
            {
                ClearSelection();
            }
            switch (newSelection)
            {
                case Tile t:
                    // If clicked out of grid or tile has no content
                    if (t == null || !t.HasContent)
                    {
                        // Nullify
                        _selectedTile = null;
                    }
                    else
                    {
                        _selectedTile = t;
                    }
                    break;
                case DeliveryAgent a:
                    _selectedAgent = a;
                    a.OnSelect();
                    break;
                default:
                    break;
            }
            popup.SetActive(SomethingSelected);
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

            if (_selectedAgent != null)
                _selectedAgent.OnDeselect();
            _selectedAgent = null;
        }

        // Updates the contents for the popup
        private void UpdateContents()
        {
            StringBuilder output = new StringBuilder();

            // If tile is selected
            if (_selectedTile != null)
            {
                AppendTileContents(output);
            }
            else // Agent is selected
            {
                AppendAgentContents(output);
            }

            if (output.Length == 0)
            {
                output.Append("No contents");
            }
            popuptext.text = output.ToString();
        }

        private void AppendAgentContents(StringBuilder output)
        {
            if (_selectedAgent.TargetList.Count > 0)
            {
                output.AppendFormat("Origin: {0}\nTarget: {1}", _selectedAgent.spawnOrigin, _selectedAgent.TargetList[0]);
            }
        }

        private void AppendTileContents(StringBuilder output)
        {
            if (_selectedTile.HasContent)
            {
                if (_selectedTile.Content is Building b)
                {
                    if (b is ConstructionSite c)
                    {
                        AppendBuildingProgress(output, c);
                        return;
                    }
                    AppendBuildingResources(output, b);
                    return;
                }
                else
                {
                    output.Append("Road");
                }
            }
        }

        private static void AppendBuildingResources(StringBuilder output, Building selectedBuilding)
        {
            foreach (KeyValuePair<ResourceType, int> kvp in selectedBuilding.Output.Contents)
            {
                output.AppendFormat("Resource = {0} Amount  = {1}\n", kvp.Key, kvp.Value);
            }
        }

        private static void AppendBuildingProgress(StringBuilder output, ConstructionSite selectedSite)
        {
            var buildCost = selectedSite.PresetToConstruct.buildCost[0];
            foreach (KeyValuePair<ResourceType, int> kvp in selectedSite.Input.Contents)
            {

                output.AppendFormat("Build progress: {0}/{1} {2}\n", kvp.Value, buildCost.amount, buildCost.type.ToString());
            }
            if (output.Length == 0)
            {
                output.AppendFormat("Build Progress: 0/{0} {1}", buildCost.amount, buildCost.type.ToString());
            }
        }

        /// <summary>
        /// Removes the contents at targetTile.
        /// </summary>
        public void RemoveObjectAt()
        {
            if (_selectedTile != null)
            {
                _selectedTile.RemoveContent();
            }
            if (_selectedAgent != null)
            {
                _selectedAgent.spawnOrigin.ReleaseAgent(_selectedAgent);
            }
            popup.SetActive(false);
            Buildings.BuildingController.Refresh.Invoke();
        }
    }
}
