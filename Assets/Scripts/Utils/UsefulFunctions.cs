using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UsefulFunctions
{
#if (UNITY_EDITOR)
    /// <summary>Looks through all the files in assets to find the assets with the same type as is given in T</summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>A list of all the objects it found</returns>
    public static List<T> FindAssetsByType<T>() where T : Object
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
#endif
}
