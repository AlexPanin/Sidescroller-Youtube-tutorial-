using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject player;
	public Vector3 checkpoint;
	private GameObject currentPlayer;
	private GameCamera cam;

	public static int levelCount = Application.levelCount;

	void Start()
	{
		cam = GetComponent<GameCamera>();

		if(GameObject.FindGameObjectWithTag("Spawn"))
			checkpoint = GameObject.FindGameObjectWithTag("Spawn").transform.position;

		SpawnPlayer(checkpoint);
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
				SpawnPlayer(checkpoint);
		}
	}

	public void SetCheckpoint(Vector3 cp)
	{
		checkpoint = cp;
	}

	public void EndLevel()
	{
		if(Application.loadedLevel + 1 < levelCount)
		{
			Application.LoadLevel("Level " + (Application.loadedLevel + 2));
			Debug.Log("Loading - " + "Level " + (Application.loadedLevel + 2));
		}
		else
			Debug.Log("Game Over");
	}
}
