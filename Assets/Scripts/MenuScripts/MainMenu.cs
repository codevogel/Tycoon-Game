using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Main Menu Button logic
/// </summary>
public class MainMenu : MonoBehaviour
{
  public void StartGame()
  {
    SceneManager.LoadScene("SampleScene"); 
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
