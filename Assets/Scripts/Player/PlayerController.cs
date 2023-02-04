using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
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
	[SerializeField, Foldout("References")] private Transform directionIndicator = default;

	[SerializeField] private List<Buff> currentBuffs = new List<Buff>();

	private Rigidbody2D rb2d = default;
	private Animator animator;

	#region Unity Callbacks
	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();

		rb2d.gravityScale = defaultGravityScale;
		StartCoroutine(DecayBuffs());
	}
	private void Update()
	{
		SetAnimatorStates();
		EvaluateJump();
	}
	#endregion

	#region Input Action Callbacks
	public void OnGroundedMovement(InputAction.CallbackContext context)
	{
		Vector2 inputAxis = context.ReadValue<Vector2>();
		rb2d.velocity = new Vector2(inputAxis.x * moveSpeed, rb2d.velocity.y);

		if (inputAxis == Vector2.zero) return;

		float angle = Mathf.Atan2(inputAxis.y, inputAxis.x) * Mathf.Rad2Deg;
		directionIndicator.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

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
	public void OnAttack(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			animator.SetTrigger("Attack");
		}
	}
	#endregion

	private void FlipSprite(Vector2 inputAxis)
	{
		switch (inputAxis.x)
		{
			case > 0:
				spriteRenderer.flipX = true;
				break;
			case < 0:
				spriteRenderer.flipX = false;
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
		animator.SetFloat("Velocity X", rb2d.velocity.x);
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

	#region buffs related stuff
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Collectable")
		{
			ICollectable collectable = other.gameObject.GetComponent<ICollectable>();
			if (collectable != null)
			{
				if (other.GetComponent<BuffCollectable>() != null)
				{
					Buff b = null;
					switch (other.GetComponent<BuffCollectable>().buffType)
					{
						case BuffTypes.Water:
							b = new Buff(BuffTypes.Water, 1);
							break;
						case BuffTypes.Speed:
							b = new Buff(BuffTypes.Speed, 30);
							break;
						case BuffTypes.Knockback:
							b = new Buff(BuffTypes.Knockback, 10);
							break;
						case BuffTypes.Damage:
							b = new Buff(BuffTypes.Damage, 10);
							break;
						default:
							break;
					}
					AddBuff(b);
				}
				collectable.Collect(this);
			}
		}
	}

	public void AddBuff(Buff newBuff)
	{
		foreach (Buff buff in currentBuffs)
		{
			if (buff.type == newBuff.type)
			{
				return;
			}
		}
		currentBuffs.Add(newBuff);
	}

	private IEnumerator DecayBuffs()
	{
		while (true)
		{
			for (int i = 0; i < currentBuffs.Count; i++)
			{
				currentBuffs[i].duration--;
				if (currentBuffs[i].duration <= 0)
				{
					currentBuffs.Remove(currentBuffs[i]);
				}
			}
			yield return new WaitForSeconds(1f);
		}
	}
	#endregion
}
