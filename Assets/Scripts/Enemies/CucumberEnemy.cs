using Markers;
using UnityEngine;

namespace Enemies
{
    public class CucumberEnemy : MonoBehaviour
    {
        [SerializeField] float speed = 2f;
        [SerializeField] float damage = 10f;
        [SerializeField] float reachDistance = 0.35f; // расстояние, при котором считаем "достигли"
        [SerializeField] AudioClip hitSfx; // опционально

        ClinicTarget clinic;
        bool isMoving;

        void Start()
        {
            // попробуем получить явную ссылку заранее (если спавнер передал, то поле clinic уже установлен)
            if (clinic == null)
                clinic = FindAnyObjectByType<ClinicTarget>();

            isMoving = clinic != null;
        }

        void Update()
        {
            if (!isMoving || clinic == null) return;

            Vector3 dir = (clinic.transform.position - transform.position);
            float dist = dir.magnitude;

            if (dist <= reachDistance)
            {
                ReachClinic();
                return;
            }

            transform.position += dir.normalized * speed * Time.deltaTime;
            // можно добавить простую поворотную анимацию здесь
        }

        void ReachClinic()
        {
            if (clinic != null && clinic.IsAlive)
            {
                clinic.TakeDamage(damage);
                if (hitSfx) AudioSource.PlayClipAtPoint(hitSfx, transform.position);
            }

            Die();
            Debug.Log("Enemy reached clinic!");
        }

        void Die()
        {
            // TODO: заменить на пулл-реализацию при необходимости
            Destroy(gameObject);
        }

        // публичный метод для спавнера, чтобы передать цель (рекомендуется вместо FindAnyObject...)
        public void Setup(ClinicTarget target)
        {
            clinic = target;
            isMoving = clinic != null;
        }
    }
}
