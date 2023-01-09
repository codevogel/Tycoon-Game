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
    public static UnityEvent Refresh = new();

    private static List<Building> _buildings = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Refresh.Invoke();
        }
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
        Refresh.AddListener(building.RefreshRecipients);
        if (building.produces.Length > 0)
        {
            Produce.AddListener(building.Produce);
            Transport.AddListener(building.Transport);
        }
    }

    internal static void UnsubscribeBuilding(Building building)
    {
        _buildings.Remove(building);
        Refresh.RemoveListener(building.RefreshRecipients);
        if (building.produces.Length > 0)
        {
            Produce.RemoveListener(building.Produce);
            Transport.RemoveListener(building.Transport);
        }
    }
}
