
using UnityEngine;

public class Placeable
{
    public virtual PlaceablePreset Preset { get; internal set; }

    public enum PlaceableType
    {
        ROAD,
        BUILDING
    }
}
