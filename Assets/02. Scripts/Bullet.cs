using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //데미지, 관통 변수
    public float damage;
    public int per;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            rb.linearVelocity = dir * 15f;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy") || per == -100) return;
        
        per--;

        if (per < 0)
        {
            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area") || per == -100) return;
        
        gameObject.SetActive(false);
    }
}
