using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public float speed;
    public int health;

    public Sprite[] Sprs;
    SpriteRenderer SprRd;
    Rigidbody2D Rb2d;

    private void Awake()
    {
        SprRd = GetComponent<SpriteRenderer>();
        Rb2d = GetComponent<Rigidbody2D>();
        Rb2d.velocity = Vector2.down * speed; //¼Óµµ

    }

    public void OnHit(int damage)
    {
        health -= damage;
        SprRd.sprite = Sprs[1];
        Invoke("ReturnSprite", 0.1f);

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ReturnSprite()
    {
        SprRd.sprite = Sprs[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BulletBoundary")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            Destroy(collision.gameObject);
        }
    }
}
