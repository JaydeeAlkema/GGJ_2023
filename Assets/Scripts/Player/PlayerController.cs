using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField, Foldout("Movement Variables")] private float moveSpeed = 2f;
	[SerializeField, Foldout("Movement Variables")] private float jumpForce = 8f;
	[SerializeField, Foldout("Movement Variables")] private float jumpCooldown = 0.5f;
	[SerializeField, Foldout("Movement Variables")] private float defaultGravityScale = 5f;
	[SerializeField, Foldout("Movement Variables")] private float fallGravityScale = 9f;
	[SerializeField, Foldout("Movement Variables")] private bool canJump = true;
	[Space]
	[SerializeField, Foldout("Movement Variables")] private LayerMask groundedMask = default;
	[SerializeField, Foldout("Movement Variables")] private float groundDetectionDistance = 0.35f;

	[SerializeField, Foldout("References")] private SpriteRenderer spriteRenderer = default;
	[SerializeField, Foldout("References")] private Transform groundedCheckTransform = default;

	private Rigidbody2D rb2d = default;
	private Animator animator;

	#region Unity Callbacks
	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();

		rb2d.gravityScale = defaultGravityScale;
	}
	private void Update()
	{
		EvaluateJump();
		//SetAnimatorStates();
	}
	#endregion

	#region Input Action Callbacks
	public void OnGroundedMovement(InputAction.CallbackContext context)
	{
		Vector2 inputAxis = context.ReadValue<Vector2>();
		rb2d.velocity = new Vector2(inputAxis.x * moveSpeed, rb2d.velocity.y);

		FlipSprite(inputAxis);
	}
	public void OnJump(InputAction.CallbackContext context)
	{
		if (!IsGrounded() || !canJump) return;

		if (context.performed)
		{
			StartCoroutine(JumpCooldownCoroutine());
			rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		}
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
		if (!Physics2D.Raycast(groundedCheckTransform.position, Vector2.down, groundDetectionDistance, groundedMask))
		{
			{
				return false;
			}
		}
		rb2d.gravityScale = defaultGravityScale;
		return true;
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
	private IEnumerator JumpCooldownCoroutine()
	{
		canJump = false;
		yield return new WaitForSeconds(jumpCooldown);
		canJump = true;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawRay(groundedCheckTransform.position, Vector2.down * groundDetectionDistance);
	}
}
