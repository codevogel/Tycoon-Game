using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Placeable
{
    public RoadPreset Preset { get; private set; }

    private RoadType type;
    public RoadType Type { get { return type; } }

    public Road(RoadType type)
    {
        this.type = type;
        Preset = null;
    }

    public enum RoadType
    {
        CROSS,
        CORNER,
        END,
        STRAIGHT,
        TJUNC
    }
}
