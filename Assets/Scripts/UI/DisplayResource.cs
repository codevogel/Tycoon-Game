using System.Collections;
using Buildings.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DisplayResource : MonoBehaviour
    {
        [SerializeField] private ResourceUI resourceUI;
        [SerializeField] private Image resourceSprite;
        [SerializeField] private TMP_Text resourceAmount;
        [SerializeField] private TMP_Text resourceDescription;

        private void Start()
        {
            resourceSprite.sprite = resourceUI.sprite;
            resourceDescription.text = resourceUI.description;
            UpdateResourceAmount();

            StartCoroutine(UpdateValues());
        }

        public void UpdateResourceAmount()
        {
            //TODO: replace
            //resourceAmount.text = ResourceManager.Instance.Resources[(int)resourceUI.Type].Amount.ToString();
        }
    
        /// <summary>
        /// Custom update loop.
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateValues()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.25f);
            
                UpdateResourceAmount();
            }
        }
    }
}
