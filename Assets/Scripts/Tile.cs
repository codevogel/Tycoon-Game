using System;
using System.Collections.Generic;
using UnityEngine;
using static GridManager;
using static Road;

public class Tile
{
    #region fields
    #endregion

    #region Properties
    public Vector2Int Indices { get; private set; }

    public Transform Root { get;  private set; }
    
    public Placeable Content { get; private set; }

    private Transform PlaceableHolder { get; set; }
    
    public bool HasContent { get => Content != null; }

    public Neighbour[] Neighbours { get => GridManager.Instance.GetNeighboursFor(this); }
    #endregion

    public Tile(GameObject prefab, Vector3 position, Vector2Int indices)
    {
        Root = GameObject.Instantiate(prefab, position, Quaternion.identity).transform;
        Indices = indices;
        PlaceableHolder = Root.Find("Placeable Holder");
    }

    #region Methods 
    public void UpdateModel(int rotation)
    {
        if (PlaceableHolder.childCount > 0)
        {
            GameObject.Destroy(PlaceableHolder.GetChild(0).gameObject);
            PlaceableHolder.localEulerAngles = new Vector3(90, 0, 0);
        }
        GameObject.Instantiate(Content.Preset.Prefab, PlaceableHolder.transform.position, Quaternion.identity, PlaceableHolder);
        Debug.Log(rotation);
        PlaceableHolder.localEulerAngles = new Vector3(90, rotation * 90, 0);
    }

    public void PlaceContent(Placeable toBePlaced, int rotation)
    {
        Content = toBePlaced;
        switch (Content)
        {
            case Road road:
                PickRoad();
                PickRoadForNeighbours();
                break;
            case Building building:
                UpdateModel(rotation);
                break;
            default:
                break;
        }
    }
    private void PickRoad()
    {
        byte  roadConnectionFlag = GetRoadConnectionFlag(Neighbours);
        //Debug.Log(Convert.ToString(roadConnectionFlag, 2));
        (RoadType type, int rotation) = GetFittingPiece(roadConnectionFlag);
        Content.Preset = ArchitectController.Instance.Roads[(int) type];
        UpdateModel(rotation);
    }

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

    private byte GetRoadConnectionFlag(Neighbour[] neighbours)
    {
        int bitwiseFlag = 0;
        foreach (Neighbour neighbour in neighbours)
        {
            if (neighbour.tile.HasContent && neighbour.tile.Content is Road)
            {
                bitwiseFlag += (int)neighbour.inDirection;
            }
        }
        return (byte) bitwiseFlag;
    }

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
        _ => throw new NotImplementedException("Invalid road connection flag: " + Convert.ToString(roadConnectionFlag, 2))
    };
    #endregion

}
