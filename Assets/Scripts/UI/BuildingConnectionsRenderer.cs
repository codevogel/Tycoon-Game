using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConnectionsRenderer : MonoBehaviour
{

    [SerializeField]
    private Material _recipientMaterial;
    [SerializeField]
    private Material _providerMaterial;
    private LineRenderer providerRenderer;
    private LineRenderer recipientRenderer;

    private void Start()
    {
        GameObject recipientGO = new GameObject();
        GameObject providerGO = new GameObject();
        recipientGO.name = "Recipient Renderer";
        providerGO.name = "Provider Renderer";
        recipientGO.transform.parent = transform;
        providerGO.transform.parent = transform;

        recipientRenderer = recipientGO.AddComponent<LineRenderer>();
        providerRenderer = providerGO.AddComponent<LineRenderer>();
        recipientRenderer.enabled = false;
        providerRenderer.enabled = false;
        recipientRenderer.material = _recipientMaterial;
        providerRenderer.material = _providerMaterial;
    }

    public void SetRecipients(Building[] recipients)
    {
        SetTargets(recipientRenderer, recipients);
    }

    public void SetProviders(Building[] providers)
    {
        SetTargets(providerRenderer, providers);
    }

    private void SetTargets(LineRenderer currentRenderer, Building[] targets)
    {
        currentRenderer.gameObject.SetActive(true);
        currentRenderer.positionCount = targets.Length * 2;
        int targetIndex = 0;
        for (int numPos = 0; numPos < currentRenderer.positionCount; numPos+=2)
        {
            currentRenderer.SetPosition(numPos, transform.position);
            currentRenderer.SetPosition(numPos + 1, targets[targetIndex++].Tile.Root.position);
        }
    }

    public void ShowLines(bool show)
    {
        recipientRenderer.enabled = show;
        providerRenderer.enabled = show;
    }
}
