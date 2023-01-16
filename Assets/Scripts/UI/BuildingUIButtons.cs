using System.Collections.Generic;
using Architect;
using UnityEngine;
using static Architect.Placeables.Placeable;

namespace UI
{
    public class BuildingUIButtons : MonoBehaviour
    {

        [SerializeField]
        public List<GameObject> buttonsForEverythingButMine = new();

        private void Start()
        {
            HideEverythingButMine(true);
        }

        public void HideEverythingButMine(bool hide)
        {
            foreach (GameObject go in buttonsForEverythingButMine)
            {
                go.SetActive(!hide);
            }
        }

        public void SelectMineBuilding()
        {
            ArchitectController.Instance.SetPlaceableType(PlaceableType.BUILDING);
            ArchitectController.Instance.SetBuildingIndex(0);
        }

        public void SelectFactoryBuilding()
        {
            ArchitectController.Instance.SetPlaceableType(PlaceableType.BUILDING);
            ArchitectController.Instance.SetBuildingIndex(1);
        }

        public void SelectRoad()
        {
            ArchitectController.Instance.SetPlaceableType(PlaceableType.ROAD);
        }
    
        public void SelectTower()
        {
            ArchitectController.Instance.SetPlaceableType(PlaceableType.BUILDING);
            ArchitectController.Instance.SetBuildingIndex(2);
        }
    }
}
