using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArchitectController;
using static Placeable;

public class BuildingUIButtons : MonoBehaviour
{
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
}
