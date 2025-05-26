using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 4;
    public Rigidbody2D target;

    private bool isLive = true;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!isLive) return;
        
        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * (speed * Time.fixedDeltaTime);
        
        rb.MovePosition(rb.position + nextVec);
        rb.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!isLive) return;
        
        sr.flipX = target.position.x < rb.position.x;
    }

    
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }
}
