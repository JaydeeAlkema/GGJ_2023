using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TreeManager treeManager;

    [SerializeField] private Transform player1SpawnPoint = default;
    [SerializeField] private Transform player2SpawnPoint = default;

    [SerializeField] private System.Action<PlayerInput> playerJoinedGame;
    [SerializeField] private System.Action<PlayerInput> playerLeftGame;

    private List<PlayerInput> players = new List<PlayerInput>();

    public List<PlayerInput> Players { get => players; set => players = value; }

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
        for (int i = 0; i < players.Count; i++)
        { 
            treeManager.AddPlayerTransform(players[i].transform);
        }
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        Players.Add(playerInput);

        if (playerJoinedGame != null)
        {
            playerJoinedGame(playerInput);
        }
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        Players.Remove(playerInput);
    }
}
