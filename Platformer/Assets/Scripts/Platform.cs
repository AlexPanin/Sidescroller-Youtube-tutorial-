using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
	public float speed = 2;
	public bool bounce = true;
	public Vector3 start;
	public Vector3 finish;

	void Update()
	{
		transform.Translate(Vector3.right * speed * Time.deltaTime);
	}


	// TODO: Move towards target
	private float IncrementTowards(float s, float target, float acceleration)
	{
		if (s == target)
			return s;
		else
		{
			float dir = Mathf.Sign(target - s);									// Must speed be increased or decreased to get closer to target
			s += acceleration * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target - s)) ? s : target;	// If speed has now passed target then return target, otherwise return speed
		}
	}
}
