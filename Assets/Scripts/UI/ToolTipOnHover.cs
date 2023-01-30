using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ToolTipOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject tooltip;

        /// <summary>
        /// When mouse enters a start coroutine before showing the tooltip
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip.SetActive(true);
        }

        /// <summary>
        /// When mouse exits a UI element stop showing a tooltip
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.SetActive(false);
        }
    }
}