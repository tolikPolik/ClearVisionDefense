using System;
using UnityEngine;

namespace Markers
{
    public class ClinicTarget : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100f;

        float currentHealth;
        public float Health01 => currentHealth / maxHealth;
        public bool IsAlive => currentHealth > 0f;

        public event Action OnDestroyed;

        void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (!IsAlive) return;

            currentHealth -= amount;
            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                OnDestroyed?.Invoke();
                // место для fx/логики завершения
                Debug.Log("Clinic destroyed");
            }
        }
    }
}
