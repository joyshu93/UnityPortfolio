using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BulletBoundary")
        {
            Destroy(gameObject);
        }
    }
}
