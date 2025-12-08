using UnityEngine;

public class Waves : MonoBehaviour
{
    [SerializeField] float difficultyGrowthPerSecond = 0.03f;
    [SerializeField] float maxDifficulty = 4f;

    public float Difficulty { get; private set; } = 1f;

    void Update()
    {
        Difficulty += difficultyGrowthPerSecond * Time.deltaTime;
        Difficulty = Mathf.Clamp(Difficulty, 1f, maxDifficulty);
    }
}
