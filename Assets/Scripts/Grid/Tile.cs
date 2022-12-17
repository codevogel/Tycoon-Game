using System;
using System.Collections.Generic;
using NavMesh;
using UnityEngine;
using static GridManager;
using static Road;

public class Tile
{
    [SerializeField] private Tile tile;

    #region fields

    #endregion

    #region Properties

    /// <summary>
    /// Indices of this tile in the grid
    /// </summary>
    public Vector2Int Indices { get; private set; }

    /// <summary>
    /// Root transform of gameobject assosciated to this tile.
    /// </summary>
    public Transform Root { get; private set; }

    /// <summary>
    /// Reference to the Placeable content this tile hosts.
    /// </summary>
    public Placeable Content { get; private set; }

    /// <summary>
    /// Reference to the collider for this tile.
    /// </summary>
    public Collider TileCollider { get; set; }

    /// <summary>
    /// Parent transform of the Placeable object
    /// </summary>
    public Transform PlaceableHolder { get; set; }

    /// <summary>
    /// Determines whether the tile hosts content
    /// </summary>
    public bool HasContent
    {
        get => Content != null;
    }

    /// <summary>
    /// Gets the neighbours for this tile from the gridmanager.
    /// </summary>
    public Neighbour[] Neighbours { get => GridManager.Instance.GetNeighboursFor(this); }

    public Transform allowContentPlacement { get; set; }
    public Transform blockContentPlacement { get; set; }

    #endregion

    /// <summary>
    /// Constructor for a tile.
    /// </summary>
    /// <param name="prefab">The tile object prefab.</param>
    /// <param name="position">The position to instantiate this tile at.</param>
    /// <param name="indices">The indices of this tile in the grid.</param>
    public Tile(GameObject prefab, Vector3 position, Vector2Int indices)
    {
        Root = GameObject.Instantiate(prefab, position, Quaternion.identity).transform;
        Indices = indices;
        PlaceableHolder = Root.Find("Placeable Holder");
        blockContentPlacement = Root.Find("Preview").Find("Red");
        allowContentPlacement = Root.Find("Preview").Find("Green");
        TileCollider = Root.Find("Collider").GetComponent<Collider>();
        TileCollider.GetComponent<TileReference>().SetReference(this);
    }

    #region Methods

    /// <summary>
    /// Updates the model to reflect the Content.
    /// </summary>
    /// <param name="rotation">Rotates by rotation * 90 degrees.</param>
    public void UpdateModel(int rotation)
    {
        if (PlaceableHolder.childCount > 0)
        {
            GameObject.Destroy(PlaceableHolder.GetChild(0).gameObject);
            PlaceableHolder.localEulerAngles = new Vector3(90, 0, 0);
        }

        if (Content != null)
        {
            GameObject.Instantiate(Content.Preset.Prefab, PlaceableHolder.transform.position, Quaternion.identity,
                PlaceableHolder);
            PlaceableHolder.localEulerAngles = new Vector3(90, rotation * 90, 0);
        }

        RuntimeNavBaker.Instance.DelayedBake();
    }

    /// <summary>
    /// Places content as this tile with a rotation.
    /// </summary>
    /// <param name="toBePlaced">Placeable to be placed.</param>
    /// <param name="rotation">Rotates by rotation * 90 degrees.</param>
    public void PlaceContent(Placeable toBePlaced, int rotation)
    {
        Content = toBePlaced;
        Debug.Log(Content);
        
        switch (Content)
        {
            case Road road:
                TileCollider.gameObject.layer = LayerMask.NameToLayer("Road");
                PickRoad();
                PickRoadForNeighbours();

                break;
            case Building building:
                TileCollider.gameObject.layer = LayerMask.NameToLayer("Building");
                UpdateModel(rotation);
                building.InitializeAfterInstantiation(this);
                break;
            default:
                break;
        }
        
        Root.name = Content.Preset.name;
    }

    /// <summary>
    /// Picks the right road piece for this tile determined by looking at it's neighbours.
    /// </summary>
    private void PickRoad()
    {
        byte roadConnectionFlag = GetRoadConnectionFlag();
        //Debug.Log(Convert.ToString(roadConnectionFlag, 2));
        (RoadType type, int rotation) = GetFittingPiece(roadConnectionFlag);
        Content.Preset = ArchitectController.Instance.Roads[(int) type];
        UpdateModel(rotation);
    }

    /// <summary>
    /// Picks the right road piece for the neighbours of this tile.
    /// </summary>
    private void PickRoadForNeighbours()
    {
        foreach (Neighbour neighbour in Neighbours)
        {
            if (neighbour.tile.HasContent && neighbour.tile.Content is Road)
            {
                neighbour.tile.PickRoad();
            }
        }
    }

    /// <summary>
    /// Get the road connection bitflag.
    /// </summary>
    /// <returns>The road connection bitflag.</returns>
    private byte GetRoadConnectionFlag()
    {
        int bitwiseFlag = 0;
        foreach (Neighbour neighbour in Neighbours)
        {
            if (neighbour.tile.HasContent && neighbour.tile.Content is Road)
            {
                bitwiseFlag += (int) neighbour.inDirection;
            }
        }

        return (byte) bitwiseFlag;
    }

    /// <summary>
    /// Get the RoadType and Rotation for this bitflag.
    /// </summary>
    /// <param name="roadConnectionFlag"></param>
    /// <returns>the RoadType and Rotation for this road connection flag.</returns>
    /// <exception cref="NotImplementedException">Throws NotImplemented when unsupported case is reached.</exception>
    private (RoadType road, int rotation) GetFittingPiece(byte roadConnectionFlag) => roadConnectionFlag switch
    {
        0b00000000 => (RoadType.CROSS, 0),
        0b00000001 => (RoadType.END, 0),
        0b00000010 => (RoadType.END, 1),
        0b00000011 => (RoadType.CORNER, 0),
        0b00000100 => (RoadType.END, 2),
        0b00000101 => (RoadType.STRAIGHT, 0),
        0b00000110 => (RoadType.CORNER, 1),
        0b00000111 => (RoadType.TJUNC, 0),
        0b00001000 => (RoadType.END, 3),
        0b00001001 => (RoadType.CORNER, 3),
        0b00001010 => (RoadType.STRAIGHT, 1),
        0b00001011 => (RoadType.TJUNC, 3),
        0b00001100 => (RoadType.CORNER, 2),
        0b00001101 => (RoadType.TJUNC, 2),
        0b00001110 => (RoadType.TJUNC, 1),
        0b00001111 => (RoadType.CROSS, 0),
        _ => throw new NotImplementedException("Invalid road connection flag: " +
                                               Convert.ToString(roadConnectionFlag, 2))
    };

    /// <summary>
    /// Removes the content from this tile and updates the model and neighbours to reflect that.
    /// </summary>
    internal void RemoveContent()
    {
        this.Content = null;
        UpdateModel(0);
        PickRoadForNeighbours();
    }

    #endregion
}
