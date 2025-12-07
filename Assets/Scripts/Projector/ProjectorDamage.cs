using Enemies;
using UnityEngine;

public class ProjectorDamage : MonoBehaviour
{
    [SerializeField] float damagePerSecond = 3f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyHealth hp))
        {
            hp.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
