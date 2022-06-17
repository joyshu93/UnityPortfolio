using UnityEngine;
using UnityEngine.UI;
using HEEUNG;

public class OthelloManager : MonoBehaviour
{
    LogicOthello lo;

    public int BlackCnt;
    public int WhiteCnt;
    public int turn; //0은 1p, 1은 2p
    bool othelloGameOver;

    public GameObject TurnImageB;
    public GameObject TurnImageW;
    public Text txtBlackCnt;
    public Text txtWhiteCnt;

    GameObject GameOverWindow;

    bool bCreateSlider;
    public bool bComMakeStone = false;

    //렌더링
    int r, c;  //이미지에서의 바둑판 행과 열 (x, y)
    float StartColPos = 6.85f; //x좌표의 가장 왼쪽아래 지점
    float StartRowPos = 6.85f;
    float interval = 0.9f; //한칸 간격
    const int num = 8;

    [Header("[ Prefabs ]")]
    public GameObject _preStoneW;
    public GameObject _preStoneB;
    public GameObject pSlider;

    void Start()
    {
        GameOverWindow = Resources.Load("GameOverWindow") as GameObject;

        //초기화
        lo = new LogicOthello(num, num);
        turn = 0;
        othelloGameOver = false;
        InitBoard();
    }

    void Update()
    {
        if (turn == 1 && othelloGameOver == false)
        {
            if (bCreateSlider == false)
            {
                CreateSlider();
            }

            if (bComMakeStone == true)
            {
                ComStonePos(); //컴퓨터 돌 생성
                if (!lo.Analyze(turn, r, c)) return;

                //돌 소환
                ComMakeStone();              
                Debug.Log(r + ", " + c);
                ReverseStoneImage();

                if (lo.Full())
                {
                    //게임 결과창 오픈
                    GameOver();
                }

                //턴 바꾸기
                ChangeTurn();

                bCreateSlider = false;
                bComMakeStone = false;
            }
        }
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

                if (hit.collider == null && othelloGameOver == false)
                {             
                    if (r == -1 || c == -1)
                    {
                        Debug.Log(r + ", " + c + ": 올바른 위치가 아닙니다.");
                        return; //범위 벗어나면 취소
                    }
                    else Debug.Log(r + ", " + c);

                    //오델로 판단
                    if (!lo.Analyze(turn, r, c)) return;

                    //돌 소환
                    MakeStone(spawn);
                    ReverseStoneImage();


                    if (lo.Full())
                    {
                        //게임 결과창 오픈
                        GameOver();
                    }

                    //턴 바꾸기
                    ChangeTurn();
                }
            }
        }        
    }

    private void FixedUpdate()
    {
        ChangeTurnImage();
        GetCountStone();
    }

    void ChangeTurn()
    {
        turn = 1 - turn;
    }

    void GetCountStone()
    {
        lo.Count();

        BlackCnt = lo.GetCountP1;
        WhiteCnt = lo.GetCountP2;

        txtBlackCnt.text = BlackCnt.ToString();
        txtWhiteCnt.text = WhiteCnt.ToString();
    }
    
    void CreateSlider()
    {
        Instantiate(pSlider).transform.SetParent(GameObject.Find("Canvas").transform, false);
        bCreateSlider = true;
    }

    void ComMakeStone()
    {
        if (turn != 1) return;

        if (lo.GetValue(r, c) == -1)
        {
            Vector3 Pos = new Vector3((r * interval) + StartRowPos, (c * interval) + StartColPos, 0);
            Instantiate(_preStoneW, Pos, Quaternion.identity);
            lo.SetValue(turn, r, c);

        }
    }

    void ComStonePos()
    {
        r = Random.Range(0, num);
        c = Random.Range(0, num);
    }
    void InitBoard()
    {
        lo.SetValue(0, 3, 3);
        lo.SetValue(0, 4, 4);
        lo.SetValue(1, 3, 4);       
        lo.SetValue(1, 4, 3);

        for(int i = 3; i <= 4; i++)
        {
            for(int j = 3; j <= 4; j++)
            {
                if(i == j)
                {
                    Instantiate(_preStoneB, new Vector3((i * interval) + StartRowPos, (j * interval) + StartColPos, 0), Quaternion.identity);
                }
                else Instantiate(_preStoneW, new Vector3((i * interval) + StartRowPos, (j * interval) + StartColPos, 0), Quaternion.identity);
            }
        }
      
    }

    void GameOver()
    {
        if (othelloGameOver) return;

        othelloGameOver = true;

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

    void ReverseStoneImage()
    {
        for(int i = 0; i < num; i++)
        {
            for(int j = 0; j < num; j++)
            {
                Vector3 tmp = new Vector3((i * interval) + StartColPos, (j * interval) + StartRowPos, 0f);
                Vector3 _direction = Vector3.forward;
                RaycastHit2D _hit = Physics2D.Raycast(tmp, _direction);

                //돌이 있는 자리 일때
                if(_hit.collider != null)
                {
                    Collider2D coll = _hit.collider;
                    //흑이 있는데 백으로 만들어야 한다면
                    if (lo.GetArr(i, j) == 1 && coll.gameObject.tag == "BlackStone")
                    {
                        Destroy(coll.gameObject);
                        Instantiate(_preStoneW, tmp, Quaternion.identity);
                    }
                    else if (lo.GetArr(i, j) == 0 && coll.gameObject.tag == "WhiteStone")
                    {
                        Destroy(coll.gameObject);
                        Instantiate(_preStoneB, tmp, Quaternion.identity);
                    }
                }             
            }
        }
    }

}