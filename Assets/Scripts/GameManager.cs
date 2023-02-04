using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;

	[SerializeField] private Transform player1SpawnPoint = default;
	[SerializeField] private Transform player2SpawnPoint = default;

	[SerializeField] private System.Action<PlayerInput> playerJoinedGame;
	[SerializeField] private System.Action<PlayerInput> playerLeftGame;

	private List<PlayerInput> players = new List<PlayerInput>();

	private void Awake()
	{
		if (!instance || instance != this)
		{
			Destroy(instance);
			instance = this;
		}
	}

	private void Start()
	{
		PlayerInputManager.instance.JoinPlayer(0, -1, null);
	}

	void OnPlayerJoined(PlayerInput playerInput)
	{
		players.Add(playerInput);

		if (playerJoinedGame != null)
		{
			playerJoinedGame(playerInput);
		}
	}

	void OnPlayerLeft(PlayerInput playerInput)
	{
		players.Remove(playerInput);
	}
}
