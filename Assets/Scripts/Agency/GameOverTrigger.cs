using UnityEngine;

namespace Agency
{
    public class GameOverTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (!other.isTrigger)
                {
                    GameManager.Instance.GameOver();
                }
            }
        }
    }
}