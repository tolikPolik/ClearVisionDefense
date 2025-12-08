using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] TMP_Text statsText;
    [SerializeField] TMP_Text patrioticText;

    float survivalTime;
    bool isGameOver;

    void Update()
    {
        if (!isGameOver)
            survivalTime += Time.deltaTime;
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        gameOverScreen.SetActive(true);

        statsText.text = $"Вы продержались: {survivalTime:F1} сек";
        patrioticText.text = GetRandomPatrioticPhrase();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    string GetRandomPatrioticPhrase()
    {
        string[] lines =
        {
            "Вы защитили клинику! Настоящий герой!",
            "Вы показали настоящий дух защитника!",
            "Клиника благодарна вам!",
            "Вы — патриот, стоящий на страже здоровья!",
            "Омск гордится вами!"
        };

        return lines[Random.Range(0, lines.Length)];
    }
}