using System.Collections;
using UnityEngine;
using HEEUNG;

public class Dongle : Mouse
{
    public DongleManager DM;

    Animator anim;
    public ParticleSystem effect; //효과

    public Rigidbody2D rb2d; //리지드바디
    CircleCollider2D cc2d; //콜라이더
    SpriteRenderer sprdr; //스프라이트

    public int level; //동글 레벨
    public bool isDrag; //드래그
    public bool isMerge; //합치기
    public bool isAttach; //충돌
    public bool isUsed; //사용 유무(점수계산을 위해)

    float deadTime;

    void Awake()
    {
        //참조
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cc2d = GetComponent<CircleCollider2D>();
        sprdr = GetComponent<SpriteRenderer>();
    }

    //동글 생성
    //스크립트 활성 시 실행되는 함수
    void OnEnable()
    {
        anim.SetInteger("Level", level); //레벨에 따른 애니메이션
    }

    //스크립트 비활성화 시 실행되는 함수
    private void OnDisable()
    {
        //동글 속성 초기화
        level = 0;
        isDrag = false;
        isMerge = false;
        isAttach = false;
        isUsed = false;

        //동글 트랜스폼 초기화
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.zero;

        //동글 물리 초기화
        rb2d.simulated = false;
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0;
        cc2d.enabled = true;
    }
    void Update()
    {
        if(isDrag)
        {
            GetMouseWorldPoint();
            SetLimitMouseMoving(-7.5f, 7.5f, transform);
            SetObjectToMouse(mouseWorldPoint.x, 3.5f, 0.2f);
        }     
    }

    public void Drag()
    {
        isDrag = true;
    }

    public void Drop()
    {
        isDrag = false;
        isUsed = true;
        rb2d.simulated = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(AttachRoutine());
    }

    IEnumerator AttachRoutine()
    {
        if (isAttach) { yield break; } //충돌음 겹침 방지 

        isAttach = true; //잠금
        DM.SFXPlay(DongleManager.SFX.Attach); //충돌음

        yield return new WaitForSeconds(0.2f); //0.2초 간격

        isAttach = false; //잠금 해제
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Dongle")
        {
            Dongle other = collision.gameObject.GetComponent<Dongle>();

            if(level == other.level && !isMerge && !other.isMerge && level < 7)
            {
                //현 동글과 충돌 동글 위치 가져오기
                float myX = transform.position.x;
                float myY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;

                //1.현 동글이 아래일 때
                //2.동일한 높이에, 현 동글이 오른쪽일 때
                if (myY < otherY || (myY == otherY && myX  > otherX))
                {
                    //충돌 동글 숨기기
                    other.Hide(transform.position, false);
                    //현 동글 레벨업
                    LevelUp();
                }
                
            }
        }
    }

    //gameover = false는 게임오버 후 삭제를 위해
    public void Hide(Vector3 targetPos, bool gameover)
    {
        isMerge = true;

        rb2d.simulated = false;
        cc2d.enabled = false;

        if (gameover == true) EffectPlay();

        StartCoroutine(HideRoutine(targetPos, gameover));
    }

    IEnumerator HideRoutine(Vector3 targetPos, bool gameover)
    {
        int frameCount = 0;
        while(frameCount < 20)
        {
            frameCount++;
            if(gameover == false)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
            }
            else if(gameover == true)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.2f);
            }

            yield return null;
        }

        DM.score += (int)Mathf.Pow(2, level); //점수 계산

        isMerge = false;
        gameObject.SetActive(false);
    }

    void LevelUp()
    {
        isMerge = true;

        rb2d.velocity = Vector2.zero; //물리속도 제거
        rb2d.angularVelocity = 0f; //회전속도 0

        StartCoroutine(LevelUpRoutine());
    }

    IEnumerator LevelUpRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetInteger("Level", level + 1); //성장 애니메이션
        EffectPlay();
        DM.SFXPlay(DongleManager.SFX.LevelUp);

        yield return new WaitForSeconds(0.3f);
        level++; //시간차 레벨업 계산

        DM.maxLevel = Mathf.Max(level, DM.maxLevel); //큰 레벨 값을 반환

        isMerge = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Finish")
        {
            deadTime += Time.deltaTime;
            if(deadTime > 2)
            {
                sprdr.color = Color.red;
            }
            if(deadTime > 5)
            {
                DM.GameOver();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag =="Finish")
        {
            deadTime = 0;
            sprdr.color = Color.white;
        }
    }

    void EffectPlay()
    {
        //위치와 크기 변경
        effect.transform.position = transform.position;
        effect.transform.localScale = transform.localScale;
        
        effect.Play(); //실행
    }
}

