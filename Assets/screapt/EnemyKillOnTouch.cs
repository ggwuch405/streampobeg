using UnityEngine;

public class EnemyKillOnTouch : MonoBehaviour
{
    [Header("Настройки убийства")]
    [SerializeField] private int damage = 100; // Урон за касание
    [SerializeField] private float killCooldown = 1f; // Задержка между уроном

    [Header("Эффекты")]
    [SerializeField] private AudioClip touchSound;
    [SerializeField] private GameObject touchEffect;

    private float lastDamageTime = 0f;

    void OnTriggerEnter(Collider other)
    {
        TryKillPlayer(other.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        TryKillPlayer(collision.gameObject);
    }

    void TryKillPlayer(GameObject otherObject)
    {
        // Проверяем что это игрок
        if (otherObject.CompareTag("Player"))
        {
            // Проверяем кд
            if (Time.time - lastDamageTime < killCooldown) return;

            // Наносим урон
            PlayerHealth playerHealth = otherObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Враг коснулся игрока! Урон: {damage}");
            }
            else
            {
                // Если у игрока нет системы здоровья - просто убиваем
                Debug.Log("Игрок убит врагом!");
                Destroy(otherObject); // Или otherObject.SetActive(false);
            }

            // Эффекты
            PlayTouchEffects();

            lastDamageTime = Time.time;
        }
    }

    void PlayTouchEffects()
    {
        // Звук
        if (touchSound != null)
        {
            AudioSource.PlayClipAtPoint(touchSound, transform.position);
        }

        // Эффект частиц
        if (touchEffect != null)
        {
            Instantiate(touchEffect, transform.position, Quaternion.identity);
        }
    }

    // Метод для отключения убийства (например если враг умер)
    public void DisableKilling()
    {
        enabled = false;
        Debug.Log("Враг больше не может убивать");
    }
}