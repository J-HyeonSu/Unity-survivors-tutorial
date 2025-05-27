using System;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public float speed = 4;
    public float health;
    public float maxHealth;
    
    
    public RuntimeAnimatorController[] animCon;
    
    public Rigidbody2D target;

    private bool isLive;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Bullet")) return;

        health -= other.GetComponent<Bullet>().damage;

        if (health > 0)
        {
            
        }
        else
        {
            Dead();
        }


    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
    
}
