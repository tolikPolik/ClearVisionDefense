using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] float maxHealth = 3f;

        float currentHealth;

        public event Action OnDied;

        void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;

            if (currentHealth <= 0f)
                Die();
        }

        void Die()
        {
            OnDied?.Invoke();
            Destroy(gameObject);
        }
    }
}
