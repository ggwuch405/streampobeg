using UnityEngine;
using Zenject.SpaceFighter;

public class LookAtPlayer : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] float rotationSpeed;
	void Update()
	{
		Vector3 direction = (player.transform.position - transform.position).normalized;
		direction.y = 0;
		
		if (direction != Vector3.zero)
		{
			Quaternion lookRotation = Quaternion.LookRotation(direction);

			
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation * Quaternion.Euler(90, 0, 0), Time.deltaTime * rotationSpeed);
		}
	}
}

    