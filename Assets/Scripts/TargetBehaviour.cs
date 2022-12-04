using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    public int health;
    [SerializeField] private int baseHealth;
    public float armor = 1; //armor stat sets how often a target can be attacked

    private void Awake()
    {
        health = baseHealth;
    }

    public void DoDamage(int damage)
    {
        health -= damage;
    }
}