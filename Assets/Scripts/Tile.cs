using UnityEngine;

public class Tile
{
    public GameObject go;
    private MeshRenderer meshRenderer;

    public Tile(GameObject go)
    {
        this.go = go;
        meshRenderer = go.GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = new Color32((byte) Random.Range(0, 255), (byte) Random.Range(0, 255), (byte) Random.Range(0, 255), 255);
    }
}