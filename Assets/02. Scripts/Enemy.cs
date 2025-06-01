using System;
using System.Collections;
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
    private Collider2D coll;
    private SpriteRenderer sr;
    private Animator anim;
    private WaitForFixedUpdate wait;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;
        
        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * (speed * Time.fixedDeltaTime);
        
        rb.MovePosition(rb.position + nextVec);
        rb.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive) return;
        if (!isLive) return;
        
        sr.flipX = target.position.x < rb.position.x;
    }

    
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rb.simulated = true;
        sr.sortingOrder = 2;
        anim.SetBool("Dead", false);
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
        if (!other.CompareTag("Bullet") || !isLive) return;

        health -= other.GetComponent<Bullet>().damage;

        StartCoroutine(KnockBack());
        
        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rb.simulated = false;
            sr.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if (GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
            
            
        }
    }

    //코루틴만의 반환형 인터페이스
    IEnumerator KnockBack()
    {
        //코루틴 반환 키워드
        //다음 하나의 물리 프레임딜레이
        yield return wait;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dir = (transform.position - playerPos).normalized;
        rb.AddForce(dir * 3, ForceMode2D.Impulse);
        

    }

		
	



    void Dead()
    {
        gameObject.SetActive(false);
    }
    
}
