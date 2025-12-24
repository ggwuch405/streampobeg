using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Здоровье")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("UI смерти")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Text deathText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;

    [Header("Эффекты")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject deathEffect;

    private bool isDead = false;
    private AudioSource audioSource;
    private Rigidbody rb;
    private Collider playerCollider;

    void Start()
    {
        currentHealth = maxHealth;

        // Скрываем панель смерти СРАЗУ
        if (deathPanel != null)
        {
            deathPanel.SetActive(false); // ВАЖНО!
        }

        currentHealth = maxHealth;

        // Получаем компоненты
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Настраиваем кнопки если есть панель смерти
        SetupDeathButtons();

        // Скрываем панель смерти
        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    void SetupDeathButtons()
    {
        if (deathPanel == null) return;

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);

        if (menuButton != null)
            menuButton.onClick.AddListener(GoToMenu);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Урон: {damage}. Здоровье: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Игрок умер!");

        // Останавливаем физику (чтобы не тряслась голова)
        StopPhysics();

        // Отключаем коллайдер
        if (playerCollider != null)
            playerCollider.enabled = false;

        // Отключаем все скрипты
        DisableAllScripts();

        // Эффекты
        PlayDeathEffects();

        // Показываем панель смерти
        ShowDeathPanel();

        // Включаем курсор
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void StopPhysics()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    void DisableAllScripts()
    {
        // Отключаем все скрипты на этом объекте кроме этого
        MonoBehaviour[] allScripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in allScripts)
        {
            if (script != this && script.enabled)
            {
                script.enabled = false;
            }
        }

        // Отключаем скрипты на детях (например на камере)
        MonoBehaviour[] childScripts = GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in childScripts)
        {
            if (script != this && script.enabled)
            {
                script.enabled = false;
            }
        }
    }

    void PlayDeathEffects()
    {
        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
    }

    void ShowDeathPanel()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);

            if (deathText != null)
                deathText.text = "ВЫ УМЕРЛИ";

            // Фокус на первую кнопку
            if (restartButton != null)
                restartButton.Select();
        }
    }

    // Методы для кнопок (оставляем те же названия)
    void RestartLevel()
    {
        Debug.Log("Рестарт уровня...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GoToMenu()
    {
        Debug.Log("Переход в меню...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void QuitGame()
    {
        Debug.Log("Выход из игры...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // Для отладки
    [ContextMenu("Умереть")]
    public void TestDeath()
    {
        TakeDamage(1000);
    }

    void OnDestroy()
    {
        // Отписываемся от кнопок
        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartLevel);

        if (menuButton != null)
            menuButton.onClick.RemoveListener(GoToMenu);

        if (quitButton != null)
            quitButton.onClick.RemoveListener(QuitGame);
    }
}