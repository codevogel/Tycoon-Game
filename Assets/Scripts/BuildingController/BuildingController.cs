using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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


    public void FixedUpdate()
    {
        // Increase tick and invoke events
        Tick++;
        Produce.Invoke();
        Transport.Invoke();
    }
}
