using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int EnemyBaseHealth = 100;

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