using Buildings.Resources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Architect.Placeables.Presets
{
    /// <summary>
    /// Scriptable preset for placeables
    /// </summary>
    [CreateAssetMenu(fileName = "Placeable", menuName = "ScriptableObjects/Placeable", order = 1)]
    public class PlaceablePreset : ScriptableObject
    {
        /// <summary>The visual for the building</summary>
        [field:SerializeField]
        public GameObject Prefab { get; set; }

        /// <summary>The resources that are removed when it is placed</summary>
        [TableList] public Resource[] buildCost;
    }
}
