using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private GameObject winUI;
    [SerializeField] private string coinTag = "Coin";

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
        }
    }

    // Измененные методы для кнопок:

    public void RestartLevel()
    {
        // ВАЖНО: Восстанавливаем время перед загрузкой сцены
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
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