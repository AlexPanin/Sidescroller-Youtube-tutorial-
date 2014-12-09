using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour
{
    // Player Handling
	public float gravity = 20;
    public float walkSpeed = 8;
	public float runSpeed = 12;
    public float acceleration = 30;
	public float jumpHeight = 12;

	private float animationSpeed;
    private float currentSpeed;
    private float targetSpeed;
	private Vector2 amountToMove;

	private PlayerPhysics playerPhysics;
	private Animator animator;

    void Start()
    {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
    }

    void Update()
    {
		// Reset acceleration upon collision
		if (playerPhysics.movementStopped)
		{
			targetSpeed = 0;
			currentSpeed = 0;
		}
		
		// If player is touching the ground
		if (playerPhysics.grounded)
		{
			amountToMove.y = 0;
			// Jump
			if(Input.GetButtonDown("Jump"))
			{
				amountToMove.y = jumpHeight;
			}
		}

		animationSpeed = IncrementTowards(animationSpeed, Mathf.Abs(targetSpeed), acceleration);
		animator.SetFloat("Speed", animationSpeed);

		// Input
		float speed = (Input.GetButton("Run")) ? runSpeed : walkSpeed;
		targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		// Set amount to move
		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime);

		// Face in the right direction
		float moveDir = Input.GetAxisRaw("Horizontal");
		if (moveDir != 0)
			transform.eulerAngles = (moveDir > 0) ? Vector3.up * 180 : Vector3.zero;
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
