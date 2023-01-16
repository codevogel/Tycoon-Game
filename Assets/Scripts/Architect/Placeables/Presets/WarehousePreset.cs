using UnityEngine;

namespace Architect.Placeables.Presets
{
    [CreateAssetMenu(fileName = "Warehouse", menuName = "ScriptableObjects/Buildings/Warehouse", order = 1)]
    public class WarehousePreset : BuildingPreset
    {
        public float collectionRange;
        public int collectionCapasity;
    
        public void Collect(Building[] buildingsInRange)
        {
            foreach (Building building in buildingsInRange)
            {
                //Send Collector?
            }
        }
    }
}