using UnityEngine;

namespace Towers
{
    public class EnemyDetection : MonoBehaviour
    {
        [SerializeField] private BaseTower baseTower;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            baseTower.AddEnemyToList(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            baseTower.RemoveEnemyFromList(other.gameObject);
        }
    }
}