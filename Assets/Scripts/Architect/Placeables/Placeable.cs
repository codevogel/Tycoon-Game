
using UnityEngine;

public class Placeable
{
    /// <summary>
    /// Preset for this Placeable.
    /// </summary>
    public PlaceablePreset Preset { get; internal set; }

    /// <summary>
    /// Represents the different types of Placeables available.
    /// </summary>
    public enum PlaceableType
    {
        ROAD,
        BUILDING
    }

    public Placeable(PlaceablePreset preset)
    {
        Preset = preset;
    }
}
