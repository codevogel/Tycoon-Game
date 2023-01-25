using UnityEngine;

namespace UI
{
    public class UIPauseScript : MonoBehaviour
    {


        public void PauseGame()
        {
            Time.timeScale = 0f;
        }
        public void ResumeGame()
        {
            Time.timeScale = 1;
        }
    }
}
