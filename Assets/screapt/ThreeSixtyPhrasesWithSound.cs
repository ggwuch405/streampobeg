using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ThreeSixtyPhrases : MonoBehaviour
{
    [Header("Фразы")]
    public List<string> phrases = new List<string>()
    {
        "Вижу тебя!",
        "Нашел!",
        "Обнаружил!",
        "Есть контакт!",
        "Ага, попался!"
    };

    [Header("Звуки")]
    public List<AudioClip> soundClips;
    public AudioClip defaultSound;

    [Header("Настройки")]
    public float maxDistance = 15f;
    public float detectionTime = 0.5f;

    private Transform player;
    private AudioSource audioSource;
    private bool playerVisible = false;
    private bool wasPlayerVisible = false;
    private float visibilityTimer = 0f;
    private bool hasSpokenThisVisibility = false;

    // Система перемешивания фраз
    private List<int> availablePhraseIndices = new List<int>();
    private int lastUsedPhraseIndex = -1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
        }

        // Инициализируем систему перемешивания
        InitializePhraseSystem();

        Debug.Log($"Панель инициализирована. Всего фраз: {phrases.Count}");
    }

    void InitializePhraseSystem()
    {
        // Создаем список индексов всех фраз
        availablePhraseIndices.Clear();
        for (int i = 0; i < phrases.Count; i++)
        {
            availablePhraseIndices.Add(i);
        }

        // Перемешиваем список
        ShuffleList(availablePhraseIndices);

        Debug.Log($"Система фраз инициализирована. Доступно фраз: {availablePhraseIndices.Count}");
    }

    void Update()
    {
        if (player == null) return;

        bool isVisibleNow = CanSeePlayer();

        if (isVisibleNow && !wasPlayerVisible)
        {
            visibilityTimer = 0f;
            hasSpokenThisVisibility = false;
        }

        if (isVisibleNow)
        {
            visibilityTimer += Time.deltaTime;

            if (visibilityTimer >= detectionTime && !hasSpokenThisVisibility)
            {
                SayPhraseInOrder();
                hasSpokenThisVisibility = true;
            }
        }
        else
        {
            visibilityTimer = 0f;

            // Когда игрок скрывается - сбрасываем флаг для следующего появления
            if (!isVisibleNow && wasPlayerVisible)
            {
                // Опционально: можно сказать фразу при скрытии
                // SayLostPhrase();
            }
        }

        wasPlayerVisible = isVisibleNow;
        playerVisible = isVisibleNow;
    }

    void SayPhraseInOrder()
    {
        if (phrases.Count == 0) return;

        // Если все фразы использованы - перетасовываем заново
        if (availablePhraseIndices.Count == 0)
        {
            InitializePhraseSystem();
        }

        // Берем первую фразу из перемешанного списка
        int phraseIndex = availablePhraseIndices[0];
        availablePhraseIndices.RemoveAt(0);

        string phrase = phrases[phraseIndex];

        // Проигрываем звук
        PlaySound(phraseIndex);

        Debug.Log($"?? {gameObject.name}: {phrase}");
        Debug.Log($"Осталось неповторенных фраз: {availablePhraseIndices.Count}");

        lastUsedPhraseIndex = phraseIndex;
    }

    // Вариант 2: Случайная фраза, но без повторения предыдущей
    void SayPhraseNoRepeat()
    {
        if (phrases.Count == 0) return;

        // Если только одна фраза - говорим её
        if (phrases.Count == 1)
        {
            SayPhraseByIndex(0);
            return;
        }

        // Выбираем случайную фразу, отличную от предыдущей
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, phrases.Count);
        }
        while (randomIndex == lastUsedPhraseIndex);

        SayPhraseByIndex(randomIndex);
        lastUsedPhraseIndex = randomIndex;
    }

    // Вариант 3: Весовая система (некоторые фразы чаще)
    void SayPhraseWeighted()
    {
        if (phrases.Count == 0) return;

        // Создаем веса для фраз (можно настроить в инспекторе)
        List<float> weights = new List<float>() { 1f, 1f, 1f, 1f, 0.5f }; // Последняя реже

        // Нормализуем веса
        float totalWeight = 0f;
        foreach (float w in weights) totalWeight += w;

        // Выбираем случайную фразу с учетом весов
        float randomValue = Random.Range(0f, totalWeight);
        float currentSum = 0f;
        int selectedIndex = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            currentSum += weights[i];
            if (randomValue <= currentSum)
            {
                selectedIndex = i;
                break;
            }
        }

        SayPhraseByIndex(selectedIndex);
    }

    void SayPhraseByIndex(int index)
    {
        if (index < 0 || index >= phrases.Count) return;

        string phrase = phrases[index];
        PlaySound(index);
        Debug.Log($"?? {gameObject.name}: {phrase}");
    }

    void PlaySound(int phraseIndex)
    {
        if (audioSource == null) return;

        if (soundClips != null && phraseIndex < soundClips.Count)
        {
            AudioClip clip = soundClips[phraseIndex];
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
                return;
            }
        }

        if (defaultSound != null)
        {
            audioSource.PlayOneShot(defaultSound);
        }
    }

    // Перемешивание списка (алгоритм Фишера-Йетса)
    void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    [ContextMenu("Перемешать фразы")]
    public void ShufflePhrases()
    {
        InitializePhraseSystem();
        Debug.Log("Фразы перемешаны!");
    }

    [ContextMenu("Тест 5 фраз подряд")]
    public void TestFivePhrases()
    {
        Debug.Log("=== Тест 5 фраз ===");
        for (int i = 0; i < 5 && i < phrases.Count; i++)
        {
            SayPhraseInOrder();
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > maxDistance) return false;

        RaycastHit hit;
        Vector3 direction = player.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            return hit.transform == player;
        }

        return true;
    }

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        bool visible = CanSeePlayer();
        Gizmos.color = visible ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, player.position);

        Gizmos.color = new Color(0, 0, 1, 0.1f);
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}