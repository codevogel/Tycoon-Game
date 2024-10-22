using System.Collections.Generic;
using Architect.Placeables;
using Architect.Placeables.Presets;
using Grid;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using Utils;
using static Architect.Placeables.Placeable;

namespace Architect
{
    /// <summary>
    /// Singleton behaviour that handles the placement of Placeables.
    /// </summary>
    public class ArchitectController : SingletonBehaviour<ArchitectController>
    {
        /// <summary>
        /// List containing presets of the available buildings.
        /// </summary>
        [field: SerializeField]
        private List<BuildingPreset> PlaceableBuildings { get; set; }

        /// <summary>
        /// Current index of preset of building to place.
        /// </summary>
        private int _currentBuildingIndex;
        private int _currentRotation;

        /// <summary>
        /// Type which the ArchitectController should currently place.
        /// </summary>
        [field: SerializeField]
        public PlaceableType CurrentlyPlacing { get; private set; }

        /// <summary>
        /// Available road pieces. Should be ordered by RoadType!
        /// </summary>
        [field: SerializeField]
        public List<RoadPreset> Roads { get; private set; }

        private BuildingPreset CurrentBuildingPreset => PlaceableBuildings[_currentBuildingIndex];

        [SerializeField]
        private BuildingPreset constructionSitePreset;

        [SerializeField]
        private BuildingUIButtons buildingUIButtons;

        private Tile _firstBuildingTile = null;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                AttemptToPlaceObject();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                IncrementBuildingPlacementRotation();

            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectNextBuilding();
            }
        }


        /// <summary>
        /// Attempts to place a building at the tile under the mouse.
        /// </summary>
        private void AttemptToPlaceObject()
        {
            Tile targetTile = GridManager.Instance.HoverTile;
            if (targetTile != null)
            {
                if (!targetTile.HasContent)
                {
                    if (_firstBuildingTile == null || _firstBuildingTile.Content == null)
                    {
                        PlaceBuildingAt(targetTile, CurrentBuildingPreset);
                        buildingUIButtons.HideEverythingButMine(false);
                        _firstBuildingTile = targetTile;
                    }
                    else
                        PlaceObjectAt(targetTile);
                }
            }
        }

        /// <summary>
        /// Places the currently selected object at the target tile.
        /// </summary>
        /// <param name="targetTile">The tile at which to place the content on.</param>
        private void PlaceObjectAt(Tile targetTile)
        {
            targetTile.PlaceContent(GetCurrentPlaceable(), rotation: _currentRotation);
        }

        /// <summary>
        /// Places a building at the target tile.
        /// </summary>
        /// <param name="presetToConstruct">The building to place.</param>
        internal void PlaceBuildingAt(Tile targetTile, BuildingPreset presetToConstruct)
        {
            targetTile.PlaceContent(new Building(presetToConstruct), rotation: _currentRotation);
        }

        /// <summary>
        /// Get the Placeable that should be placed.
        /// </summary>
        /// <returns>The placeable that should be placed.</returns>
        /// <exception cref="KeyNotFoundException">Throws a KeyNotfoundException when an unsupported case is reached.</exception>
        private Placeable GetCurrentPlaceable()
        {
            return CurrentlyPlacing switch
            {
                PlaceableType.BUILDING => new ConstructionSite(constructionSitePreset, CurrentBuildingPreset),
                PlaceableType.ROAD => new Road(),
                _ => throw new KeyNotFoundException("Did not find PlaceableType" + CurrentlyPlacing)
            };
        }

        /// <summary>
        /// Rotates the current building angle by rotationStep.
        /// </summary>
        private void IncrementBuildingPlacementRotation()
        {
            _currentRotation = (_currentRotation + 1) % 4;
        }

        /// <summary>
        /// Increments (and rolls-over) the currentBuildingIndex.
        /// </summary>
        private void SelectNextBuilding()
        {
            _currentBuildingIndex = (_currentBuildingIndex + 1) % PlaceableBuildings.Count;
        }

        public void SetPlaceableType(PlaceableType placeableType)
        {
            CurrentlyPlacing = placeableType;
        }

        public void SetBuildingIndex(int index)
        {
            _currentBuildingIndex = index;
        }
    }
}
