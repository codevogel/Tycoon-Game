using UnityEngine;

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
}