using Enemies;
using Markers;
using System.Collections;
using UnityEngine;

namespace Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameObject enemyPrefab;
        [SerializeField] Generator generator;

        [Header("Spawn Settings")]
        [SerializeField] float baseSpawnInterval = 2f;
        [SerializeField] float spawnIntervalMin = 0.3f;
        [SerializeField] float spawnRadius = 8f;
        [SerializeField] Transform[] spawnPoints;

        [Header("Limits")]
        [SerializeField] int maxEnemies = 50;

        ClinicTarget clinic;
        Waves waves;

        int currentCount;
        bool spawning;

        void Awake()
        {
            clinic = FindAnyObjectByType<ClinicTarget>();
            waves = FindAnyObjectByType<Waves>();

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
                    SpawnOne();

                float interval = ComputeInterval();
                yield return new WaitForSeconds(interval);
            }
        }

        void SpawnOne()
        {
            Vector2 pos;

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
                pos = sp.position;
            }
            else
            {
                pos = (Vector2)transform.position +
                      Random.insideUnitCircle.normalized * spawnRadius;
            }

            GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity, transform);
            currentCount++;

            if (go.TryGetComponent(out CucumberEnemy enemy))
                enemy.Setup(clinic, waves);

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
            float diffFactor = waves ? Mathf.Lerp(1f, 0.3f, waves.Difficulty) : 1f;

            float energyFactor = generator ? Mathf.Clamp01(generator.Energy01) : 1f;
            float energyMult = 1f - energyFactor * 0.7f;

            float interval = baseSpawnInterval * diffFactor * energyMult;

            return Mathf.Clamp(interval, spawnIntervalMin, baseSpawnInterval);
        }
    }
}