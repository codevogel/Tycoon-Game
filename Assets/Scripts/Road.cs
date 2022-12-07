using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Placeable
{
    public enum RoadType
    {
        CROSS = 0,
        STRAIGHT = 1,
        CORNER = 2,
        END = 3,
        TJUNC = 4
    }
}
