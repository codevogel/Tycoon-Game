using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    #region fields
    [SerializeField]
    private GameObject _tilePrefab;

    #endregion
    #region Properties
    public Vector2Int Indices { get; private set; }

    public Transform Root { get;  private set; }
    
    public Placeable Content { get; private set; }

    private MeshFilter ModelMeshFilter { get; set; }
    #endregion

    public Tile(Vector3 position, Vector2Int indices)
    {
        Root = GameObject.Instantiate(_tilePrefab, position, Quaternion.identity).transform;
        Indices = indices;
        ModelMeshFilter = Root.Find("Model Offset").Find("Model").GetComponent<MeshFilter>();
    }

    #region Methods 
    public void UpdateModel()
    {
        ModelMeshFilter.mesh = Content.Preset.Mesh;
    }
    #endregion

}
