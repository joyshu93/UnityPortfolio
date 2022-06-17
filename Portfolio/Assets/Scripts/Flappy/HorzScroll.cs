using HEEUNG;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HorzScroll : HScroll
{
    FlappyManager FM;

    private void Awake()
    {
        FM = GameObject.Find("FlappyManager").GetComponent<FlappyManager>();
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if(FM.flappyGameOver == true)
        {
            SetStop();
        }
    }

    void GameStart()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        SetRigidbody(2f);
    }

}
