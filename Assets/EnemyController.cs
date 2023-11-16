using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform player;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Calculate direction towards player
        Vector2 direction = player.position - transform.position;

        // Normalize direction
        direction.Normalize();

        // Calculate movement vector
        Vector2 move = direction * moveSpeed;

        // Move enemy
        rb.velocity = new Vector2(move.x, move.y);
    }
}
