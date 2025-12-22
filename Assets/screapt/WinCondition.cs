using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    [Header("Экран победы")]
    [SerializeField] private GameObject winUI;

    [Header("Настройки")]
    [SerializeField] private string coinTag = "coin";

    void Start()
    {
        // Выключаем экран победы при старте
        if (winUI != null)
        {
            winUI.SetActive(false);
        }

        Debug.Log("Система победы запущена");
    }

    void Update()
    {
        // Постоянно проверяем, остались ли монетки на сцене
        CheckForWin();
    }

    void CheckForWin()
    {
        // Если экран уже показан - выходим
        if (winUI != null && winUI.activeSelf)
            return;

        // Ищем все монетки с тегом "Coin"
        GameObject[] coins = GameObject.FindGameObjectsWithTag(coinTag);

        // Если монеток не осталось
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

            // Останавливаем время
            Time.timeScale = 0f;

            // Включаем курсор
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Debug.Log("ПОБЕДА! Все монетки собраны!");
        }
    }

    // Методы для кнопок UI
    public void HideWinScreen()
    {
        if (winUI != null)
        {
            winUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}