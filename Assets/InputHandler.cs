using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour,
IPointerUpHandler, IPointerDownHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
  #region WASD input variables
  public Camera sceneCamera;
  private Vector3 _movementInput;
  private Vector3 _moveDirection;
  private Vector2 _mousePosition;
  #endregion

  #region rotation variables 
  private Vector3 _firstPoint;
  private Vector3 _secondPoint;

  private float _xAngle;
  private float _yAngle;
  private float _xAngTemp;
  private float _yAngTemp;
  private bool _isMidMouseDown = false;

  #endregion

  #region Onpointer methods
  public void OnPointerClick(PointerEventData eventData)
  {
    throw new System.NotImplementedException();
  }

  //OnPointerDown is also required to receive OnPointerUp callbacks
  public void OnPointerDown(PointerEventData eventData)
  {
    throw new System.NotImplementedException();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    throw new System.NotImplementedException();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    throw new System.NotImplementedException();
  }

  public void OnPointerMove(PointerEventData eventData)
  {
    throw new System.NotImplementedException();
  }

  //Do this when the mouse click on this selectable UI object is released.
  //is also required to receive OnPointerDown callbacks
  public void OnPointerUp(PointerEventData eventData)
  {
    throw new System.NotImplementedException();
  }

  #endregion

  private void Update()
  {
    #region WASD Input
    _movementInput.x = Input.GetAxisRaw("Horizontal");
    _movementInput.z = Input.GetAxisRaw("Vertical");
    _moveDirection = new Vector3(_movementInput.x, 0f, _movementInput.z).normalized;//normalized so diagonal goes same speed
    _mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
    #endregion

    #region rotation input
    if (Input.GetMouseButtonDown(2))
    {
      _firstPoint = Input.mousePosition;
      _xAngTemp = _xAngle;
      _yAngTemp = _yAngle;
      _isMidMouseDown = true;
    }

    if (Input.GetMouseButtonUp(2))
    {
      _isMidMouseDown = false;
    }

    if (_isMidMouseDown)
    {
      _secondPoint = Input.mousePosition;
      _xAngle = _xAngTemp + (_secondPoint.x - _firstPoint.x) * 180.0f / Screen.width;
      _yAngle = _yAngTemp - (_secondPoint.y - _firstPoint.y) * 90.0f / Screen.height;
      transform.rotation = Quaternion.Euler(_yAngle, _xAngle, 0.0f);
    }
    #endregion
  }

  private void FixedUpdate()
  {
    #region WASD movement
    sceneCamera.transform.position += new Vector3(_moveDirection.x, 0f, _moveDirection.z);
    #endregion
  }
}
