using UnityEngine;

namespace UI
{
    public class UIPauseScript : MonoBehaviour
    {

        [SerializeField] GameObject play;
        [SerializeField] GameObject pause;
        public void PauseGame()
        {

            play.SetActive(true);
            pause.SetActive(false);
            Time.timeScale = 0f;
        }
        public void ResumeGame()
        {
            play.SetActive(false);
            pause.SetActive(true);
            Time.timeScale = 1;
        }
    }
}
