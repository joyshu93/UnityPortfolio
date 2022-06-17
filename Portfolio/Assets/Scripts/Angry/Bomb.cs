using UnityEngine;
using HEEUNG;

public class Bomb : Mouse
{
    AngryManager AM;
    [HideInInspector] public GameObject Instance; //인스턴스 게임오브젝트
    bool clickedOn = false;

    //스프링
    public Rigidbody2D _rb2d;
    SpringJoint2D _spring;
    Vector2 _prevVelocity; //프레임단위 바로 이전 속도

    //제한거리
    Ray _rayToCatapult; //최대 제한거리 지점을 위한 레이저
    Transform _zeroPoint; //원점

    //새총 줄
    LineRenderer lrLineBack, lrLineFore;

    private void Start()
    {
        AM = GameObject.Find("AngryManager").GetComponent<AngryManager>();

        _zeroPoint = GameObject.Find("ZeroPoint").GetComponent<Transform>();
        _rayToCatapult = new Ray(_zeroPoint.position, Vector3.zero);
        _rb2d = GetComponent<Rigidbody2D>();
        _spring = GetComponent<SpringJoint2D>();

        lrLineBack = GameObject.Find("LineBack").GetComponent<LineRenderer>();
        lrLineFore = GameObject.Find("LineFore").GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (AM.angryGameOver) return;

        if(AM.bCreate == true)
        {
            ShowLine(); //새총줄 표시

            if (clickedOn && _spring != null)
            {
                GetMouseWorldPoint(); //마우스 월드 포인트
                SetLimitDistance(_rayToCatapult, _zeroPoint); //볼의 제한거리 설정
                SetObjectToMouse(mouseWorldPoint.x, mouseWorldPoint.y,1f); //마우스 클릭 위치에 오브젝트
            }

            //스프링 존재시
            if (_spring != null)
            {
                //스프링 제거, 마지막 속력 저장 후 발사
                if (_prevVelocity.sqrMagnitude > _rb2d.velocity.sqrMagnitude)
                {
                    Destroy(_spring);

                    _rb2d.velocity = _prevVelocity; //마지막 속력값 저장

                    Invoke("Destroy", 7f); //발사 후 7초 뒤 제거
                }

                //마우스 놓을 때
                if (clickedOn == false)
                {
                    _prevVelocity = _rb2d.velocity; //바로 앞 속력 지정
                }
            }
        }       
    }

    //마우스 클릭
    private void OnMouseDown()
    {       
        clickedOn = true;
    }

    //마우스 논클릭
    private void OnMouseUp()
    {
        clickedOn = false;
        _rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    //새총 줄 표시
    void ShowLine()
    {
        if (clickedOn && _spring != null)
        {         
            //라인의 index 1을 포지션 업데이트
            lrLineBack.SetPosition(1, transform.position);
            lrLineFore.SetPosition(1, transform.position);
        }
        else
        {
            lrLineBack.SetPosition(1, new Vector3(1.15f, -0.5f, 0f));
            lrLineFore.SetPosition(1, new Vector3(1.15f, -0.5f, 0f));
        }
    }

    private void Destroy()
    {
        AM.ReStart(); //맵 초기화


        Destroy(gameObject);       
    }
}

