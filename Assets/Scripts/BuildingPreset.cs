using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 1)]
public class BuildingPreset : ScriptableObject
{
    public GameObject Model;
    [TableList] public Resource[] BuildCost;
    [TableList] public Resource[] InitialProduction;
    [TableList] public Resource[] Upkeep;
    [TableList] public Resource[] Production;
    public int UpkeepStorageSize;
    public int ProductionStorageSize;
}
