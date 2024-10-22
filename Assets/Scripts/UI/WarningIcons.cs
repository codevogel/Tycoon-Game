using Architect.Placeables;
using Grid;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Handles behaviour for UI warning icons.
    /// </summary>
    public class WarningIcons : MonoBehaviour
    {
        private Tile parentTile;
        public GameObject NoRoads, NoResources, NoCars;

        private void Awake()
        {
            parentTile = transform.parent.parent.parent.GetComponent<Tile>();
            CheckBuilding();
        }

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