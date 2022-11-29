using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    public int health;
    [SerializeField] private int baseHealth;
    public float armor = 1;
    private float _timer;


    private void Awake()
    {
        health = baseHealth;
    }

    public void DoDamage(int damage)
    {
  
        if (!(_timer > armor)) return; //armor stat sets how often a target can be attacked

        health -= damage;
        _timer = 0;
    }

    private void Update()
    {
        SetTimer();
    }

    private void SetTimer()
    {
        _timer += Time.deltaTime;
    }
}