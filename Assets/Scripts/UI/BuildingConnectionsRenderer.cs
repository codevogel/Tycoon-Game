using UnityEngine;

namespace UI
{
    public class BuildingConnectionsRenderer : MonoBehaviour
    {

        [SerializeField]
        private Material recipientMaterial;
        [SerializeField]
        private Material providerMaterial;
        private LineRenderer _providerRenderer;
        private LineRenderer _recipientRenderer;

        private void Start()
        {
            GameObject recipientGo = new GameObject();
            GameObject providerGo = new GameObject();
            recipientGo.name = "Recipient Renderer";
            providerGo.name = "Provider Renderer";
            recipientGo.transform.parent = transform;
            providerGo.transform.parent = transform;

            _recipientRenderer = recipientGo.AddComponent<LineRenderer>();
            _providerRenderer = providerGo.AddComponent<LineRenderer>();
            _recipientRenderer.enabled = false;
            _providerRenderer.enabled = false;
            _recipientRenderer.material = recipientMaterial;
            _providerRenderer.material = providerMaterial;
        }

        public void SetRecipients(Building[] recipients)
        {
            SetTargets(_recipientRenderer, recipients);
        }

        public void SetProviders(Building[] providers)
        {
            SetTargets(_providerRenderer, providers);
        }

        private void SetTargets(LineRenderer currentRenderer, Building[] targets)
        {
            currentRenderer.gameObject.SetActive(true);
            currentRenderer.positionCount = targets.Length * 2;

            Debug.Log(currentRenderer.positionCount);
            int targetIndex = 0;
            for (int numPos = 0; numPos < currentRenderer.positionCount; numPos+=2)
            {
                currentRenderer.SetPosition(numPos, transform.position);
                currentRenderer.SetPosition(numPos + 1, targets[targetIndex++].Tile.transform.position);
            }
        }

        public void ShowLines(bool show)
        {
            _recipientRenderer.enabled = show;
            _providerRenderer.enabled = show;
        }
    }
}
