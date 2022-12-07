using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;

public class RoadNav : MonoBehaviour
{
    public NavMeshSurface[] surfaces;
    public NavMeshModifier[] mods;
    public Transform[] objectsToRotate;


    private void Update()
    {
        surfaces = GetComponentsInChildren<NavMeshSurface>();
    }

    [Button]
    private void BuildNavMeshy()
    {
        foreach (var t in surfaces)
        {
            t.BuildNavMesh();
        }
    }
}

//https://learn.unity.com/tutorial/runtime-navmesh-generation#
//https://docs.unity3d.com/Manual/nav-AreasAndCosts.html