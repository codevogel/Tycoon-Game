using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardText : MonoBehaviour
{
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_camera != null)
        {
            var cameraTransform = _camera.transform;
            var position = cameraTransform.position;
            transform.LookAt(new Vector3(
                x: position.x, 
                45,
                z: position.z)
            );
        }

        transform.Rotate(new Vector3(0, 180, 0));
    }
}
