using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour
{
    // Player Handling
	public float gravity = 20;
    public float speed = 8;
    public float acceleration = 30;
	public float jumpHeight = 12;

    private float currentSpeed;
    private float targetSpeed;
	private Vector2 amountToMove;

	private PlayerPhysics playerPhysics;

    void Start()
    {
		playerPhysics = GetComponent<PlayerPhysics>();
    }

    void Update()
    {
		if (playerPhysics.movementStopped)
		{
			targetSpeed = 0;
			currentSpeed = 0;
		}

		// Input
		targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		if (playerPhysics.grounded)
		{
			amountToMove.y = 0;
			// Jump
			if(Input.GetButtonDown("Jump"))
			{
				amountToMove.y = jumpHeight;
			}
		}

		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime);
    }

	// Increment speed towards target by acceleration
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
