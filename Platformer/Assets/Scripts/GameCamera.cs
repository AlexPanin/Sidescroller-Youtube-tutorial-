using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{
	private Transform target;
	private float trackSpeed = 12;

	public void SetTarget(Transform t)
	{
		target = t;
		transform.position = new Vector3(t.position.x, t.position.y, transform.position.z);
	}

	void LateUpdate()
	{
		if (target)
		{
			float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
			float y = IncrementTowards(transform.position.y, target.position.y, trackSpeed);
			transform.position = new Vector3(x, y, transform.position.z);
		}
	}

	// Increment speed towards target by acceleration
	private float IncrementTowards(float s, float target, float acceleration)
	{
		if (s == target)
			return s;
		else
		{
			float dir = Mathf.Sign(target - s);							// Must speed be increased or decreased to get closer to target
			s += acceleration * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target - s)) ? s : target;		// If speed has now passed target then return target, otherwise return speed
		}
	}
}
