using UnityEngine;
using UnityEngine.UI;

public class BasicButtonHandler : MonoBehaviour
{
    // Сериализованное поле для привязки кнопки в инспекторе
    [SerializeField] private Button _button;

    // Сериализованное поле для настройки через инспектор
    [SerializeField] private string message = "Button clicked!";

    private void Start()
    {
        // Проверяем, присвоена ли кнопка
        if (_button != null)
        {
            // Добавляем обработчик события нажатия
            _button.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Button is not assigned in the inspector!");
        }
    }

    private void OnButtonClicked()
    {
        Debug.Log(message);
        // Здесь добавляйте свою логику
    }

    private void OnDestroy()
    {
        // Важно удалять слушателей при уничтожении объекта
        if (_button != null)
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}