using UnityEngine;

namespace MenuScripts
{
    /// <summary>
    /// Controls the main menu cube.
    /// </summary>
    public class MainMenuCubeMovement : MonoBehaviour
    {
        float _speed = 1;
        void Update()
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * _speed);
        }
    }
}
