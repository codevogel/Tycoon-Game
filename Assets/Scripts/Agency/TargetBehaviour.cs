using UnityEngine;
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
        [SerializeField] ParticleSystem onDamage;

        private void Awake()
        {
            health = baseHealth;
            healthBar.value = (float)health / baseHealth;
        }

        private void Destroyed()
        {
            GameManager.Instance.GameOver();
            gameObject.SetActive(false);
        }

        public void DoDamage(int damage)
        {
            health -= damage;
            healthBar.value = (float)health / baseHealth;
            onDamage.Play();
            
            if (health <= 0)
            {
                Destroyed();
            }
        }
    }
}