using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private Vector2Int indices;

    private GameObject tileObject;
    private GameObject building;

    private MeshRenderer meshRenderer;
    private Transform buildingHolder;

    public Vector2Int Indices { get { return indices; } }

    /// <summary>
    /// Gets the gameobject for this Tile.
    /// </summary>
    public GameObject TileObject
    {
        get { return tileObject; }
    }

    /// <summary>
    /// Gets or sets the built object on this tile.
    /// </summary>
    public GameObject Object
    {
        get
        {
            return building;
        }
        set
        {
            building = value;
        }
    }

    /// <summary>
    /// Get the object holder (the parent transform of the object placed on this tile).
    /// </summary>
    public Transform ObjectHolder
    {
        get { return buildingHolder; }
    }

    /// <summary>
    /// Constructor for a Tile object
    /// </summary>
    /// <param name="go">The gameobject linked to this Tile.</param>
    public Tile(GameObject go, Vector2Int indices)
    {
        this.tileObject = go;
        this.indices = indices;
        meshRenderer = go.GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        buildingHolder = go.transform.Find("Building Offset").Find("Building Holder");
    }
}
