using System;
using System.Collections.Generic;
using System.Text;
using Agency;
using Architect.Placeables;
using Buildings;
using Buildings.Resources;
using Grid;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(EntranceExitPlacer))]
    public class PopupScript : MonoBehaviour
    {
        #region Popup vars
        public GameObject popup;
        public TMP_Text popuptext;
        private Tile _selectedTile;
        private DeliveryAgent _selectedAgent;
        private EntranceExitPlacer _entranceExitPlacer;

        [SerializeField] private Button _entranceExitButton;
        [SerializeField] private GameObject _transportRange;
        [SerializeField] private Slider _transportRangeSlider;

        private bool SomethingSelected { get => _selectedTile != null || _selectedAgent != null; }
        #endregion

        private void Awake()
        {
            _entranceExitPlacer = GetComponent<EntranceExitPlacer>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                // Attempt raycast on agent Clickbox
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, LayerMask.GetMask("Clickbox")))
                {
                    ShowOrHidePopup(hit.collider.gameObject.GetComponentInParent<DeliveryAgent>());
                }
                else
                {
                    // Show popup for hover tile
                    ShowOrHidePopup(GridManager.Instance.GetTileAt(GridManager.Instance.HoverTile.GridPosition));
                }
            }
            if (popup.activeSelf)
            {
                UpdateContents();
            }
        }

        /// <summary>
        /// Shows the popup for newSelection.
        /// </summary>
        /// <typeparam name="T">The type of the object to show the popup for.</typeparam>
        /// <param name="newSelection">The object to show the popup for.</param>
        private void ShowOrHidePopup<T>(T newSelection)
        {
            // If already selected something, clear that selection
            if (SomethingSelected)
            {
                ClearSelection();
            }

            // Now switch on the new selection
            switch (newSelection)
            {
                case Tile t:
                    // ... if clicked out of grid or tile has no content
                    if (t == null || !t.HasContent)
                    {
                        // Keep selection null
                        break;
                    }
                    else
                    {
                        // Select the tile
                        _selectedTile = t;
                        if (_selectedTile.Content is Building b && _selectedTile.Content is not ConstructionSite)
                        {
                            _entranceExitButton.gameObject.SetActive(true);
                            if (b.IsTransporting)
                            {
                                _transportRange.SetActive(true);
                                _transportRangeSlider.value = b.Range;
                                OnTransportRangeSliderChange();
                            }
                        }
                        t.OnSelect();
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
        /// Clears the current selection.
        /// </summary>
        private void ClearSelection()
        {
            // If popup was showing a building
            if (_selectedTile != null)
                _selectedTile.OnDeselect();
            _selectedTile = null;

            if (_selectedAgent != null)
                _selectedAgent.OnDeselect();
            _selectedAgent = null;

            _entranceExitPlacer.enabled = false;
            _entranceExitButton.gameObject.SetActive(false);
            _transportRange.SetActive(false);
        }

        /// <summary>
        /// Updates popup contents according to current selection
        /// </summary>
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

        public void StartPlacingEntranceAndExit()
        {
            _entranceExitPlacer.enabled = true;
            _entranceExitPlacer.SelectBuilding((Building) _selectedTile.Content);
        }


        /// <summary>
        /// Append popup info for an agent
        /// </summary>
        /// <param name="output">The StringBuilder to append to.</param>
        private void AppendAgentContents(StringBuilder output)
        {
            if (_selectedAgent.TargetList.Count > 0)
            {
                output.AppendFormat("Origin: {0}\nTarget: {1}", 
                    _selectedAgent.spawnOrigin.transform.parent.parent.name, _selectedAgent.TargetList[0].Tile.name);
            }
        }

        /// <summary>
        /// Append popup info for a tile
        /// </summary>
        /// <param name="output">The StringBuilder to append to.</param>
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


        /// <summary>
        /// Append resources for a building
        /// </summary>
        /// <param name="output">The StringBuilder to append to.</param>
        /// <param name="selectedBuilding">The building from which the resources are appended.</param>
        private static void AppendBuildingResources(StringBuilder output, Building selectedBuilding)
        {
            foreach (KeyValuePair<ResourceType, int> kvp in selectedBuilding.Output.Contents)
            {
                output.AppendFormat("Resource = {0} Amount  = {1}\n", kvp.Key, kvp.Value);
            }
            foreach (KeyValuePair<ResourceType, int> kvp in selectedBuilding.Input.Contents)
            {
                output.AppendFormat("Resource = {0} Amount  = {1}\n", kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Append building progress for a construction site
        /// </summary>
        /// <param name="output">The StringBuilder to append to.</param>
        /// <param name="selectedBuilding">The site from which the progress is appended.</param>
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
            if (_selectedTile != null) _selectedTile.RemoveContent();
            if (_selectedAgent != null)
            {
                _selectedAgent.spawnOrigin.ReleaseAgent(_selectedAgent);
            }
            popup.SetActive(false);
            BuildingController.Instance.Refresh.Invoke();
            ClearSelection();
        }

        public void OnTransportRangeSliderChange()
        {
            _selectedTile.TransportRangeVisual.transform.localScale = Vector3.one + Vector3.one * _transportRangeSlider.value * 2;
            Building b = _selectedTile.Content as Building;
            b.Range = _transportRangeSlider.value;
            b.RefreshRecipients();
        }
    }
}
