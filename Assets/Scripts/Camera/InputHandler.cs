using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// Controls the parent "Camera Rig of the main camera. 
/// Controls the menu 
/// Does not control UI
/// </summary>
public class InputHandler : MonoBehaviour
{
    #region WASD variables
    [SerializeField] private float movementTime;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float fastSpeed;
    private float _movementSpeed;
    [SerializeField] private Vector3 newPos;
    #endregion

    #region rotation variables
    [SerializeField] private Quaternion newRotation;
    [SerializeField] private float rotationAmount;
    [SerializeField] private Vector3 rotateStartPos;
    [SerializeField] private Vector3 rotateCurrentPos;
    #endregion

    #region zoom variables 
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 zoomAmount;
    [SerializeField] private Vector3 newZoom;
    #endregion

    #region mouse variables
    [SerializeField] private Vector3 dragStartPosition;
    [SerializeField] private Vector3 dragCurrentPosition;
    #endregion

    #region menu variables 
    [SerializeField] private GameObject popup;
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




            newZoom.y += Input.mouseScrollDelta.y * zoomAmount.y;

            newZoom.y = Mathf.Clamp(newZoom.y, 0, 50);
            // if (newZoom.y < 0) { newZoom.y = 0; }
            // if (newZoom.y > 50) { newZoom.y = 50; } 

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

        if (Input.GetKey(KeyCode.W)) newPos += (transform.forward * _movementSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) newPos += (transform.right * -_movementSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) newPos += (transform.forward * -_movementSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) newPos += (transform.right * _movementSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Q)) newRotation *= Quaternion.Euler(Vector3.up * rotationAmount * Time.deltaTime);
        if (Input.GetKey(KeyCode.E)) newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount * Time.deltaTime);

        //if pause menu active close pause menu ,
        //if popup venster active close popup 
        //if none of the above open pause menu
        if (Input.GetKey(KeyCode.Escape))
        {
            popup.SetActive(false);
        }
        #endregion
    }
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}