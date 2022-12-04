using UnityEngine;

public class Tile
{
    private GameObject tileObject;
    private GameObject building;
    
    private MeshRenderer meshRenderer;
    private Transform buildingHolder;

    /// <summary>
    /// Gets the gameobject for this Tile.
    /// </summary>
    public GameObject TileObject
    {
        get { return tileObject; }
    }

    /// <summary>
    /// Gets or sets the building on this tile.
    /// </summary>
    public GameObject Building
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
    /// Get the building holder (the parent transform of a building).
    /// </summary>
    public Transform BuildingHolder
    {
        get { return buildingHolder; }
    }

    /// <summary>
    /// Constructor for a Tile object
    /// </summary>
    /// <param name="go">The gameobject linked to this Tile.</param>
    public Tile(GameObject go)
    {
        this.tileObject = go;
        meshRenderer = go.GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = new Color32((byte) Random.Range(0, 255), (byte) Random.Range(0, 255), (byte) Random.Range(0, 255), 255);
        buildingHolder = go.transform.Find("Building Offset").Find("Building Holder");
    }
}