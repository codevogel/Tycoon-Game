using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private Vector2Int indices;
    public Vector2Int Indices { get { return indices; } }

    private Transform root;
    private Transform modelHolder;
    private MeshFilter modelMeshFilter;
    private Placeable content;

    public Transform Root { get { return root; } }
    public Placeable Content { get { return content; } }

    public Transform ModelHolder {  get { return modelHolder; } }

    /// <summary>
    /// Constructor for a Tile object
    /// </summary>
    /// <param name="go">The gameobject linked to this Tile.</param>
    public Tile(Transform root, Vector2Int indicesOfThisTile)
    {
        this.root = root;
        this.indices = indicesOfThisTile;
        modelHolder = root.Find("Model");
        modelMeshFilter = modelHolder.GetChild(0).GetComponent<MeshFilter>();
    }

    public void Build(BuildRequest request)
    {
        switch (request.Placeable)
        {
            case Building building:
                UpdateModel(building.Preset.Mesh);
                RotateModel(request.Rotation);
                break;
            case Road road:
                UpdateModel(road.Preset.Mesh);
                RotateModel(request.Rotation);
                break;
        }
    }

    private void RotateModel(int rotation)
    {
        ModelHolder.localEulerAngles = new Vector3(0, 0, rotation * 90);
    }

    private void UpdateModel(Mesh newMesh) 
    {
        modelMeshFilter.mesh = newMesh;
    }
}
