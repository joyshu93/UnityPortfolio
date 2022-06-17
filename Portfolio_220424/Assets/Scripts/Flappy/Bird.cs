using UnityEngine;
using HEEUNG;

public class Bird : MonoBehaviour
{
    FlappyManager FM;

    //애니메이션
    Animator birdAnim;

    //움직임
    Rigidbody2D birdRb2d;  
    public float upForce = 200f;

    //피격효과
    PolygonCollider2D _coll;
    SpriteRenderer _renderer;
    bool bBlink = false, bShow = true;
    int blinkCount = 5;
    float fBlink = 3f;
    bool fGround = false;

    private void Start()
    {
        FM = GameObject.Find("FlappyManager").GetComponent<FlappyManager>();

        birdRb2d = GetComponent<Rigidbody2D>();
        birdAnim = GetComponent<Animator>();
        birdRb2d.bodyType = RigidbodyType2D.Kinematic;

        _renderer = GetComponent<SpriteRenderer>();
        _coll = GetComponent<PolygonCollider2D>();

        //초기화
        _coll.isTrigger = true; //충돌시 물리적 처리 x
        birdAnim.enabled = false; //애니메이션 x
    }

    private void Update()
    {
        if (FM.flappyGameOver) birdRb2d.simulated = false;

        if (Input.GetMouseButtonDown(0) &&  FM.flappyGameOver == false)
        {
            if(fGround == true)
            {
                Debug.Log("OffGround");
                _coll.isTrigger = true;
                fGround = false;
            }
            birdRb2d.velocity = Vector2.zero;
            birdRb2d.AddForce(new Vector2(0f, upForce));
            birdAnim.SetTrigger("SetFlap");
        }

        Blink();

    }

    public void GameStart()
    {
        birdRb2d.bodyType = RigidbodyType2D.Dynamic;
        birdAnim.enabled = true;
    }

    //isTrigger가 양쪽 다 false 상태의 Enter
    private void OnCollisionEnter2D(Collision2D collision)
    {
        birdAnim.SetTrigger("SetDie");
        FM.GameOver();
    }

    //isTrigger가 둘중 하나라도 true 상태의 Stay
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ground1" || collision.gameObject.name == "Ground2")
        {
            if (fGround == true) return;

            Debug.Log("OnGround");
            //땅과 부딪히면 트리거 false
            fGround = true;
            _coll.isTrigger = false;
        }
    }

    //isTrigger가 둘중 하나라도 true 상태의 Enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "DeadLine")
        {
            birdAnim.SetTrigger("SetDie");
            FM.GameOver();
        }
        if (bBlink) return; //깜박거리면 리턴       

        if (collision.gameObject.tag != "Collision") return; //충돌체가 column 또는 인지 확인

        bBlink = true; //깜박임 시작
        fBlink = 2.5f; //깜박일 시간
        FM.SetLifeDown();
    }

    void Blink()
    {
        if (!bBlink) return; //깜박거리는 상황이 아니면 종료
        
        FM.godMode = true;

        if (--blinkCount <= 0) //blinkCount 횟수동안 투명, 불투명을 유지
        {
            blinkCount = 5;
            if (bShow = !bShow) _renderer.color = Color.white;
            else _renderer.color = Color.clear;
        }

        fBlink -= Time.deltaTime;
        //깜빡임 시간 종료
        if(fBlink < 0f) 
        {
            bBlink = false; //깜박임 상태 제거
            _renderer.color = Color.white; //불투명 복귀

            if (FM.life == 0)
            {
                _coll.isTrigger = false;
            }

            FM.godMode = false;
        }
    }
}
