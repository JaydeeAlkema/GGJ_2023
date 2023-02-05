using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour/*, IDamageable*/
{
	// TODO:
	// 1. Use scriptable Int instead
	[SerializeField] private int damage = 10;
	[SerializeField] private float destroyTime = 0.5f;

	private bool canDealDamage = true;

	public GameObject sender = null;

    public int Damage { get => damage; set => damage = value; }

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
        
    }
}
