using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceManager : SingletonBehaviour<ResourceManager>
{
    public ResourceUI[] uiObjects;
    [TableList] public Resource[] resources;

    [Button]
    public void ResetResources()
    {
        uiObjects = FindAssetsByType<ResourceUI>().ToArray();
        resources = new Resource[uiObjects.Length];
        for (int i = 0; i < uiObjects.Length; i++)
        {
            resources[i] = new Resource(uiObjects[i].Type);
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