using System;
using UnityEngine;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private Text _coinCounterText;

    [Header("Game Settings")]
    [SerializeField] private string _coinTag = "Coin";

    private int _totalCoins;
    private int _collectedCoins;
    private bool _gameWon = false;

    // Статическая ссылка для доступа из монеток
    public static WinManager Instance { get; private set; }

    void Awake()
    {
        // Делаем этот объект доступным из других скриптов
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Находим все монетки
        CountAllCoins();

        // Скрываем меню
        if (_winMenu != null)
            _winMenu.SetActive(false);

        UpdateUI();
    }

    void CountAllCoins()
    {
        GameObject[] allCoins = GameObject.FindGameObjectsWithTag(_coinTag);
        _totalCoins = allCoins.Length;
        Debug.Log($"На уровне {_totalCoins} монет");
    }

    // Метод для вызова из монетки
    public void AddCoin()
    {
        if (_gameWon) return;

        _collectedCoins++;
        Debug.Log($"Монетка собрана: {_collectedCoins}/{_totalCoins}");

        UpdateUI();

        if (_collectedCoins >= _totalCoins)
        {
            WinGame();
        }
    }

    void UpdateUI()
    {
        if (_coinCounterText != null)
        {
            _coinCounterText.text = $"Монеты: {_collectedCoins}/{_totalCoins}";
        }
    }

    void WinGame()
    {
        _gameWon = true;
        Debug.Log("🎉 ВСЕ МОНЕТКИ СОБРАНЫ!");

        if (_winMenu != null)
        {
            _winMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }

    // Кнопки в UI
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    internal void CollectCoin()
    {
        throw new NotImplementedException();
    }
}