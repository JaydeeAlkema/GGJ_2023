using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IDamageable
{
	// TODO:
	// 1. Use scriptable Int instead
	[SerializeField] private int damage = 10;

	private bool canDealDamage = true;

	public void DealDamage(int damage, PlayerController player)
	{
		if (!canDealDamage) return;
		player.Health -= damage;
		canDealDamage = false;
	}
}
