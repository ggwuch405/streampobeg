using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float rotationSpeed = 5f;

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null) playerTransform = player.transform;
        else Debug.LogWarning("FacePlayer: Игрок с тегом '" + playerTag + "' не найден в сцене.");
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Направление на игрока в горизонтальной плоскости
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer. y= 0f;

        if (directionToPlayer.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            // Плавный поворот вокруг оси Y
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Удаляем возможный наклон камеры/плоскости по X/Z, оставляя только вращение вокруг Y
            Vector3 euler = transform.eulerAngles;
            euler.x = 90f;
            euler.z = 0f;
            transform.eulerAngles = euler;
        }
    }
}