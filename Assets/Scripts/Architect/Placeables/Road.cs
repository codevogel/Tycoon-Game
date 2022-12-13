using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Placeable
{
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

    public Road(RoadPreset preset) : base(preset)
    {

    }
}
