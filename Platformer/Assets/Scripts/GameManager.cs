using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject player;
	public Vector3 spawnPoint = Vector3.zero;
	private GameObject currentPlayer;
	private GameCamera cam;

	void Start()
	{
		cam = GetComponent<GameCamera>();
		SpawnPlayer(spawnPoint);
	}

	// Spawn Player
	private void SpawnPlayer(Vector3 spawnPos)
	{
		currentPlayer = Instantiate(player, spawnPos, Quaternion.identity) as GameObject;
		cam.SetTarget(currentPlayer.transform);
	}

	private void Update()
	{
		if(!currentPlayer)
		{
			if(Input.GetButtonDown("Respawn"))
				SpawnPlayer(spawnPoint);
		}
	}
}
