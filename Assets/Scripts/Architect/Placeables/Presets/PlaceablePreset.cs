using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeable", menuName = "ScriptableObjects/Placeable", order = 1)]
public class PlaceablePreset : ScriptableObject
{
    /// <summary>The visual for the building</summary>
    [field:SerializeField]
    public GameObject Prefab { get; set; }

    /// <summary>The resources that are removed when it is placed</summary>
    [TableList] public Resource[] BuildCost;
}
