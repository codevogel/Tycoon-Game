using Architect.Placeables;
using UnityEngine;

namespace Agency.Towers
{
    /// <summary>
    /// Checks if enemies have collisions with triggers.
    /// </summary>
    public class EnemyDetection : MonoBehaviour
    {
        [SerializeField] private BaseTower baseTower;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            baseTower.AddEnemyToList(other.gameObject);
            baseTower.NewTarget();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            baseTower.RemoveEnemyFromList(other.gameObject);
            baseTower.NewTarget();
        }
    }
}