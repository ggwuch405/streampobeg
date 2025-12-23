using UnityEngine;

public class PanelEnemy3D : MonoBehaviour
{
    [Header("Цель")]
    [SerializeField] private Transform player;
    [SerializeField] private string playerTag = "Player";

    [Header("Настройки обнаружения")]
    [SerializeField] private float viewDistance = 15f;      // Дальность обзора
    [SerializeField] private float viewAngle = 90f;         // Угол обзора (градусы)
    [SerializeField] private LayerMask obstacleLayers;      // Слои препятствий

    [Header("Звук обнаружения")]
    [SerializeField] private AudioClip detectionSound;
    [SerializeField] private float soundVolume = 0.7f;

    [Header("Визуальные эффекты")]
    [SerializeField] private Material normalMaterial;       // Обычный материал
    [SerializeField] private Material detectedMaterial;     // Материал при обнаружении
    [SerializeField] private Light alertLight;              // Свет при обнаружении (опционально)

    private AudioSource audioSource;
    private Renderer panelRenderer;
    private bool canPlaySound = true;
    private bool playerVisible = false;

    void Start()
    {
        // Автопоиск игрока
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj != null) player = playerObj.transform;
        }

        // Получаем компоненты
        panelRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        // Создаем AudioSource если нет
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; // 3D звук
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.maxDistance = 20f;
        }

        // Настройка света
        if (alertLight != null)
        {
            alertLight.enabled = false;
        }

        Debug.Log($"Панель {gameObject.name} инициализирована. Игрок: {player != null}");
    }

    void Update()
    {
        if (player == null) return;

        // Проверяем видимость игрока
        bool isVisibleNow = IsPlayerVisible();

        // Если игрок стал видимым (был невидим - стал видим)
        if (isVisibleNow && !playerVisible)
        {
            OnPlayerDetected();
        }
        // Если игрок стал невидимым (был видим - стал невидим)
        else if (!isVisibleNow && playerVisible)
        {
            OnPlayerLost();
        }

        playerVisible = isVisibleNow;

        // Визуальная обратная связь
        UpdateVisuals(isVisibleNow);
    }

    bool IsPlayerVisible()
    {
        // 1. Проверяем расстояние
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > viewDistance)
        {
            return false; // Слишком далеко
        }

        // 2. Проверяем находится ли игрок перед панелью (в угле обзора)
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle > viewAngle / 2)
        {
            return false; // Вне угла обзора
        }

        // 3. Проверяем нет ли стен между панелью и игроком
        RaycastHit hit;
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = player.position - rayOrigin;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, viewDistance, obstacleLayers))
        {
            // Если луч попал в игрока - видно
            if (hit.transform == player || hit.transform.IsChildOf(player))
            {
                return true;
            }
            else
            {
                // Если луч попал во что-то другое (стену) - не видно
                return false;
            }
        }

        // Если луч ничего не hit (маловероятно, но на всякий случай)
        return true;
    }

    void OnPlayerDetected()
    {
        Debug.Log($"Панель {gameObject.name} обнаружила игрока!");

        // Проигрываем звук если можно
        if (canPlaySound && detectionSound != null)
        {
            audioSource.PlayOneShot(detectionSound, soundVolume);
            canPlaySound = false;

            // Через 1 секунду можно снова проиграть звук
            Invoke(nameof(ResetSound), 1f);
        }
    }

    void OnPlayerLost()
    {
        Debug.Log($"Игрок скрылся от панели {gameObject.name}");
        // Можно добавить эффект когда игрок скрывается
    }

    void UpdateVisuals(bool isDetected)
    {
        // Меняем материал
        if (panelRenderer != null)
        {
            if (isDetected && detectedMaterial != null)
            {
                panelRenderer.material = detectedMaterial;
            }
            else if (normalMaterial != null)
            {
                panelRenderer.material = normalMaterial;
            }
        }

        // Включаем/выключаем свет
        if (alertLight != null)
        {
            alertLight.enabled = isDetected;
        }
    }

    void ResetSound()
    {
        canPlaySound = true;
    }

    // Метод для ручной проверки (можно вызывать из других скриптов)
    public bool CheckPlayerVisibility()
    {
        return IsPlayerVisible();
    }

    // Визуализация в редакторе
    void OnDrawGizmosSelected()
    {
        if (player != null && playerVisible)
        {
            // Красная линия когда видит
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
        else if (player != null)
        {
            // Желтая линия когда не видит
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, player.position);
        }

        // Рисуем зону обнаружения
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // Рисуем угол обзора
        float halfAngle = viewAngle / 2;
        Vector3 leftDirection = Quaternion.Euler(0, -halfAngle, 0) * transform.forward * viewDistance;
        Vector3 rightDirection = Quaternion.Euler(0, halfAngle, 0) * transform.forward * viewDistance;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
        Gizmos.DrawRay(transform.position, leftDirection);
        Gizmos.DrawRay(transform.position, rightDirection);

        // Соединяем края
        Gizmos.DrawLine(transform.position + leftDirection, transform.position + rightDirection);
    }
}