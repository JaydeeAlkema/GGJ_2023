using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[SerializeField] private TreeManager treeManager;
	[SerializeField] private UIManager uiManager;

    [SerializeField] private List<Transform> spawnPoints = default;

	[SerializeField] private InputAction joinAction;
	[SerializeField] private InputAction leaveAction;

	public System.Action<PlayerInput> playerJoinedGame;
	public System.Action<PlayerInput> playerLeftGame;

	[SerializeField] private List<PlayerInput> players = new List<PlayerInput>();
	[SerializeField] private List<PlayerController> controllers = new List<PlayerController>();

	public UIManager UiManager { get => uiManager; set => uiManager = value; }
    public TreeManager TreeManager { get => treeManager; set => treeManager = value; }

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
		for (int i = 0; i < players.Count; i++)
		{
			treeManager.AddPlayerTransform(players[i].transform);
		}
	}

	//Event in Unity
	void OnPlayerJoined(PlayerInput playerInput)
	{
		players.Add(playerInput);
		if (playerJoinedGame != null)
		{
			playerJoinedGame(playerInput);
		}

		controllers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).ToList();
	}


	void OnJoinAction(InputAction.CallbackContext context)
	{
		PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
	}

	void OnLeaveAction(InputAction.CallbackContext context)
	{

    }

    public void RespawnPlayers(GameObject playerObject)
    {
        for (int i = 0; i < controllers.Count; i++)
        {
            if (playerObject == controllers[i].gameObject)
            {
                playerObject.transform.position = spawnPoints[i].position;
            }
        }
    }

    [ContextMenu("Reset the game")]
    public void ResetGame()
    {
        SceneManager.LoadScene("TestScene");
    }
}
