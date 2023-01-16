using UnityEngine;

namespace Buildings.Resources
{
    [CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
    public class ResourceUI : ScriptableObject
    {
        public ResourceType type;
        public Sprite sprite;
        public string description;
    }
}
