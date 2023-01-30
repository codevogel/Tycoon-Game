using UnityEngine;

namespace UI
{
    public class BillboardText : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
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
}