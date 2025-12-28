using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private GameObject winUI;
    [SerializeField] private string coinTag = "Coin";
    [SerializeField] private int currentLevelNumber = 1; // Добавил номер уровня

    void Start()
    {
        // ВАЖНО: Всегда сбрасываем время при старте сцены
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (winUI != null)
        {
            winUI.SetActive(false);
        }

        // Проверка наличия LevelProgress
        if (FindObjectOfType<LevelProgress>() == null)
        {
            Debug.LogWarning("LevelProgress не найден. Прогресс не будет сохраняться!");
        }
    }

    void Update()
    {
        if (winUI != null && winUI.activeSelf)
            return;

        GameObject[] coins = GameObject.FindGameObjectsWithTag(coinTag);

        if (coins.Length == 0)
        {
            ShowWinScreen();
        }
    }

    void ShowWinScreen()
    {
        if (winUI != null)
        {
            winUI.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // СОХРАНЯЕМ ПРОГРЕСС СРАЗУ ПРИ ПОБЕДЕ
            SaveLevelProgress();
        }
    }

    void SaveLevelProgress()
    {
        if (LevelProgress.Instance != null)
        {
            LevelProgress.Instance.LevelCompleted(currentLevelNumber);
            Debug.Log($"Уровень {currentLevelNumber} пройден! Открыт уровень {currentLevelNumber + 1}");
        }
        else
        {
            Debug.LogError("LevelProgress.Instance равен null! Прогресс не сохранён.");
        }
    }

    // Измененные методы для кнопок:

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        // Автоматически определяем следующий уровень
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Если уровни кончились - идём в меню
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ContinueGame()
    {
        if (winUI != null)
        {
            winUI.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}