using UnityEngine;

namespace UI
{
    /// <summary>
    /// Handles behaviour for play/pause buildings
    /// </summary>
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
