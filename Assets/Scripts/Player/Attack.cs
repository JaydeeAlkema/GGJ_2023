using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour/*, IDamageable*/
{
	// TODO:
	// 1. Use scriptable Int instead
	[SerializeField] private int damage = 10;
    [SerializeField] private float pushForce = 15f;
	[SerializeField] private float destroyTime = 0.5f;

	private bool canDealDamage = true;

	public GameObject sender = null;

    public int Damage { get => damage; set => damage = value; }
    public float PushForce { get => pushForce; set => pushForce = value; }

    private void OnEnable()
	{
		Destroy(gameObject, destroyTime);
	}

	//public void DealDamage(int damage, PlayerController player)
	//{
	//	if (!canDealDamage) return;
	//	player.Health -= damage;
	//	canDealDamage = false;
	//}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!canDealDamage || collision.gameObject == sender) return;
		if (collision.GetComponent<PlayerController>() != null) collision.GetComponent<PlayerController>().Health -= damage;
        GameManager.instance.UiManager.UpdateHealthBars();
        PushAway(collision.GetComponent<Rigidbody2D>(),sender.transform.position, pushForce);
    }

    public void PushAway(Rigidbody2D rigidbody2D, Vector2 origin, float force)
    {
        Vector2 direction = (Vector2)rigidbody2D.transform.position - origin;
        rigidbody2D.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }
}
