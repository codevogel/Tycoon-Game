using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceablePreset : ScriptableObject
{
    /// <summary>The visual for the building</summary>
    public Mesh Mesh { get; set; };
    /// <summary>The resources that are removed when it is placed</summary>
    [TableList] public Resource[] BuildCost;
}
