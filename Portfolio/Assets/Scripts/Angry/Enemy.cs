using UnityEngine;

public class Enemy : MonoBehaviour
{
    AngryManager AM;
    Animator aniEnemy;
    float lifeTime;
    public float neededForce = 15f; //파괴에 필요한 힘

    private void Start()
    {
        AM = GameObject.Find("AngryManager").GetComponent<AngryManager>();

        aniEnemy = this.GetComponent<Animator>();
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lifeTime < 2f) return;

       if(CollisionForce(collision) >= neededForce)
        {
            aniEnemy.SetBool("Damage", true);
        }
    }

    float CollisionForce(Collision2D coll)
    {
        float speed = coll.relativeVelocity.sqrMagnitude;
        if(coll.collider.GetComponent<Rigidbody2D>())
        {
            return speed * coll.collider.GetComponent<Rigidbody2D>().mass;
        }
        return speed;
    }

    public void Death()
    {
        AM.PlusScore();
        Destroy(gameObject);
    }
}
