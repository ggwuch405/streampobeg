using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    [Header("Main Menu Window")]
    [SerializeField] private GameObject _mainMenuWindow;

    [Header("Target Window to Open")]
    [SerializeField] private GameObject _targetWindow;

    [Header("Button Reference")]
    [SerializeField] private Button _openWindowButton;

    private void Start()
    {
        // Если кнопка не назначена в инспекторе, ищем на этом объекте
        if (_openWindowButton == null)
        {
            _openWindowButton = GetComponent<Button>();
        }

        if (_openWindowButton != null)
        {
            _openWindowButton.onClick.AddListener(OpenTargetWindow);
        }

        // Инициализация: показываем главное меню, скрываем целевое окно
        if (_mainMenuWindow != null)
            _mainMenuWindow.SetActive(true);

        if (_targetWindow != null)
            _targetWindow.SetActive(false);
    }

    public void OpenTargetWindow()
    {
        // Скрываем текущее окно
        if (_mainMenuWindow != null)
            _mainMenuWindow.SetActive(false);

        // Показываем целевое окно
        if (_targetWindow != null)
        {
            _targetWindow.SetActive(true);
            Debug.Log("Opened window: " + _targetWindow.name);
        }
    }

    public void CloseTargetWindow()
    {
        // Скрываем целевое окно
        if (_targetWindow != null)
            _targetWindow.SetActive(false);

        // Показываем главное меню
        if (_mainMenuWindow != null)
            _mainMenuWindow.SetActive(true);
    }

    private void OnDestroy()
    {
        if (_openWindowButton != null)
        {
            _openWindowButton.onClick.RemoveListener(OpenTargetWindow);
        }
    }
}