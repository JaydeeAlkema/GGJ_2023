using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[SerializeField] private TreeManager treeManager;

	[SerializeField] private Transform player1SpawnPoint = default;
	[SerializeField] private Transform player2SpawnPoint = default;

	[SerializeField] private InputAction joinAction;
	[SerializeField] private InputAction leaveAction;

	public System.Action<PlayerInput> playerJoinedGame;
	public System.Action<PlayerInput> playerLeftGame;

	private List<PlayerInput> players = new List<PlayerInput>();

	private void Awake()
	{
		if (!instance || instance != this)
		{
			Destroy(instance);
			instance = this;
		}

		joinAction.Enable();
		joinAction.performed += context => OnJoinAction(context);

		leaveAction.Enable();
		leaveAction.performed += context => OnLeaveAction(context);
	}

	private void Start()
	{
		PlayerInputManager.instance.JoinPlayer(0, -1, null);
		for (int i = 0; i < players.Count; i++)
		{
			treeManager.AddPlayerTransform(players[i].transform);
		}
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

	void OnJoinAction(InputAction.CallbackContext context)
	{
		PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
	}
	void OnLeaveAction(InputAction.CallbackContext context)
	{

	}
}
