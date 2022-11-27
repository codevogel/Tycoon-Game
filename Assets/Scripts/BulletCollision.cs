using Towers;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] private BaseTower baseTower;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            baseTower.DoDamage(other);
        }
    }
}