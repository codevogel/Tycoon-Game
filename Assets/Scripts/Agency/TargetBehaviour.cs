using UnityEngine;

namespace Agency
{
    public class TargetBehaviour : MonoBehaviour
    {
        public int health;
        [SerializeField] private int baseHealth;
        public float armor = 1; //armor stat sets how often a target can be attacked
        [SerializeField] ParticleSystem onDamage;

        private void Awake()
        {
            health = baseHealth;
        }

        private void Update()
        {
            Destroyed();
        }

        private void Destroyed()
        {
            if (health <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        public void DoDamage(int damage)
        {
            health -= damage;
            onDamage.Play();
        }
    }
}