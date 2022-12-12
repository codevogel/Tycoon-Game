using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile", order = 1)]
public class TilePreset : ScriptableObject
{
    public Sprite Sprite;
    public GameObject Obstacle; //An optional obstacle like a rock or trees. This would need to be removed first
    public Resource[] resources;
}
