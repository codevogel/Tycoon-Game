using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the game state.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int enemyBaseHealth = 100;
    public Transform globalTarget;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] private GameObject gameOverCanvas;

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
    }

    public void GoToScene(int scene)
    {
       
        SceneManager.LoadScene(scene);
    }
}