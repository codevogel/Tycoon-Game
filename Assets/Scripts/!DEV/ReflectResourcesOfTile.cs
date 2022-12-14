using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileReference))]
public class ReflectResourcesOfTile : MonoBehaviour
{

    Tile tile;

    public string ammoIn, ammoOut;
    public string ironIn, ironOut;

    private void Start()
    {
        tile = GetComponent<TileReference>().Tile;
    }


    private void Update()
    {
        if (tile.HasContent && tile.Content is Building)
        {
            Building building = tile.Content as Building;
            Dictionary<ResourceType, int> storageIn = building.input.Contents;
            Dictionary<ResourceType, int> storageOut = building.output.Contents;

            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                if (type == ResourceType.Iron)
                {
                    ironIn = (storageIn.ContainsKey(type) ? storageIn[type] : 0).ToString();
                    ironOut = (storageOut.ContainsKey(type) ? storageOut[type] : 0).ToString();
                }
                else if (type == ResourceType.Ammo)
                {
                    ammoIn = (storageIn.ContainsKey(type) ? storageIn[type] : 0).ToString();
                    ammoOut = (storageOut.ContainsKey(type) ? storageOut[type] : 0).ToString();
                }
            }
        }
    }


}
