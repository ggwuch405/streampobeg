using UnityEngine;
using UnityEngine.UI;

public class SprintSystem : MonoBehaviour
{
    [Header("Настройки спринта")]
    [SerializeField] private float sprintSpeed = 10f;      // Скорость при спринте
    [SerializeField] private float walkSpeed = 5f;         // Обычная скорость
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift; // Клавиша спринта

    [Header("Настройки выносливости")]
    [SerializeField] private float maxStamina = 100f;      // Макс. выносливость
    [SerializeField] private float staminaDrain = 20f;     // Скорость траты
    [SerializeField] private float staminaRegen = 15f;     // Скорость восстановления
    [SerializeField] private float regenDelay = 1f;        // Задержка перед восстановлением

    [Header("Ссылки")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Slider staminaBar;            // UI полоска выносливости
    [SerializeField] private Image staminaFill;            // Image полоски

    [Header("Визуальные эффекты")]
    [SerializeField] private GameObject sprintEffect;      // Эффект при спринте
    [SerializeField] private AudioSource breathSound;      // Звук дыхания

    private float currentStamina;
    private float regenTimer = 0f;
    private bool isSprinting = false;
    private float currentSpeed;

    void Start()
    {
        // Инициализация
        currentStamina = maxStamina;
        currentSpeed = walkSpeed;

        // Автопоиск компонентов
        if (controller == null)
            controller = GetComponent<CharacterController>();

        // Настройка UI
        UpdateStaminaUI();
    }

    void Update()
    {
        HandleSprint();
        UpdateStamina();

        // Визуальные эффекты
        HandleVisualEffects();
    }

    void HandleSprint()
    {
        // Проверяем можно ли спринтовать
        bool canSprint = Input.GetKey(sprintKey) &&
                        currentStamina > 0 &&
                        controller.velocity.magnitude > 0.1f;

        // Если нажата кнопка спринта и есть выносливость
        if (canSprint)
        {
            isSprinting = true;
            currentSpeed = sprintSpeed;

            // Тратим выносливость
            currentStamina -= staminaDrain * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            // Сбрасываем таймер восстановления
            regenTimer = 0f;
        }
        else
        {
            isSprinting = false;
            currentSpeed = walkSpeed;

            // Восстанавливаем выносливость если не спринтуем
            if (regenTimer >= regenDelay)
            {
                currentStamina += staminaRegen * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
            else
            {
                regenTimer += Time.deltaTime;
            }
        }
    }

    void UpdateStamina()
    {
        // Обновляем UI
        UpdateStaminaUI();

        // Меняем цвет полоски при низкой выносливости
        if (staminaFill != null)
        {
            if (currentStamina < 30f)
            {
                staminaFill.color = Color.red;
            }
            else if (currentStamina < 70f)
            {
                staminaFill.color = Color.yellow;
            }
            else
            {
                staminaFill.color = Color.green;
            }
        }
    }

    void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina / maxStamina;
        }
    }

    void HandleVisualEffects()
    {
        // Эффект спринта
        if (sprintEffect != null)
        {
            sprintEffect.SetActive(isSprinting);
        }

        // Звук дыхания при низкой выносливости
        if (breathSound != null)
        {
            if (currentStamina < 20f && !breathSound.isPlaying)
            {
                breathSound.Play();
            }
            else if (currentStamina > 50f && breathSound.isPlaying)
            {
                breathSound.Stop();
            }
        }
    }

    // Метод для получения текущей скорости (если нужно для других скриптов)
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }

    public float GetStaminaPercent()
    {
        return currentStamina / maxStamina;
    }

    // Метод для добавления выносливости (например, из зелий)
    public void AddStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }
}