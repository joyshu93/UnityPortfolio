using Practice;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Move mv = new Move();
    public Transform Tf;
    public Animator Ani;

    //ÃÑ¾Ë
    public GameObject BulletA;
    public GameObject BulletB;
    public float bulletSpeed = 10f;
    public float maxFireDelay = 0.2f;
    public float curFireDelay = 0f;
    public int bulletLevel = 1;

    private void Awake()
    {
        Tf = this.GetComponent<Transform>();
        Ani = this.GetComponent<Animator>();
        BulletA = Resources.Load("Practice/Player_Bullet_A") as GameObject;
        BulletB = Resources.Load("Practice/Player_Bullet_B") as GameObject;


    }

    private void Update()
    {
        PlayerFire();
        FireReload();
    }

    private void FixedUpdate()
    {
        PlayerMoving();
    }

    void PlayerMoving()
    {
        if (mv.horizontalDirection == 0) Ani.SetInteger("Move", 0);
        else if (mv.horizontalDirection == 1) Ani.SetInteger("Move", 1);
        else if (mv.horizontalDirection == -1) Ani.SetInteger("Move", -1);

        mv.Moving(Tf);
    }
    void PlayerFire()
    {
        if (!Input.GetButton("Fire1")) return; //¹ß»ç¹öÆ°À» ´­·¶´Â°¡
        if (curFireDelay < maxFireDelay) return; //µô·¹ÀÌ È®ÀÎ

        switch(bulletLevel)
        {
            case 1:
                GameObject Bullet = Instantiate(BulletA, transform.position, transform.rotation);
                Rigidbody2D Bullet_rb2d = Bullet.GetComponent<Rigidbody2D>();
                Bullet_rb2d.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject BulletL = Instantiate(BulletA, transform.position + Vector3.left * 0.1f, transform.rotation);
                GameObject BulletR = Instantiate(BulletA, transform.position + Vector3.right * 0.1f, transform.rotation);
                Rigidbody2D BulletL_rb2d = BulletL.GetComponent<Rigidbody2D>();
                Rigidbody2D BulletR_rb2d = BulletR.GetComponent<Rigidbody2D>();
                BulletL_rb2d.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                BulletR_rb2d.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject BulletLL = Instantiate(BulletA, transform.position + Vector3.left * 0.3f, transform.rotation);
                GameObject BulletCC = Instantiate(BulletB, transform.position, transform.rotation);
                GameObject BulletRR = Instantiate(BulletA, transform.position + Vector3.right * 0.3f, transform.rotation);
                Rigidbody2D BulletLL_rb2d = BulletLL.GetComponent<Rigidbody2D>();
                Rigidbody2D BulletCC_rb2d = BulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D BulletRR_rb2d = BulletRR.GetComponent<Rigidbody2D>();
                BulletLL_rb2d.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                BulletCC_rb2d.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                BulletRR_rb2d.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
                break;
        }
        curFireDelay = 0f;
    }

    void FireReload()
    {
        curFireDelay += Time.deltaTime;
    }
}
