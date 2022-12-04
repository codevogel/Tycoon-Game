using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile", order = 1)]
public class FloorTilePreset : ScriptableObject
{
    public Sprite Sprite;
    public bool CanBeBuildOn = true;
    public Resource[] resources;
}
