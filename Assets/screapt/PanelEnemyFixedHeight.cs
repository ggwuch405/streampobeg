using UnityEngine;
using UnityEngine.AI;

public class PanelEnemyFixedHeight : MonoBehaviour
{
    [Header("Патрулирование")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float waitTimeAtPoint = 2f;

    [Header("Преследование")]
    public float chaseSpeed = 4f;
    public float sightRange = 10f;

    [Header("Высота")]
    public float fixedHeight = 1f; // Фиксированная высота

    [Header("Ссылки")]
    public Transform player;
    public LayerMask obstacleMask;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private bool isChasing = false;
    private Vector3 lastValidPosition;

    void Start()
    {
        // Сохраняем начальную позицию с правильной высотой
        transform.position = new Vector3(
            transform.position.x,
            fixedHeight,
            transform.position.z
        );
        lastValidPosition = transform.position;

        // Автопоиск игрока
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        // Настраиваем NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
        }

        // ОТКЛЮЧАЕМ ВЕРТИКАЛЬНУЮ КОРРЕКЦИЮ NavMeshAgent
        agent.updatePosition = false; // САМОЕ ВАЖНОЕ!
        agent.updateRotation = true;
        agent.updateUpAxis = false;

        // Настройки для панели
        agent.baseOffset = fixedHeight;
        agent.height = 0.1f;
        agent.radius = 1f;
        agent.speed = patrolSpeed;
        agent.stoppingDistance = 0.5f;

        // Отключаем гравитацию
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // Начинаем патрулирование
        if (patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }

        Debug.Log($"Панель-враг на высоте: {fixedHeight}");
    }

    void Update()
    {
        // ВАЖНО: Вручную обновляем позицию, сохраняя высоту
        if (agent.nextPosition != Vector3.zero)
        {
            // Берем X и Z от agent, а Y оставляем фиксированным
            Vector3 targetPosition = new Vector3(
                agent.nextPosition.x,
                fixedHeight,
                agent.nextPosition.z
            );

            transform.position = targetPosition;
            lastValidPosition = transform.position;
        }

        if (player == null) return;

        // Проверяем видимость игрока
        bool canSeePlayer = CanSeePlayer();

        if (canSeePlayer && !isChasing)
        {
            StartChasing();
        }
        else if (!canSeePlayer && isChasing)
        {
            StopChasing();
        }

        // Логика движения
        if (isChasing)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            HandlePatrol();
        }
    }

    void LateUpdate()
    {
        // Дополнительная фиксация высоты
        if (Mathf.Abs(transform.position.y - fixedHeight) > 0.01f)
        {
            transform.position = new Vector3(
                transform.position.x,
                fixedHeight,
                transform.position.z
            );
        }
    }

    bool CanSeePlayer()
    {
        float distanceToPlayer = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(player.position.x, 0, player.position.z)
        );

        if (distanceToPlayer > sightRange) return false;

        RaycastHit hit;
        Vector3 direction = player.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out hit, distanceToPlayer, obstacleMask))
        {
            return hit.transform == player;
        }

        return true;
    }

    void StartChasing()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
        Debug.Log("Враг увидел игрока!");
    }

    void StopChasing()
    {
        isChasing = false;
        agent.speed = patrolSpeed;

        if (patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }
    }

    void HandlePatrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartWaiting();
        }

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPoint)
            {
                StopWaiting();
                GoToNextPatrolPoint();
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        // Корректируем высоту точек патрулирования
        Vector3 targetPoint = patrolPoints[currentPatrolIndex].position;
        targetPoint.y = fixedHeight;

        agent.SetDestination(targetPoint);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void StartWaiting()
    {
        isWaiting = true;
        waitTimer = 0f;
        agent.isStopped = true;
    }

    void StopWaiting()
    {
        isWaiting = false;
        agent.isStopped = false;
    }

    // Для отладки
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * fixedHeight);
    }
}