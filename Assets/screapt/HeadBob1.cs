using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField] private Transform cameraTransform;     // Камера игрока
    [SerializeField] private PlayerCamera playerMovement;   // Ссылка на твой скрипт движения

    [Header("Покачивание при ходьбе")]
    [SerializeField] private float walkBobSpeed = 10f;      // Скорость покачивания
    [SerializeField] private float walkBobAmount = 0.05f;   // Сила покачивания

    [Header("Покачивание при беге")]
    [SerializeField] private float runBobSpeed = 15f;       // Скорость при беге
    [SerializeField] private float runBobAmount = 0.08f;    // Сила при беге

    [Header("Плавность")]
    [SerializeField] private float transitionSpeed = 10f;   // Скорость смены анимации

    private Vector3 cameraOriginalPosition;  // Исходная позиция камеры
    private float bobTimer = 0f;             // Таймер для анимации
    private bool isMoving = false;

    void Start()
    {
        // Автопоиск камеры если не назначена
        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        // Автопоиск скрипта движения
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerCamera>();
        }

        // Сохраняем исходную позицию камеры
        if (cameraTransform != null)
        {
            cameraOriginalPosition = cameraTransform.localPosition;
        }
        else
        {
            Debug.LogError("Camera Transform not assigned!");
            enabled = false;
        }
    }

    void Update()
    {
        HandleHeadBob();
    }

    void HandleHeadBob()
    {
        if (cameraTransform == null || playerMovement == null) return;

        // Проверяем движется ли игрок (через Rigidbody или ввод)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        isMoving = (Mathf.Abs(moveHorizontal) > 0.1f || Mathf.Abs(moveVertical) > 0.1f);

        // Если игрок движется и находится на земле
        if (isMoving)
        {
            // Определяем параметры в зависимости от спринта
            float currentBobSpeed = playerMovement.IsSprinting() ? runBobSpeed : walkBobSpeed;
            float currentBobAmount = playerMovement.IsSprinting() ? runBobAmount : walkBobAmount;

            // Увеличиваем таймер
            bobTimer += Time.deltaTime * currentBobSpeed;

            // Вычисляем новую позицию камеры
            Vector3 newPosition = cameraOriginalPosition;

            // Вертикальное покачивание (основное)
            newPosition.y += Mathf.Sin(bobTimer) * currentBobAmount;

            // Легкое горизонтальное покачивание
            newPosition.x += Mathf.Cos(bobTimer * 0.5f) * currentBobAmount * 0.3f;

            // Плавно применяем новую позицию
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                newPosition,
                Time.deltaTime * transitionSpeed
            );
        }
        else
        {
            // Если не движемся - плавно возвращаем камеру в исходное положение
            bobTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                cameraOriginalPosition,
                Time.deltaTime * transitionSpeed
            );
        }
    }

    // Для отладки - рисуем линию в редакторе
    void OnDrawGizmosSelected()
    {
        if (cameraTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.up * 0.5f);
        }
    }
}