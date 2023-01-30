using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AgentTargetRenderer : MonoBehaviour
{

    [SerializeField]
    private Material _lineMaterial;
    private LineRenderer _lineRenderer;


    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.material = _lineMaterial;
    }

    public void ShowLines(bool show)
    {
        _lineRenderer.enabled = show;
    }

    public void SetOriginAndTarget(Transform origin, Transform target)
    {
        _lineRenderer.positionCount = 3;
        _lineRenderer.SetPositions(new Vector3[]
        {
            origin.position,
            transform.position,
            target.position
        });
    }

}
