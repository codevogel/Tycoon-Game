using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceManager : SingletonBehaviour<ResourceManager>
{
    [TableList] public ResourceStorage[] resources;

    [Button]
    public void ResetResources()
    {
        Resource[] resource = FindAssetsByType<Resource>().ToArray();
        resources = new ResourceStorage[resource.Length];
        for (int i = 0; i < resource.Length; i++)
        {
            resources[i] = new ResourceStorage(resource[i]);
        }
    }

    [Serializable]
    public class ResourceStorage
    {
        public Resource Resource;
        public int Amount;
        public ResourceStorage(Resource resource, int amount = 0)
        {
            Resource = resource;
            Amount = amount;
        }
    }

    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }
}