using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField, Foldout("Movement Variables")] private float moveSpeed = 2f;
	[SerializeField, Foldout("Movement Variables")] private float jumpForce = 8f;
	[SerializeField, Foldout("Movement Variables")] private float defaultGravityScale = 5f;
	[SerializeField, Foldout("Movement Variables")] private float fallGravityScale = 9f;
	[Space]
	[SerializeField, Foldout("Movement Variables")] private LayerMask groundedMask = default;
	[SerializeField, Foldout("Movement Variables")] private float groundDetectionDistance = 0.35f;

	[SerializeField, Foldout("References")] private SpriteRenderer spriteRenderer = default;
	[SerializeField, Foldout("References")] private Transform groundedCheckTransform = default;

	private Rigidbody2D rb2d = default;
	private PlayerControls PlayerControls = default;
	private Animator animator;

	#region Unity Callbacks
	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		PlayerControls = new PlayerControls();

		rb2d.gravityScale = defaultGravityScale;
	}
	private void OnEnable()
	{
		PlayerControls.Player.GroundedMovement.performed += Move;
		PlayerControls.Player.GroundedMovement.Enable();

		PlayerControls.Player.Jump.performed += Jump;
		PlayerControls.Player.Jump.Enable();
	}
	private void OnDisable()
	{
		PlayerControls.Player.GroundedMovement.Disable();
		PlayerControls.Player.Jump.Disable();
	}
	private void Update()
	{
		EvaluateJump();
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
		rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
		if (Physics2D.Raycast(groundedCheckTransform.position, Vector2.down, groundDetectionDistance, groundedMask))
		{
			rb2d.gravityScale = defaultGravityScale;
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
	private void EvaluateJump()
	{
		if (IsGrounded()) return;
		if (rb2d.velocity.y < 0)
		{
			rb2d.gravityScale = fallGravityScale;
		}
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
		Gizmos.DrawRay(groundedCheckTransform.position, Vector2.down * groundDetectionDistance);
	}
}
