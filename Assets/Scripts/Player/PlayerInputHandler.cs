using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
	[SerializeField] private GameObject playerPrefab = null;
	[SerializeField] private PlayerController playerController = null;
	[Space]
	[SerializeField] private Vector3 spawnPos = new Vector3(0, 0, 0);

	private void Awake()
	{
		if (playerPrefab != null)
		{
			playerController = Instantiate(playerPrefab, spawnPos, transform.rotation).GetComponent<PlayerController>();
			transform.parent = playerController.transform;
		}
	}

	public void OnGroundedMovement(InputAction.CallbackContext context)
	{
		playerController.OnGroundedMovement(context);
	}
	public void OnJump(InputAction.CallbackContext context)
	{
		playerController.OnJump(context);
	}
}
