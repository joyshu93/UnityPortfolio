using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Columns : MonoBehaviour
{
    BoxCollider2D box;

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        box.isTrigger = true; //감지만 하고 충돌은 않는다.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Bird>() != null)
        {
            FlappyManager FM = GameObject.Find("FlappyManager").GetComponent<FlappyManager>();
            FM.SetAddScore();
        }
    }

}
