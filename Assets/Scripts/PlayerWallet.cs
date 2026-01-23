using UnityEngine;
using TMPro; // Обязательно добавьте эту строку в начало скрипта

public class PlayerWalletsasusam : MonoBehaviour
{
    [SerializeField] private int totalCoins = 0;
    // Перетащите ваш UI Text (TMP) сюда в Инспекторе
    [SerializeField] private TextMeshProUGUI coinUIText;

    void Start()
    {
        // Убедимся, что UI обновляется сразу при старте сцены (показывая 0 монет)
        UpdateUI();
    }

    void OnTriggerEnter(Collider other)
    {
        Coin coin = other.GetComponent<Coin>();

        if (coin != null)
        {
            int collectedValue = coin.Collect();
            totalCoins += collectedValue;

            // Вызываем метод обновления UI
            UpdateUI();
        }
    }

    // Метод для обновления текстового элемента UI
    private void UpdateUI()
    {
        if (coinUIText != null)
        {
            // Устанавливаем текст: "Монеты: [количество]"
            coinUIText.text =  totalCoins.ToString() + "/10";
        }
    }
}