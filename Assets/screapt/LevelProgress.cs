using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    // Ключ для сохранения в PlayerPrefs
    private const string UNLOCKED_LEVELS_KEY = "UnlockedLevels";

    // Статическая переменная для доступа из любых скриптов
    public static LevelProgress Instance;

    void Awake()
    {
        // Делаем скрипт доступным из любого места
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 1. Сохраняем, что уровень пройден
    public void LevelCompleted(int levelNumber)
    {
        // Разблокируем следующий уровень
        UnlockLevel(levelNumber + 1);
        Debug.Log($"Уровень {levelNumber} пройден! Теперь доступен уровень {levelNumber + 1}");
    }

    // 2. Разблокируем уровень
    public void UnlockLevel(int levelNumber)
    {
        // Получаем текущее значение
        int unlocked = GetUnlockedLevelsCount();

        // Если новый уровень больше текущего - сохраняем
        if (levelNumber > unlocked)
        {
            PlayerPrefs.SetInt(UNLOCKED_LEVELS_KEY, levelNumber);
            PlayerPrefs.Save();
        }
    }

    // 3. Проверяем, доступен ли уровень
    public bool IsLevelUnlocked(int levelNumber)
    {
        return levelNumber <= GetUnlockedLevelsCount();
    }

    // 4. Получаем количество открытых уровней
    public int GetUnlockedLevelsCount()
    {
        // По умолчанию 1 уровень открыт
        return PlayerPrefs.GetInt(UNLOCKED_LEVELS_KEY, 1);
    }

    // 5. Сброс прогресса (для теста)
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(UNLOCKED_LEVELS_KEY);
        Debug.Log("Прогресс сброшен!");
    }
}