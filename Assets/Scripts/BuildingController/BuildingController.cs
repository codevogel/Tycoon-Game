using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingController : MonoBehaviour
{
    public static int Tick { get; private set; }

    public static UnityEvent Produce;
    public static UnityEvent Transport;
    public void FixedUpdate()
    {
        Tick++;
        Produce.Invoke();
    }
}
