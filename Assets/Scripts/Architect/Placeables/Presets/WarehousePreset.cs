using UnityEngine;

[CreateAssetMenu(fileName = "Warehouse", menuName = "ScriptableObjects/Buildings/Warehouse", order = 1)]
public class WarehousePreset : BuildingPreset
{
    public float CollectionRange;
    public int CollectionCapasity;
    
    public void Collect(Building[] buildingsInRange)
    {
        foreach (Building building in buildingsInRange)
        {
            //Send Collector?
        }
    }
}