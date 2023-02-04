using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField, Foldout("Movement Variables")] private ScriptableFloat moveSpeed;
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

    [SerializeField] private BuffsScriptableObject buffScriptableObject;

    private Rigidbody2D rb2d = default;
    private Animator animator;

    public int Health { get => health; set => health = value; }

    #region Unity Callbacks
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        rb2d.gravityScale = defaultGravityScale;
        StartCoroutine(UpdateBuffs());
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
        rb2d.velocity = new Vector2(inputAxis.x * moveSpeed.currentValue, rb2d.velocity.y);

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

    #region buffs and collision related stuff
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.DealDamage(10, this);
            }
        }

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
                            b = new Buff(BuffTypes.Speed, buffScriptableObject.moveSpeedBuffDuration);
                            break;
                        case BuffTypes.Knockback:
                            b = new Buff(BuffTypes.Knockback, buffScriptableObject.boostedKnockbackDuration);
                            break;
                        case BuffTypes.Damage:
                            b = new Buff(BuffTypes.Damage, buffScriptableObject.damageBuffDuration);
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
        switch (newBuff.type)
        {
            case BuffTypes.Water:
                //TODO: Method that changes tree manager target
                break;
            case BuffTypes.Speed:
                moveSpeed.currentValue = buffScriptableObject.boostedMovementSpeed;
                break;
            case BuffTypes.Knockback:
                //knockback.currentValue = buffScriptableObject.boostedKnockback;
                break;
            case BuffTypes.Damage:
                //damage.currentValue = buffScriptableObject.boostedDamage
                break;
            default:
                break;
        }
    }

    private IEnumerator UpdateBuffs()
    {
        while (true)
        {
            for (int i = 0; i < currentBuffs.Count; i++)
            {
                currentBuffs[i].duration--;
                switch (currentBuffs[i].type)
                {
                    case BuffTypes.Water:
                        //TODO: Method that changes tree manager target
                        break;
                    case BuffTypes.Speed:
                        moveSpeed.currentValue = buffScriptableObject.boostedMovementSpeed;
                        break;
                    case BuffTypes.Knockback:
                        //knockback.currentValue = buffScriptableObject.boostedKnockback;
                        break;
                    case BuffTypes.Damage:
                        //damage.currentValue = buffScriptableObject.boostedDamage
                        break;
                    default:
                        break;
                }

                if (currentBuffs[i].duration <= 0)
                {
                    switch (currentBuffs[i].type)
                    {
                        case BuffTypes.Water:
                            break;
                        case BuffTypes.Speed:
                            moveSpeed.currentValue = moveSpeed.startValue;
                            break;
                        case BuffTypes.Knockback:
                            //knockBack.currentValue = knockBack.startvalue;
                            break;
                        case BuffTypes.Damage:
                            //damage.currentValue = damage.startValue;
                            break;
                        default:
                            break;
                    }
                    currentBuffs.Remove(currentBuffs[i]);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

}
