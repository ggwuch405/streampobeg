using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;

    [Header("Выносливость")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrain = 20f; // Трата в секунду
    [SerializeField] private float staminaRegen = 15f; // Восстановление в секунду
    [SerializeField] private Slider staminaBar; // Опционально - перетащи Slider UI

    [Header("Камера")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;

    private Rigidbody rb;
    private float currentStamina;
    private float rotationX = 0;
    private bool isSprinting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing!");
            return;
        }

        // Настройка Rigidbody
        rb.freezeRotation = true;

        // Инициализация выносливости
        currentStamina = maxStamina;

        // Настройка курсора
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Настройка UI выносливости
        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
        }
    }

    void Update()
    {
        // Обработка вращения камеры
        HandleCameraRotation();

        // Обработка спринта
        HandleSprint();

        // Обновление UI выносливости
        UpdateStaminaUI();
    }

    void FixedUpdate()
    {
        // Движение в FixedUpdate (для физики)
        HandleMovement();
    }

    void HandleCameraRotation()
    {
        if (playerCamera == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Вращение тела игрока по горизонтали
        transform.Rotate(Vector3.up * mouseX);

        // Вращение камеры по вертикали
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    void HandleMovement()
    {
        if (rb == null) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Направление движения относительно взгляда камеры
        Vector3 moveDirection = transform.forward * moveZ + transform.right * moveX;
        moveDirection.Normalize();

        // Определяем текущую скорость (с учетом спринта)
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Целевая скорость
        Vector3 targetVelocity = moveDirection * currentSpeed;

        // Сохраняем вертикальную скорость (для гравитации)
        targetVelocity.y = rb.linearVelocity.y;

        // Плавное изменение скорости
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * 10f);
    }

    void HandleSprint()
    {
        // Проверяем нажата ли кнопка спринта (LeftShift)
        bool sprintKeyPressed = Input.GetKey(KeyCode.LeftShift);

        // Проверяем движется ли игрок
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        // Можем спринтовать если:
        // 1. Нажата кнопка спринта
        // 2. Есть выносливость
        // 3. Игрок движется
        if (sprintKeyPressed && currentStamina > 0 && isMoving)
        {
            isSprinting = true;
            // Тратим выносливость
            currentStamina -= staminaDrain * Time.deltaTime;
        }
        else
        {
            isSprinting = false;
            // Восстанавливаем выносливость
            currentStamina += staminaRegen * Time.deltaTime;
        }

        // Ограничиваем выносливость между 0 и maxStamina
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;

            // Меняем цвет полоски в зависимости от выносливости
            Image fillImage = staminaBar.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                if (currentStamina < 30f)
                    fillImage.color = Color.red;     // Мало выносливости
                else if (currentStamina < 70f)
                    fillImage.color = Color.yellow;  // Средне
                else
                    fillImage.color = Color.green;   // Много
            }
        }
    }

    // Метод для добавления выносливости (можно вызвать из других скриптов)
    public void AddStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    // Метод для проверки спринта (можно использовать в других скриптах)
    public bool IsSprinting()
    {
        return isSprinting;
    }

    // Метод для получения текущей скорости
    public float GetCurrentSpeed()
    {
        return isSprinting ? sprintSpeed : walkSpeed;
    }
}