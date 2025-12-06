using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private Rigidbody rb;

	[Header("Camera")]
	[SerializeField] private Transform cameraTransform;
	[SerializeField] private float cameraDistance = 5f;
	[SerializeField] private float cameraHeight = 3f;

	private Vector3 moveInput;

	private void Update()
	{
		float h = Input.GetAxisRaw("Horizontal");  
		float v = Input.GetAxisRaw("Vertical");     

		moveInput = new Vector3(h, 0f, v).normalized;
	}

	private void FixedUpdate()
	{
		Vector3 move = moveInput * moveSpeed;
		Vector3 newPos = rb.position + move * Time.fixedDeltaTime;

		rb.MovePosition(newPos);
	}

	private void LateUpdate()
	{
		if (cameraTransform == null) return;

		
		Vector3 desiredPos = rb.position -
							 transform.forward * cameraDistance +
							 Vector3.up * cameraHeight;

		cameraTransform.position = Vector3.Lerp(
			cameraTransform.position,
			desiredPos,
			Time.deltaTime * 5f
		);

		cameraTransform.LookAt(rb.position + Vector3.up * 1.5f);
	}
}
