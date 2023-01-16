using Buildings.Resources;
using UnityEngine;

namespace Architect.Placeables.Presets
{
    [CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Buildings/Base", order = 1)]
    public class BuildingPreset : PlaceablePreset
    {
        public Resource[] initialStorage;
        public Resource[] produces;
        public Resource[] productionCost;
        public int range;
        public int productionTime;
        public int transportTime;
    }
}
