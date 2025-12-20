using UnityEngine;
using UnityEngine.UI;

public class SimpleMenuSwitcher : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _levelSelectionUI;

    private void Start()
    {
        // Автопоиск, если не назначено
        if (_playButton == null)
            _playButton = GameObject.Find("PlayButton")?.GetComponent<Button>();

        if (_closeButton == null)
            _closeButton = GameObject.Find("CloseButton")?.GetComponent<Button>();

        if (_mainMenuUI == null)
            _mainMenuUI = GameObject.Find("MainMenu");

        if (_levelSelectionUI == null)
            _levelSelectionUI = GameObject.Find("LevelSelection");

        // Назначаем обработчики
        if (_playButton != null)
            _playButton.onClick.AddListener(OpenLevelSelection);

        if (_closeButton != null)
            _closeButton.onClick.AddListener(CloseLevelSelection);

        // Начальное состояние
        SetMainMenuActive(true);
    }

    public void OpenLevelSelection()
    {
        SetMainMenuActive(false);
        SetLevelSelectionActive(true);
        Debug.Log("Открыт выбор уровней");
    }

    public void CloseLevelSelection()
    {
        SetLevelSelectionActive(false);
        SetMainMenuActive(true);
        Debug.Log("Закрыт выбор уровней, кнопка 'Играть' снова активна");
    }

    private void SetMainMenuActive(bool active)
    {
        if (_mainMenuUI != null)
        {
            _mainMenuUI.SetActive(active);

            // Управляем интерактивностью
            CanvasGroup cg = _mainMenuUI.GetComponent<CanvasGroup>();
            if (cg == null) cg = _mainMenuUI.AddComponent<CanvasGroup>();

            cg.interactable = active;
            cg.blocksRaycasts = active;
        }

        // Активируем кнопку "Играть"
        if (_playButton != null && active)
        {
            _playButton.interactable = true;
            _playButton.Select(); // Фокус для управления с клавиатуры
        }
    }

    private void SetLevelSelectionActive(bool active)
    {
        if (_levelSelectionUI != null)
        {
            _levelSelectionUI.SetActive(active);

            // Управляем интерактивностью
            CanvasGroup cg = _levelSelectionUI.GetComponent<CanvasGroup>();
            if (cg == null) cg = _levelSelectionUI.AddComponent<CanvasGroup>();

            cg.interactable = active;
            cg.blocksRaycasts = active;
        }

        // Деактивируем кнопку "Играть" при открытии окна уровней
        if (_playButton != null && active)
        {
            _playButton.interactable = false;
        }
    }

    // Для управления с клавиатуры
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_levelSelectionUI != null && _levelSelectionUI.activeSelf)
            {
                CloseLevelSelection();
            }
        }
    }
}