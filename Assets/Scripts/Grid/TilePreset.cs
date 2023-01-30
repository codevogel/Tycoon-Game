using Architect.Placeables.Presets;
using Buildings.Resources;
using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile", order = 1)]
    public class TilePreset : ScriptableObject
    {
        public Sprite sprite;
        public PlaceablePreset obstacle; //An optional obstacle like a rock or trees. This would need to be removed first
        public Resource[] resources;
    }
}
