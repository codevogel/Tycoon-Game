using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScripts
{
    /// <summary>
    /// Main Menu Button logic
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string scene;

        public void StartGame()
        {
            SceneManager.LoadScene(scene);
        }

        public void ContinueGame()
        {
            //SceneManager.LoadScene(lastSavedGame);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}