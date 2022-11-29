using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(OnHoverToolTip());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    IEnumerator OnHoverToolTip()
    {
        yield return new WaitForSeconds(0.5f);
        tooltip.SetActive(true);
    }
}