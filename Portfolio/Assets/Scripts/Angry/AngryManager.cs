using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AngryManager : MonoBehaviour
{
    AboutCamera AC;

    //게임
    Bomb Ball;
    public Transform SpawnPoint;
    public Text txtLife, txtBestScore, txtScore;
    [HideInInspector] public int score = 0, life = 3;
    int aliveEnemy = 3; //생존한 적의 수

    //게임종료 
    GameObject GameOverWindow; //결과창
    public bool angryGameOver;

    [Header("[ Prefabs ]")]
    public GameObject preBall; //공 프리팹 
    public GameObject Enemy;
    public GameObject Plank; //판자 프리팹
    public GameObject Count; //카운트 프리팹
    GameObject[] IngameObjs; //인게임 객체모음
    GameObject[] DestroyObjs; //삭제할 객체모음  

    [Header("[ Obstacles ]")]
    bool bCreateBall = false; //공 생성 유무
    bool bCreateObs = false; //장애물 생성 유무
    public bool bCreate = false; //공&&장애물 생성유무
    int cnt1stF, cnt2ndF, cnt3rdF;
    int size = 7; //한 층에 생성되는 최대 개수
    float Interval = 1.65f; //기둥 간격
    Vector2 StandardHorizontal = new Vector2(14.95f, -1f); //1층 기둥 생성 기준점
    Vector2 StandardVertical = new Vector2(15.75f, 0f); //2층 바닥 생성 기준점

    //적
    float[,] enemyPosX = new float[4, 7]; //층, X좌표
    float[,] enemyPosY = new float[4, 7]; //층, Y좌표
    
    //카메라
    public Transform ObstaclePosition; //장애물 좌표
    public Transform CatapultPosition; //새총 좌표
    Vector3 offset = new Vector3(1.15f, 0f, 0f); //카메라 목표지점

    private void Start()
    {
        AC = new AboutCamera();

        angryGameOver = false;

        //리소스 연결
        GameOverWindow = Resources.Load("GameOverWindow") as GameObject;

        //카메라 시작 세팅
        AC.SetLimitStep1(new Vector2(10.8f, 0.3f), new Vector2(30f, 9.4f)); //카메라 제한 스텝1

        Instantiate(Count).transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    private void Update()
    {
        //텍스트 표시
        txtScore.text = string.Format("SCORE : {0}", score);
        txtLife.text = string.Format("Life : {0}", life);
        txtBestScore.text = string.Format("BEST SCORE : {0}", AppManager.AM.SM.angryScore);   
    }

    private void LateUpdate()
    {
        AC.SetLimitStep2(Camera.main); //카메라 제한 스텝2     
    }

    private void FixedUpdate()
    {
        AC.SetSmoothMoving(offset, Camera.main);

        //공이 움직일 때 따라가기
        if (Ball != null && Mathf.Abs(Ball._rb2d.velocity.x + Ball._rb2d.velocity.y) > 0) 
        {
            AC.SetToPlayer(Camera.main);
        }
    }

    public void InitCreate()
    {
        //적과 장애물 제거
        DestroyObjs = GameObject.FindGameObjectsWithTag("Collision"); 
        foreach (var o in DestroyObjs) Destroy(o);
        bCreateObs = false;
        bCreate = false;
    }

    //스테이지(장애물, 적)와 공 생성
    public void Create()
    {
        if (angryGameOver) return;

        //생성 시작
        if (bCreateBall == false && bCreateObs == false)
        {
            CreateStage(); //스테이지 생성
        }

        if(bCreateObs == true && bCreateBall == false)
        {
           Invoke("CreateBall", 2f);
           bCreate = true;
        }
    }

    //공 생성
    void CreateBall()
    {
        GameObject tmp = Instantiate(preBall, SpawnPoint.position, SpawnPoint.rotation); //공 생성
        Ball = tmp.GetComponent<Bomb>();
        offset = CatapultPosition.position;
    }

    //스테이지(장애물, 적) 생성 함수
    public void CreateStage()
    {
        //장애물 생성
        if (bCreateObs == false)
        {
            StartCoroutine(CoObastacle());
        }
        //코루틴 멈춤
        else if (bCreateObs == true)
        {
            StopCoroutine(CoObastacle());
        }
    }

    //장애물과 적 생성 코루틴
    IEnumerator CoObastacle()
    {
        //1층, 2층, 3층 칸의 개수
        //윗층은 아랫층보다 같거나 작음
        cnt1stF = Random.Range(1, size);
        cnt2ndF = Random.Range(1, cnt1stF + 1);
        cnt3rdF = Random.Range(1, cnt2ndF);

        CreateStageLogic(cnt1stF, 1, 0);
        CreateStageLogic(cnt2ndF, 2, cnt1stF);
        CreateStageLogic(cnt3rdF, 3, cnt2ndF);
        Invoke("CreatEnemy", 0.3f);

        yield return null;
    }

    //장애물 생성
    public void CreateStageLogic(int cnt, int floor, int compare)
    {    
        Vector2 hor = StandardHorizontal;
        Vector2 ver = StandardVertical;

        //기준점 다시잡기
        int tmp = cnt;
        if (tmp % 2 == 1) tmp += 1;
        hor.x -= (tmp / 2) * Interval;
        ver.x -= (tmp / 2) * Interval;
 
        if (floor > 1)
        {
            int a = cnt % 2; //현재층
            int b = compare % 2; //아래층
            //홀짝이 다르면, 간격 조정           
            //아래층이 홀수이며 5보다 크거나 같을때 왼쪽
            if(a != b && b == 1)
            {
                hor.x -= Interval / 2f;
                ver.x -= Interval / 2f;
                StandardHorizontal.x -= Interval / 2f;
                StandardVertical.x -= Interval / 2f;
            }
            else if(a != b)
            {
                hor.x += Interval / 2f;
                ver.x += Interval / 2f;
                StandardHorizontal.x += Interval / 2f;
                StandardVertical.x += Interval / 2f;
            }

            //높이 증가
            for (int i = 0; i < floor -1 ; i++)
            {
                hor.y += 2f;
                ver.y += 2f;
            }          
        }

        //기둥층
        for (int i = 0; i <= cnt; i++)
        {
            Quaternion rotation = Quaternion.identity; //각도
            rotation.eulerAngles = new Vector3(0f, 0f, 90f); //세워서 생성

            Instantiate(Plank, hor, rotation);
            hor.x += Interval;
        }

        //바닥층
        for(int i = 0; i <= cnt - 1; i++)
        {
            Quaternion rotation = Quaternion.identity; //각도
            rotation.eulerAngles = new Vector3(0f, 0f, 0f); //눕혀서 생성

            Instantiate(Plank, ver, rotation);
            enemyPosX[floor, i] = ver.x; //적을 만들기 위한 x좌표 추출해놓기
            enemyPosY[floor, i] = ver.y - 0.9f; //적을 만들기 위한 y좌표 추출해놓기
            ver.x += Interval;
        }

        if (floor == 3)
        {
            bCreateObs = true;
        }
    }

    //적 생성
    public void CreatEnemy()
    {
        int random = Random.Range(0, 7); //배열 0~6까지
        //1층부터 3층
        for(int i = 1; i <= 3; i++)
        {
            if(enemyPosX[i, random] != 0f)
            {
                Vector2 Pos = new Vector2(enemyPosX[i, random], enemyPosY[i, random]);
                Instantiate(Enemy, Pos, Quaternion.identity);
            }
            else
            {
                random = Random.Range(0, 7);
                i--;
            }
        }

        offset = ObstaclePosition.position;
    }

    //적 배열 초기화
    void InitEnemyPos()
    {
        System.Array.Clear(enemyPosX, 0, enemyPosX.Length);
        System.Array.Clear(enemyPosY, 0, enemyPosY.Length);
    }

    //재시작
    public void ReStart()
    {
        //적이 살아있으면 life -= 1
        if(aliveEnemy > 0 && life > 0)
        {
            life -= 1;
        }

        //종료
        if (life <= 0)
        {
            //점수 저장

            //종료
            GameOver();
        }
        //재시작
        else
        {
            bCreateBall = false; //생성된 공 x
            aliveEnemy = 3;

            InitEnemyPos(); //배열 초기화

            //장애물 재생성
            InitCreate();
            Invoke("Create", 2f);
        }
    }

    //인게임시 오브젝트관리
    public void SetIngameObjs()
    {
        foreach (var o in IngameObjs) o.SetActive(true); //인게임 오브젝트 나타내기
        Instantiate(Count).transform.SetParent(GameObject.Find("Canvas").transform, false); //시작 카운트
    }

    //점수 획득
    public void PlusScore()
    {
        aliveEnemy -= 1;
        score += 10;
    }

    public void GameOver()
    {
        if (angryGameOver) return;
            
        angryGameOver = true;
        Instantiate(GameOverWindow).transform.SetParent(GameObject.Find("Canvas").transform, false);
            
    }

}
