using Architect.Placeables.Presets;
using Buildings.Resources;
using UnityEngine;

namespace Grid
{
    /// <summary>
    /// Base class for TilePreset ScriptableObjects.
    /// </summary>
    [CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile", order = 1)]
    public class TilePreset : ScriptableObject
    {
        public Sprite sprite;
        //An optional obstacle like a rock or trees. This would need to be removed first
        public PlaceablePreset obstacle;         
        public Resource[] resources;
    }
}
