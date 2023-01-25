using UnityEngine;
using UnityEngine.Events;
using Unity.UI;
using UnityEngine.UI;

namespace Agency
{
    public class TargetBehaviour : MonoBehaviour
    {
        public int health;
        [SerializeField] private int baseHealth;

        [SerializeField] private Slider healthBar;
        public float armor = 1; //armor stat sets how often a target can be attacked
        public UnityEvent onDamage;

        private void Awake()
        {
            health = baseHealth;
            healthBar.value = (float) health / baseHealth;
        }

        private void Destroyed()
        {
            GameManager.Instance.GameOver();
            gameObject.SetActive(false);
        }

        public void DoDamage(int damage)
        {
            health -= damage;
            onDamage.Invoke();
            healthBar.value = (float) health / baseHealth;

            if (health <= 0)
            {
                Destroyed();
            }
        }
    }
}