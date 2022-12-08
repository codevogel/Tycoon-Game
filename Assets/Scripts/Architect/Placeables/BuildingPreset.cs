using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<<< HEAD:Assets/Scripts/Grid/Buildings/BuildingPreset.cs
[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Buildings/Base", order = 1)]
public class BuildingPreset : ScriptableObject
========
[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 1)]
public class BuildingPreset : PlaceablePreset
>>>>>>>> Dev-Branch:Assets/Scripts/Architect/Placeables/BuildingPreset.cs
{
    /// <summary>The resources that are made when it is placed</summary>
    [TableList] public Resource[] InitialProduction;
    /// <summary>These resources are removed from its upkeep storage</summary>
    [TableList] public Resource[] Upkeep;
    /// <summary>These resources are added to its production storage</summary>
    [TableList] public Resource[] Production;
    public int UpkeepStorageSize;
    public int ProductionStorageSize;
}
