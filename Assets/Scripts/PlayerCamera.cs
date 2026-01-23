using UnityEngine;

public class PlayerCamerasas : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform playerCamera;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing!");
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

       
        Vector3 moveDirection = transform.TransformDirection(new Vector3(moveX, 0f, moveZ));

      
        Vector3 newVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
        rb.linearVelocity = newVelocity;
    }
}