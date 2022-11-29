using System.Collections;
using UnityEngine;

public enum HideDirection{
    Up,
    Down,
    Left,
    Right
}

public class HideShow : MonoBehaviour
{
    [SerializeField] private GameObject targetToHide;
    public RectTransform targetRect;
    [SerializeReference] private HideDirection _direction;
    private bool isHidden;
    private void Start()
    {
        targetRect = (RectTransform)targetToHide.transform;
        isHidden = false;
    }

    public void HideObject(HideShow direction)
    {
        switch (direction._direction)
        {
            case HideDirection.Up:
                if (!isHidden) HideObjectUpwards();
                return;
            case HideDirection.Down:
                if (!isHidden) HideObjectDownwards();
                return;
            case HideDirection.Left:
                if (!isHidden) HideObjectLeftwards();
                return;
            case HideDirection.Right:
                if (!isHidden) HideObjectRightwards();
                return;
        }
    }

    private void HideObjectUpwards()
    {
        Vector3 targetPosition = targetToHide.transform.position + new Vector3(0, targetRect.rect.height, 0);
        StartCoroutine(LerpPosition(targetPosition, 2f));
    }
    
    private void HideObjectDownwards()
    {
        Vector3 targetPosition = targetToHide.transform.position - new Vector3(0, targetRect.rect.height, 0);
        StartCoroutine(LerpPosition(targetPosition, 2f));
    }
    
    private void HideObjectLeftwards()
    {
        Vector3 targetPosition = targetToHide.transform.position - new Vector3(targetRect.rect.width, 0, 0);
        StartCoroutine(LerpPosition(targetPosition, 2f));
    }
    
    private void HideObjectRightwards()
    {
        Vector3 targetPosition = targetToHide.transform.position + new Vector3(targetRect.rect.width, 0, 0);
        StartCoroutine(LerpPosition(targetPosition, 2f));
    }
    
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            targetToHide.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            isHidden = true;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
