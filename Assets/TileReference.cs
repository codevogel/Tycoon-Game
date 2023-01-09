using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileReference : MonoBehaviour
{
    public Tile Tile { get; private set; }
    public void SetReference(Tile tile)
    {
        Tile = tile;
    }
}
