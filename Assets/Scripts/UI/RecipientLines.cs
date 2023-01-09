using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RecipientLines : MonoBehaviour
{

    [SerializeField]
    private Material _lineMaterial; 
    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.material = _lineMaterial;
    }

    public void SetRecipients(Building[] recipients)
    {
        _lineRenderer.gameObject.SetActive(true);
        _lineRenderer.positionCount = recipients.Length * 2;
        int recipientIndex = 0;
        for (int numPos = 0; numPos < _lineRenderer.positionCount; numPos+=2)
        {
            _lineRenderer.SetPosition(numPos, transform.position);
            _lineRenderer.SetPosition(numPos + 1, recipients[recipientIndex++].Tile.Root.position);
        }
    }

    public void ShowLines(bool show)
    {
        _lineRenderer.enabled = show;
    }

}
