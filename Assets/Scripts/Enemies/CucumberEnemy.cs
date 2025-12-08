using Enemies;
using Markers;
using UnityEngine;

public class CucumberEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float baseSpeed = 2f;

    ClinicTarget clinic;
    EnemyHealth health;
    Waves waves;

    Vector2 targetPoint;
    float speed;

    void Awake()
    {
        TryGetComponent(out health);
    }

    public void Setup(ClinicTarget clinic, Waves waves)
    {
        this.clinic = clinic;
        this.waves = waves;

        if (health != null && waves != null)
            health.ApplyDifficulty(waves.Difficulty);

        speed = baseSpeed * GetSpeedMultiplier();

        PickRandomPoint();
    }

    void Update()
    {
        if (clinic == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        Vector2 pos = transform.position;
        Vector2 dir = (targetPoint - pos);

        if (dir.sqrMagnitude < 0.01f)
        {
            clinic.TakeDamage(Time.deltaTime * 4f);
            return;
        }

        dir.Normalize();
        pos += dir * (speed * Time.deltaTime);

        transform.position = pos;
    }

    void PickRandomPoint()
    {
        var col = clinic.GetComponent<BoxCollider2D>();

        if (col == null)
        {
            targetPoint = clinic.transform.position;
            return;
        }

        Bounds b = col.bounds;

        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);

        targetPoint = new Vector2(x, y);
    }

    float GetSpeedMultiplier()
    {
        if (waves == null) return 1f;

        return Mathf.Lerp(1f, 1.8f, waves.Difficulty);
    }
}