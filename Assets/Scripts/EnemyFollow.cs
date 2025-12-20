using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;

    [Header("Настройки")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 rotationOffset = new Vector3(90, 0, 0);
    [SerializeField] private float stopDistance = 1.2f; // Расстояние, на котором он замрет

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.updateRotation = false; // Мы крутим сами
        agent.stoppingDistance = stopDistance;
    }

    private void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance + 0.1f) // Добавляем небольшой зазор (буфер)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            // Если подошел близко — ПОЛНАЯ остановка, чтобы не было тряски
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        RotateTowardsPlayer();
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(rotationOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}