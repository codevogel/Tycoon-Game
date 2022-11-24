using System;

[Serializable]
public class Resource
{
    public ResourceType Type;
    public int Amount;
    public Resource(ResourceType type, int amount = 0)
    {
        Type = type;
        Amount = amount;
    }
}
