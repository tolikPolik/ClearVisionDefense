using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    bool isPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                SetPause();
        }
    }

    public void SetPause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
