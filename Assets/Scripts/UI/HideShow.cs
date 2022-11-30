using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This enum is created so you can select the direction an object has to move in.
/// </summary>
public enum HideDirection{
    Up,
    Down,
    Left,
    Right
}

/// <summary>
/// This class is created to move objects on the vertical or horizontal axis
/// </summary>
public class HideShow : MonoBehaviour
{
    [SerializeField] private GameObject targetToHide;
    public RectTransform targetRect;
    [SerializeReference] private HideDirection direction;
    private bool _isHidden;
    public float hideSpeed;
    public bool isMoving;
    
    private void Start()
    {
        targetRect = (RectTransform)targetToHide.transform;
        _isHidden = false;
        hideSpeed = 0.5f;
        isMoving = false;
    }

    /// <summary>
    /// This method uses a switch case to see which direction an object has to be moved to.
    /// </summary>
    /// <param name="direction">Gets the direction enum from a given script</param>
    public void HideObject(HideShow direction)
    {
        if(isMoving) return;
        
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

    /// <summary>
    /// Move the object up based on the object height
    /// </summary>
    /// <param name="isHidingObject"></param>
    private void MoveObjectUpwards(bool isHidingObject)
    {
        Vector3 targetPosition = targetToHide.transform.position + new Vector3(0, targetRect.rect.height, 0);
        _isHidden = isHidingObject;
        StartCoroutine(LerpPosition(targetPosition, hideSpeed));
    }
    
    /// <summary>
    /// Move the object down based on object height
    /// </summary>
    /// <param name="isHidingObject"></param>
    private void MoveObjectDownwards(bool isHidingObject)
    {
        Vector3 targetPosition = targetToHide.transform.position - new Vector3(0, targetRect.rect.height, 0);
        _isHidden = isHidingObject;
        StartCoroutine(LerpPosition(targetPosition, hideSpeed));
    }
    
    /// <summary>
    /// Move the object to the left by object width
    /// </summary>
    /// <param name="isHidingObject"></param>
    private void MoveObjectLeftwards(bool isHidingObject)
    {
        Vector3 targetPosition = targetToHide.transform.position - new Vector3(targetRect.rect.width, 0, 0);
        _isHidden = isHidingObject;
        StartCoroutine(LerpPosition(targetPosition, hideSpeed));
    }
    
    /// <summary>
    /// Move the object to the right by object width
    /// </summary>
    /// <param name="isHidingObject"></param>
    private void MoveObjectRightwards(bool isHidingObject)
    {
        Vector3 targetPosition = targetToHide.transform.position + new Vector3(targetRect.rect.width, 0, 0);
        _isHidden = isHidingObject;
        StartCoroutine(LerpPosition(targetPosition, hideSpeed));
    }
    
    /// <summary>
    /// This IEnumerator moves the target object using a lerp to a target position
    /// </summary>
    /// <param name="targetPosition">Desired position of the target object</param>
    /// <param name="duration">How long the movement lasts</param>
    /// <returns></returns>
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            isMoving = true;
            targetToHide.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        
        isMoving = false;
    }
}
