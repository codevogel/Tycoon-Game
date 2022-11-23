using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{

}
[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
public class Resource : ScriptableObject
{
    public ResourceType Type;
    public int Amount;
    public Sprite Sprite;
    public string Description;
}
