using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScripts
{
  /// <summary>
  /// Pause Menu buttons and menu logic.
  /// Entering the menu sets the games timescale to 0. 
  /// </summary>
  public class PauseMenu : MonoBehaviour
  {
    [SerializeField] GameObject pauseMenu;
    public static bool IsPaused;

    private void Start()
    {
      pauseMenu.SetActive(false);
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
        if (IsPaused) ResumeGame(); else PauseGame();
      }
    }

    public void PauseGame()
    {
      pauseMenu.SetActive(true);
      Time.timeScale = 0f;
      IsPaused = true;
    }
    public void ResumeGame()
    {
      pauseMenu.SetActive(false);
      Time.timeScale = 1f;
      IsPaused = false;
    }

    public void GoToMainMenu()
    {
      Time.timeScale = 1f;
      SceneManager.LoadScene("MainMenu");
      IsPaused = false;
    }
    public void QuitGame()
    {
      Application.Quit();
    }
  }
}
