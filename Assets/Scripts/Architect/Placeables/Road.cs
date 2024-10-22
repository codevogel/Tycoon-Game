using UnityEngine;

namespace Architect.Placeables
{
    /// <summary>
    /// Defines behaviour for Road Placeables.
    /// Extends from Placeable.
    /// </summary>
    public class Road : Placeable
    {
        public override void OnDestroy()
        {
            Debug.Log("Destroyed road");
        }

        /// <summary>
        /// The types of roads that exist.
        /// </summary>
        public enum RoadType
        {
            CROSS = 0,
            STRAIGHT = 1,
            CORNER = 2,
            END = 3,
            TJUNC = 4
        }
    }
}