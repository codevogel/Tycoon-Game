using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause Menu buttons and menu logic.
/// Entering the menu sets the games timescale to 0. 
/// </summary>
public class PauseMenu : MonoBehaviour
{
  [SerializeField] GameObject pauseMenu;
  public static bool isPaused;
  void Start()
  {
    pauseMenu.SetActive(false);
  }
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (isPaused) ResumeGame(); else PauseGame();
    }
  }

  public void PauseGame()
  {
    pauseMenu.SetActive(true);
    Time.timeScale = 0f;
    isPaused = true;
  }
  public void ResumeGame()
  {
    pauseMenu.SetActive(false);
    Time.timeScale = 1f;
    isPaused = false;
  }

  public void GoToMainMenu()
  {
    Time.timeScale = 1f;
    SceneManager.LoadScene("MainMenu");
    isPaused = false;
  }
  public void QuitGame()
  {
    Application.Quit();
  }
}
