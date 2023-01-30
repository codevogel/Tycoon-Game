using UnityEngine;

namespace Buildings.Resources
{
    /// <summary>
    /// ScriptableObject for ResourceUI.
    /// extends from ScriptableObject.
    /// </summary>
    [CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
    public class ResourceUI : ScriptableObject
    {
        public ResourceType type;
        public Sprite sprite;
        public string description;
    }
}
