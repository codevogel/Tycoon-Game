using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingController : MonoBehaviour
{
    private static List<Building> buildings;
    public static int Tick { get; private set; }

    public static UnityEvent Produce;
    public static UnityEvent Transport;

    public static void AddBuilding(Building building)
    {
        buildings.Add(building);
    }


    public void FixedUpdate()
    {
        Tick++;
        Produce.Invoke();
    }

}
