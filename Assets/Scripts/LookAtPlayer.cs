using UnityEngine;
using Zenject.SpaceFighter;

public class LookAtPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Добавляем поворот на 90 градусов по оси X, чтобы "поднять" доску
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation * Quaternion.Euler(90, 0, 0), Time.deltaTime * rotationSpeed);
        }
    }
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
