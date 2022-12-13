using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Building;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Buildings/Base", order = 1)]
public class BuildingPreset : PlaceablePreset
{
    public BuildingType buildingType;
    public Resource[] InitialStorage;
    public Resource[] Produces;
    public Resource[] ProductionCost;
    public int ProductionTime;
}
