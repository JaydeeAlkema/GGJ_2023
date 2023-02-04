using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    private float speed = 10f;

    private Rigidbody2D rb;
    public float Speed { get => speed; set => speed = value; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.right * Speed;
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public void DealDamage(int damage, PlayerController player)
    {
        player.Health -= damage;
        Debug.Log(player.Health);
        Destroy(gameObject);
    }
}
