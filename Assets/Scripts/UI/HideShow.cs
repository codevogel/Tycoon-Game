using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeReference] private HideDirection direction;
    private bool _isHidden;
    public float hideSpeed;
    public bool _isMoving;
    private void Start()
    {
        targetRect = (RectTransform)targetToHide.transform;
        _isHidden = false;
        _isMoving = false;
    }

    public void HideObject(HideShow direction)
    {
        if(_isMoving) return;
        
        switch (direction.direction)
        {
            case HideDirection.Up:
                if (!_isHidden) MoveObjectUpwards(true);
                else MoveObjectDownwards(false);
                return;
            case HideDirection.Down:
                if (!_isHidden) MoveObjectDownwards(true);
                else MoveObjectUpwards(false);
                return;
            case HideDirection.Left:
                if (!_isHidden) MoveObjectLeftwards(true);
                else MoveObjectRightwards(false);
                return;
            case HideDirection.Right:
                if (!_isHidden) MoveObjectRightwards(true);
                else MoveObjectLeftwards(false);
                return;
        }
    }

    private void MoveObjectUpwards(bool isHidingObject)
    {
        Vector3 targetPosition = targetToHide.transform.position + new Vector3(0, targetRect.rect.height, 0);
        _isHidden = isHidingObject;
        StartCoroutine(LerpPosition(targetPosition, hideSpeed));
    }
    
    private void MoveObjectDownwards(bool isHidingObject)
    {
        Vector3 targetPosition = targetToHide.transform.position - new Vector3(0, targetRect.rect.height, 0);
        _isHidden = isHidingObject;
        StartCoroutine(LerpPosition(targetPosition, hideSpeed));
    }
    
    private void MoveObjectLeftwards(bool isHidingObject)
    {
        Vector3 targetPosition = targetToHide.transform.position - new Vector3(targetRect.rect.width, 0, 0);
        _isHidden = isHidingObject;
        StartCoroutine(LerpPosition(targetPosition, hideSpeed));
    }
    
    private void MoveObjectRightwards(bool isHidingObject)
    {
        Vector3 targetPosition = targetToHide.transform.position + new Vector3(targetRect.rect.width, 0, 0);
        _isHidden = isHidingObject;
        StartCoroutine(LerpPosition(targetPosition, hideSpeed));
    }
    
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            _isMoving = true;
            targetToHide.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        
        _isMoving = false;
    }
}
