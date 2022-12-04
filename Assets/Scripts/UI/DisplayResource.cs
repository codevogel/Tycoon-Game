using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DisplayResource : MonoBehaviour
{
    [SerializeField] private ResourceUI resourceUI;
    [SerializeField] private Image resourceSprite;
    [SerializeField] private TMP_Text resourceAmount;
    [SerializeField] private TMP_Text resourceDescription;

    private void Start()
    {
        resourceSprite.sprite = resourceUI.Sprite;
        resourceDescription.text = resourceUI.Description;
        UpdateResourceAmount();

        StartCoroutine(UpdateValues());
    }

    public void UpdateResourceAmount()
    {
        resourceAmount.text = ResourceManager.Instance.Resources[(int)resourceUI.Type].Amount.ToString();
    }
    
    /// <summary>
    /// Custom update loop.
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateValues()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            
            UpdateResourceAmount();
        }
    }
}
