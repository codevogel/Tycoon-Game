using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;

    /// <summary>
    /// When mouse enters a start coroutine before showing the tooltip
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(OnHoverToolTip());
    }

    /// <summary>
    /// When mouse exits a UI element stop showing a tooltip
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    /// <summary>
    /// Coroutine waits half a second before showing the tooltip
    /// </summary>
    /// <returns></returns>
    IEnumerator OnHoverToolTip()
    {
        yield return new WaitForSeconds(0.5f);
        tooltip.SetActive(true);
    }
}