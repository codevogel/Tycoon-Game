using Grid;
using UnityEngine;

namespace UI
{
    public class WarningIcons : MonoBehaviour
    {
        public Tile parentTile;
        public GameObject NoRoads, NoResources, NoCars;

        public void CheckBuilding()
        {
            //If the new content is a building add the functions to its events
            if (parentTile.Content is Building b)
            {
                b.OnRoadCheck += OnNoRoads;
                b.OnFabricate += OnNoResources;
                b.OnTransport += OnNoCars;
            }
        }

        public void OnNoRoads(bool b)
        {
            NoRoads.SetActive(!b);
        }

        public void OnNoResources(bool b)
        {
            NoResources.SetActive(!b);
        }

        public void OnNoCars(bool b)
        {
            NoCars.SetActive(!b);
        }
    }
}