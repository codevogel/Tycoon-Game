using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// Controls the parent "Camera Rig of the main camera. 
/// Instead of controlling the camera itself.
/// </summary>
public class CameraController : MonoBehaviour
{
  #region WASD variables
  public float movementTime;
  public float normalSpeed;
  public float fastSpeed;
  private float _movementSpeed;
  public Vector3 newPos;
  #endregion

  #region rotation variables
  public Quaternion newRotation;
  public float rotationAmount;
  public Vector3 rotateStartPos;
  public Vector3 rotateCurrentPos;
  #endregion

  #region zoom variables 
  public Transform cameraTransform;
  public Vector3 zoomAmount;
  public Vector3 newZoom;
  #endregion

  #region mouse variables
  public Vector3 dragStartPosition;
  public Vector3 dragCurrentPosition;

  private Vector3 _oldMousePos;
  #endregion

  private void Start()
  {
    newPos = transform.position;
    newRotation = transform.rotation;
    newZoom = cameraTransform.localPosition;
  }

  private void Update()
  {
    #region MouseInputs

    if (Input.mouseScrollDelta.y != 0)
    {
      newZoom += Input.mouseScrollDelta.y * zoomAmount;
    }
    #endregion

    #region KeyInputs 
    if (Input.GetKey(KeyCode.LeftShift))
    {
      _movementSpeed = fastSpeed;
    }
    else
    {
      _movementSpeed = normalSpeed;
    }

    if (Input.GetKey(KeyCode.W)) newPos += (transform.forward * _movementSpeed);
    if (Input.GetKey(KeyCode.A)) newPos += (transform.right * -_movementSpeed);
    if (Input.GetKey(KeyCode.S)) newPos += (transform.forward * -_movementSpeed);
    if (Input.GetKey(KeyCode.D)) newPos += (transform.right * _movementSpeed);

    if (Input.GetKey(KeyCode.Q)) newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
    if (Input.GetKey(KeyCode.E)) newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
    #endregion

  }
  private void LateUpdate()
  {
    transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * movementTime);
    transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
  }
}