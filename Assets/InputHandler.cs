using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour,
IPointerUpHandler, IPointerDownHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
  public Camera sceneCamera;
  #region input parameters
  private Vector3 _movementInput;
  private Vector3 _moveDirection;
  private Vector2 _mousePosition;
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

  void Update()
  {
    #region input
    _movementInput.x = Input.GetAxisRaw("Horizontal");
    _movementInput.z = Input.GetAxisRaw("Vertical");
    _moveDirection = new Vector3(_movementInput.x, 0f, _movementInput.z).normalized;//normalized so diagonal goes same speed
    _mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
    #endregion
  }

  void FixedUpdate()
  {
    sceneCamera.transform.position += new Vector3(_moveDirection.x, 0f, _moveDirection.z);
  }
}
