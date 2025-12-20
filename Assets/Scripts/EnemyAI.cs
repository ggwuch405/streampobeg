using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 10f;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        // Отключаем встроенный поворот, чтобы он не конфликтовал с нашим
        agent.updateRotation = false;
    }

    void Update()
    {
        if (player != null && agent.isOnNavMesh)
        {
            // 1. Говорим агенту, куда идти
            agent.SetDestination(player.position);

            // 2. Вызываем метод поворота, который ты спрашивал
            LookAtPlayer();
        }
    }

    // Вот сюда мы вставляем сам метод (вне Update, но внутри класса)
    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Поворот на 90 градусов, чтобы твоя доска стояла вертикально
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation * Quaternion.Euler(90, 0, 0), Time.deltaTime * rotationSpeed);
        }
    }
}