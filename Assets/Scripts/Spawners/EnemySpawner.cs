using Enemies;
using Markers;
using System.Collections;
using UnityEngine;

namespace Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] GameObject enemyPrefab;
        [SerializeField] float baseSpawnInterval = 2f;
        [SerializeField] float spawnIntervalMin = 0.3f;
        [SerializeField] float spawnRadius = 8f;
        [SerializeField] int maxEnemies = 50;
        [SerializeField] Transform[] spawnPoints;

        [Header("Optional: dynamic difficulty")]
        [SerializeField] Generator generator;

        ClinicTarget clinic;
        int currentCount;
        bool spawning;

        void Awake()
        {
            clinic = FindAnyObjectByType<ClinicTarget>();
            if (generator == null)
                generator = FindAnyObjectByType<Generator>();
        }

        void OnEnable()
        {
            spawning = true;
            StartCoroutine(SpawnLoop());
        }

        void OnDisable()
        {
            spawning = false;
        }

        IEnumerator SpawnLoop()
        {
            while (spawning)
            {
                if (currentCount < maxEnemies)
                {
                    SpawnOne();
                }

                float interval = ComputeInterval();
                yield return new WaitForSeconds(interval);
            }
        }

        void SpawnOne()
        {
            Vector2 pos;
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
                pos = sp.position;
            }
            else
            {
                pos = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
            }

            var go = Instantiate(enemyPrefab, pos, Quaternion.identity, transform);
            var enemy = go.GetComponent<CucumberEnemy>();
            if (enemy != null)
            {
                enemy.Setup(clinic);
            }

            currentCount++;
            StartCoroutine(WatchEnemyDestroyed(go));
        }

        IEnumerator WatchEnemyDestroyed(GameObject go)
        {
            while (go != null)
                yield return null;

            currentCount = Mathf.Max(0, currentCount - 1);
        }

        float ComputeInterval()
        {
            if (generator != null)
            {
                float energyFactor = Mathf.Clamp01(generator.Energy01);
                float multiplier = 1f - energyFactor * 0.7f;
                float result = baseSpawnInterval * multiplier;
                return Mathf.Clamp(result, spawnIntervalMin, baseSpawnInterval);
            }

            return baseSpawnInterval;
        }
    }
}
