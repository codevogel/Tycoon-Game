using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Building;

public class BuildingController : MonoBehaviour
{
    /// <summary>
    /// Current tick
    /// </summary>
    public static int Tick { get; private set; }
    /// <summary>
    /// Event for production cycle
    /// </summary>
    public static UnityEvent Produce = new();
    /// <summary>
    /// Event for transport cycle
    /// </summary>
    public static UnityEvent Transport = new();

    private static List<Building> _buildings = new();

    private void Update()
    {
    }

    public void FixedUpdate()
    {
        // Increase tick and invoke events
        Tick++;
        Produce.Invoke();
        Transport.Invoke();
    }

    internal static void SubscribeBuilding(Building building)
    {
        _buildings.Add(building);
        switch (building.LocalPreset.buildingType)
        {
            case BuildingType.Factory:
                BuildingController.Produce.AddListener(building.Produce);
                BuildingController.Transport.AddListener(building.Transport);
                break;
            case BuildingType.Storage:
                break;
            case BuildingType.Tower:
                break;
            default:
                break;
        }
    }

    internal static void UnsubscribeBuilding(Building building)
    {
        _buildings.Remove(building);
        switch (building.LocalPreset.buildingType)
        {
            case BuildingType.Factory:
                BuildingController.Produce.RemoveListener(building.Produce);
                BuildingController.Transport.RemoveListener(building.Transport);
                break;
            case BuildingType.Storage:
                break;
            case BuildingType.Tower:
                break;
            default:
                break;
        }
    }
}
