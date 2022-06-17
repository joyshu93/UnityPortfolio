public class Logic2D
{
    //2차원 데이터
    private int[,] arr; //2차원 데이터
    private int row, col; //바둑판 열과 행 최대값

    private int mCheckValue = 0; //비교값
    protected int length; //오브젝트들의 연속된 길이
    protected int CountP1; //오브젝트 개수
    protected int CountP2;

    protected void Counting()
    {
        CountP1 = 0;
        CountP2 = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (arr[i, j] != -1)
                {
                    if (arr[i, j] == 0) CountP1++;
                    else if (arr[i, j] == 1) CountP2++;
                }
            }
        }
    }

    //판이 가득찼는지 세어보기
    protected bool IsFull()
    {
        Counting();
        if ((CountP1 + CountP2) == (row * col)) return true;
        else return false;
    }

    //비교값저장
    protected int CheckValue
    {
        set { mCheckValue = value; }
    }

    //임시 저장 배열 설정
    private const int DATASIZE = 100;
    protected class Point
    {
        public int x, y;
    }
    protected Point[] mPoints;

    //데이터 저장
    public int GetValue(int r, int c)
    {
        return arr[r, c];
    }

    //length 초기화
    protected void ResetLength()
    {
        length = 0;
    }

    //방향
    protected enum Direction
    {
        U, //up
        UR, //up-right
        R, //right
        DR, //down-right
        D, //down
        DL, //down-left
        L, //left
        UL //up-left
    }

    //방향에 따른 2차원 좌표이동값
    protected int[,] Move = new int[8, 2]
    {
        {0, 1},
        {1, 1},
        {1, 0},
        {1, -1},
        {0, -1},
        {-1, -1},
        {-1, 0},
        {-1, 1}
    };

    public Logic2D(int r, int c)
    {
        row = r;
        col = c;
        InitData();
    }

    //보드 초기화
    public void InitData()
    {
        mPoints = new Point[DATASIZE];
        for (int i = 0; i < mPoints.Length; i++) mPoints[i] = new Point();

        arr = new int[row, col];
        ResetLength();

        //보드 초기화
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                arr[i, j] = -1;
            }
        }

    }

    public void SetValue(int _turn, int r, int c)
    {
        arr[r, c] = _turn;
    }

    protected bool IsSequential(int chk, ref int _length)
    {
        if (chk == mCheckValue)
        {
            _length++;
            return true;
        }
        return false;
    }

    protected bool AnalyzeDirection(int chk, int dir, int sr, int sc)
    {
        CheckValue = chk;
        for (int r = sr, c = sc;
            (0 <= r && r < row) && (0 <= c && c < col);
            r += Move[dir, 0], c += Move[dir, 1])
        {
            if (r == sr && c == sc) continue;

            if (!IsSequential(arr[r, c], ref length))
            {
                return arr[r, c] == -1;
            }
            else
            {
                int index = length - 1;
                mPoints[index].x = r;
                mPoints[index].y = c;
            }
        }
        return true;
    }
}

public class LogicOmok : Logic2D
{
    public LogicOmok(int r, int c) : base(r, c) { }

    public int GetCountP1
    {
        get { return CountP1; }
    }
    public int GetCountP2
    {
        get { return CountP2; }
    }

    public void Count()
    {
        Counting();
    }

    public bool Full()
    {
        if (IsFull()) return true;
        else return false;
    }

    public bool Analyze(int _turn, int r, int c)
    {
        for (Direction dir = Direction.U; dir <= Direction.UL; dir++)
        {
            AnalyzeDirection(_turn, (int)dir, r, c);

            if (length >= 4) return true;
            ResetLength();
        }
        return false;
    }


}

public class LogicOthello : Logic2D
{
    public LogicOthello(int r, int c) : base(r, c) { }

    public int GetCountP1
    {
        get { return CountP1; }
    }
    public int GetCountP2
    {
        get { return CountP2; }
    }
    public void Count()
    {
        Counting();
    }

    public bool Full()
    {
        if (IsFull()) return true;
        else return false;
    }

    public int GetArr(int r, int c)
    {
        return GetValue(r, c);
    }

    public bool Analyze(int _turn, int r, int c)
    {
        if (GetArr(r, c) != -1) return false;

        int chkTurn = 1 - _turn;
        ResetLength();

        for (Direction dir = Direction.U; dir <= Direction.UL; dir++)
        {
            int beforeLength = length;
            if (AnalyzeDirection(chkTurn, (int)dir, r, c))
            {
                length = beforeLength;
            }
        }
        if (length > 0)
        {
            for (int i = 0; i < length; i++)
            {
                SetValue(_turn, mPoints[i].x, mPoints[i].y);
            }
            return true;
        }
        return false;
    }


}

