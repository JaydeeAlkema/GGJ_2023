using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField, Foldout("Movement Variables")] private float moveSpeed = 2f;
	[SerializeField, Foldout("Movement Variables")] private float jumpForce = 8f;
	[Space]
	[SerializeField, Foldout("Movement Variables")] private LayerMask groundedMask = default;

	[SerializeField, Foldout("References")] private SpriteRenderer spriteRenderer = default;
	[SerializeField, Foldout("References")] private Transform groundedCheckTransform = default;

	private Rigidbody2D rb2d = default;
	private Controlls_Player1 PlayerControls = default;
	private Animator animator;

	#region Unity Callbacks
	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		PlayerControls = new Controlls_Player1();
	}
	private void OnEnable()
	{
		PlayerControls.PlayerActions.GroundedMovement.performed += Move;
		PlayerControls.PlayerActions.GroundedMovement.Enable();

		PlayerControls.PlayerActions.Jump.performed += Jump;
		PlayerControls.PlayerActions.Jump.Enable();
	}
	private void OnDisable()
	{
		PlayerControls.PlayerActions.GroundedMovement.Disable();
		PlayerControls.PlayerActions.Jump.Disable();
	}
	private void Update()
	{
		//SetAnimatorStates();
	}
	#endregion

	#region Input Action Callbacks
	private void Move(InputAction.CallbackContext context)
	{
		Vector2 inputAxis = context.ReadValue<Vector2>();
		rb2d.velocity = new Vector2(inputAxis.x * moveSpeed, rb2d.velocity.y);

		FlipSprite(inputAxis);
	}
	private void Jump(InputAction.CallbackContext context)
	{
		if (!IsGrounded()) return;
		rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
	}
	#endregion

	private void FlipSprite(Vector2 inputAxis)
	{
		switch (inputAxis.x)
		{
			case 1:
				spriteRenderer.flipX = false;
				break;
			case -1:
				spriteRenderer.flipX = true;
				break;
		}
	}
	private bool IsGrounded()
	{
		if (Physics2D.Raycast(groundedCheckTransform.position, Vector2.down, 0.5f, groundedMask))
		{
			return true;
		}
		{
			return false;
		}
	}
	private bool IsMoving()
	{
		if (rb2d.velocity.x != 0) return true;
		else return false;
	}
	private bool IsJumping()
	{
		if (rb2d.velocity.y != 0) return true;
		else return false;
	}
	private void SetAnimatorStates()
	{
		animator.SetBool("Moving", IsMoving());
		animator.SetBool("Jumping", IsJumping());
		animator.SetBool("Grounded", IsGrounded());
		animator.SetFloat("Velocity Y", rb2d.velocity.y);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawRay(groundedCheckTransform.position, Vector2.down * 0.5f);
	}
}
