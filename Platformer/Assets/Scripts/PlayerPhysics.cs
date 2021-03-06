﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour
{
	public LayerMask collisionMask;

	private BoxCollider collider;
	private Vector3 s;
	private Vector3 c;

	private Vector3 originalSize;
	private Vector3 originalCenter;
	private float colliderScale;

	private int collisionDivisionX = 3;
	private int collisionDivisionY = 10;

	private float skin = .005f;

	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool movementStopped;
	[HideInInspector]
	public bool canWallHold;

	private Transform platform;
	private Vector3 platformPositionOld;
	private Vector3 deltaPlatformPos;

	Ray ray;
	RaycastHit hit;

	void Start()
	{
		collider = GetComponent<BoxCollider>();
		colliderScale = transform.localScale.x;

		originalSize = collider.size;
		originalCenter = collider.center;
		SetCollider(originalSize, originalCenter);
	}

	public void Move(Vector2 moveAmount, float moveDirX)
	{
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 position = transform.position;

		if(platform)
			deltaPlatformPos = platform.position - platformPositionOld;
		else
			deltaPlatformPos = Vector3.zero;

		#region Vertical Collisions
		// Check collisions above and below
		grounded = false;
		for (int i = 0; i < collisionDivisionX; i++)
		{
			float dir = Mathf.Sign(deltaY);
			float x = (position.x + c.x - s.x / 2) + s.x / (collisionDivisionX - 1) * i;	// Left, center and then rightmost point of collider
			float y = position.y + c.y + s.y / 2 * dir;										// Bottom of collider

			ray = new Ray(new Vector2(x, y), new Vector2(0, dir));
			Debug.DrawRay(ray.origin, ray.direction);
			if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + skin, collisionMask))
			{
				platform = hit.transform;
				platformPositionOld = platform.position;

				// Get distance between player and ground
				float distance = Vector3.Distance(ray.origin, hit.point);

				// Stop player's downward movement after comming within skin width of a collider
				if (distance > skin)
					deltaY = distance * dir - skin * dir;
				else
					deltaY = 0;

				grounded = true;
				break;
			}
			else
				platform = null;
		}
		#endregion

		#region Horizontal Collisions
		// Check collisions left and right
		movementStopped = false;
		canWallHold = false;

		if (deltaX != 0)
		{
			for (int i = 0; i < collisionDivisionY; i++)
			{
				float dir = Mathf.Sign(deltaX);
				float x = position.x + c.x + s.x / 2 * dir;									// Left, center and then rightmost point of collider
				float y = position.y + c.y - s.y / 2 + s.y / (collisionDivisionY - 1) * i;	// Bottom of collider
			
				ray = new Ray(new Vector2(x, y), new Vector2(dir, 0));
				Debug.DrawRay(ray.origin, ray.direction);
				if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + skin, collisionMask))
				{
					if(hit.collider.tag == "Wall Jump")
					{
						if(Mathf.Sign(deltaX) == Mathf.Sign(moveDirX) && moveDirX != 0);
						canWallHold = true;
					}

					// Get distance between player and ground
					float distance = Vector3.Distance(ray.origin, hit.point);
				
					// Stop player's downward movement after comming within skin width of a collider
					if (distance > skin)
						deltaX = distance * dir - skin * dir;
					else
						deltaX = 0;

					movementStopped = true;
					break;
				}
			}
		}
		#endregion

		// Check collisions in the direction of movement
		if (!grounded && !movementStopped)
		{
			Vector3 playerDir = new Vector3(deltaX, deltaY);
			Vector3 o = new Vector3(position.x + c.x + s.x / 2 * Mathf.Sign(deltaX), 
		                        	position.y + c.y + s.y / 2 * Mathf.Sign(deltaY));
			ray = new Ray(o, playerDir.normalized);
			Debug.DrawRay(ray.origin, ray.direction);
			if (Physics.Raycast(ray, Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY), collisionMask))
			{
				grounded = true;
				deltaY = 0;
			}
		}

		Vector2 finalTransform = new Vector2(deltaX + deltaPlatformPos.x, deltaY);

		transform.Translate(finalTransform, Space.World);
	}

	public void SetCollider(Vector3 size, Vector3 center)
	{
		collider.size = size;
		collider.center = center;

		s = size * colliderScale;
		c = center * colliderScale;
	}

	public void ResetCollider()
	{
		SetCollider(originalSize, originalCenter);
	}
}
