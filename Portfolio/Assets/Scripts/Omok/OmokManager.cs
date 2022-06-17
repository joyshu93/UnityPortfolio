using UnityEngine;

public class OmokManager : MonoBehaviour
{
    LogicOmok lo;

    public int BlackCnt;
    public int WhiteCnt;
    public int turn; //0은 1p, 1은 2p
    bool omokGameOver;
    public bool isOmok; //오목 완성 판별
    public int score; //점수(400점 만점 - 돌갯수)

    public GameObject TurnImageW;
    public GameObject TurnImageB;
    GameObject GameOverWindow;

    bool bCreateSlider;
    public bool bComMakeStone = false; //컴퓨터 턴 표시위한 bool

    int r, c, or, oc;  //이미지에서의 바둑판 행과 열 (x, y)
    float StartColPos = 5.83f; //x좌표의 가장 왼쪽아래 지점
    float StartRowPos = 5.83f;
    float interval = 0.463f; //한칸 간격
    int num = 19;

    [Header("[ Prefabs ]")]
    public GameObject _preStoneW;
    public GameObject _preStoneB;
    public GameObject pSlider;


    void Start()
    {
        //초기화   
        lo = new LogicOmok(num, num);
        isOmok = false;
        score = 0;
        omokGameOver = false;
        turn = 0;
        GameOverWindow = Resources.Load("GameOverWindow") as GameObject;
    }

    void Update()
    {
        ChangeTurnImage();

        //컴퓨터 턴
        if (turn == 1 && omokGameOver == false)
        {
            if (bCreateSlider == false)
            {
                CreateSlider();
            }

            if (bComMakeStone == true)
            {
                //돌 소환
                ComMakeStone();
                Debug.Log(r + ", " + c);

                //오목 판단
                if (lo.Analyze(turn, r, c))
                {
                    isOmok = true;
                    CountStone();
                    //게임 결과창 오픈
                    GameOver();
                }

                if (lo.Full())
                {
                    CountStone();
                    //게임 결과창 오픈
                    GameOver();
                }

                //턴 바꾸기
                ChangeTurn();
                bCreateSlider = false;
                bComMakeStone = false;
            }
        }
        //플레이어 턴
        else if (turn == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //클릭 위치의 world point변환
                Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);//클릭 지점 보정
                float newX = ReturnColPos(mouseWorldPoint.x);
                float newY = ReturnRowPos(mouseWorldPoint.y);
                //돌 소환지점
                Vector3 spawn = new Vector3(newX, newY, 0f);
                //누른지점 레이저 발사
                Vector3 direction = Vector3.forward;
                RaycastHit2D hit = Physics2D.Raycast(spawn, direction);

                if (hit.collider == null && omokGameOver == false)
                {

                    if (r == -1 || c == -1)
                    {
                        Debug.Log(r + ", " + c + ": 올바른 위치가 아닙니다.");
                        return; //범위 벗어나면 취소
                    }
                    else Debug.Log(r + ", " + c + "test");

                    or = r;
                    oc = c;

                    //돌 소환
                    MakeStone(spawn);

                    //오목 판단
                    if (lo.Analyze(turn, r, c))
                    {
                        isOmok = true;
                        CountStone();
                        //게임 결과창 오픈
                        GameOver();
                    }

                    if (lo.Full())
                    {
                        CountStone();
                        //게임 결과창 오픈
                        GameOver();
                    }

                    //턴 바꾸기
                    ChangeTurn();
                }
            }
        }

    }

    void ChangeTurn()
    {
        turn = 1 - turn;
    }

    void CountStone()
    {
        lo.Count();
        BlackCnt = lo.GetCountP1;
        WhiteCnt = lo.GetCountP2;
    }


    void CreateSlider()
    {
        Instantiate(pSlider).transform.SetParent(GameObject.Find("Canvas").transform, false);
        bCreateSlider = true;
    }

    void GameOver()
    {
        if (omokGameOver) return;

        omokGameOver = true;
        if (isOmok) score = 400 - (BlackCnt + WhiteCnt); //오목 완성일때만 계산
        Instantiate(GameOverWindow).transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    void MakeStone(Vector3 Pos)
    {
        if (turn == 0)
        {
            Instantiate(_preStoneB, Pos, Quaternion.identity);
            lo.SetValue(turn, r, c);
        }
    }

    void ComMakeStone()
    {
        if (turn != 1) return;

        while (true)
        {
            ComStonePos();
            if (lo.GetValue(r, c) == -1)
            {
                Vector3 Pos = new Vector3((r * interval) + StartRowPos, (c * interval) + StartColPos, 0);
                Instantiate(_preStoneW, Pos, Quaternion.identity);
                lo.SetValue(turn, r, c);
                break;
            }
        }
    }

    void ComStonePos()
    {
        /*
        r = Random.Range(0, num);
        c = Random.Range(0, num);
        */
        int sr = or, sc = oc;
        int[,] move = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
        int mi = 0, times = 2, _r = sr, _c = sc, cnt = 1;
        bool ok = false;
        while (times-- > 0)
        {
            for (int i = 0; i < cnt; i++)
            {
                _r += move[mi, 0];
                _c += move[mi, 1];
                if (lo.GetValue(_r, _c) == -1)
                {
                    ok = true;
                    break;
                }
            }

            if (ok) break;
            else
            {
                mi = (mi + 1) % 4;
                if (times == 0)
                {
                    times = 2;
                    cnt++;
                }
            }
        }

        r = _r;
        c = _c;
    }


    float ReturnColPos(float colPos)
    {
        float x = (colPos - StartColPos) / interval;
        int xx = (int)(x / 1f);
        if (x - xx > 0.5f)
        {
            xx += 1;
        }

        //범위 벗어나는것 지정
        if (xx < 0 || xx >= num) xx = -1;
        r = xx;

        float newXPos = (xx * interval) + StartColPos;
        return newXPos;
    }

    float ReturnRowPos(float rowPos)
    {
        float y = (rowPos - StartRowPos) / interval;
        int yy = (int)(y / 1f);
        if (y - yy > 0.5f)
        {
            yy += 1;
        }

        //범위 벗어나는것 지정
        if (yy < 0 || yy >= num) yy = -1;
        c = yy;

        float newYPos = (yy * interval) + StartRowPos;
        return newYPos;
    }

    void ChangeTurnImage()
    {
        if (turn == 0)
        {
            TurnImageB.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            TurnImageW.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);
        }
        else if (turn == 1)
        {
            TurnImageB.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);
            TurnImageW.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
    }
}