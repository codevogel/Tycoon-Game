using UnityEngine;

public enum ResourceType
{
    People,
    Food,
    Minerals,
    Ammo
}

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
public class Resource : ScriptableObject
{
    public ResourceType Type;
    public Sprite Sprite;
    public string Description;
}
