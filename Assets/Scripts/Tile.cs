using UnityEngine;

public class Tile
{
    private GameObject tileObject;
    private GameObject building;
    
    private MeshRenderer meshRenderer;
    private Transform buildingHolder;

    public GameObject TileObject
    {
        get { return tileObject; }
    }

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

    public Transform BuildingHolder
    {
        get { return buildingHolder; }
    }

    public Tile(GameObject go)
    {
        this.tileObject = go;
        meshRenderer = go.GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = new Color32((byte) Random.Range(0, 255), (byte) Random.Range(0, 255), (byte) Random.Range(0, 255), 255);
        buildingHolder = go.transform.Find("Building Offset").Find("Building Holder");
    }
}