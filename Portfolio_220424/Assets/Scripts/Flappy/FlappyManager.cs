using UnityEngine;
using UnityEngine.UI;

public class FlappyManager : MonoBehaviour
{
    GameObject Count;
    GameObject GameOverWindow;
    public bool godMode = false; //무적상태
    public bool flappyGameOver;
    public int score = 0, life = 3;

    [Header("[ Text ]")]
    public Text txtBestScore;
    public Text txtScore;
    public Text txtLife; 

    private void Start()
    {
        flappyGameOver = false;

        //결과창
        GameOverWindow = Resources.Load("GameOverWindow") as GameObject;

        //리소스 연결
        Count = Resources.Load("Count") as GameObject;  //시작 카운트

        //텍스트 표시
        txtLife.text = string.Format("Life : {0}", life);
        txtBestScore.text = string.Format("BEST SCORE : {0}", AppManager.AM.SM.flappyScore);

        Instantiate(Count).transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    public void SetAddScore()
    {
        if (flappyGameOver) return;
        if (godMode == true) return;
        
        score += 10;

        txtScore.text = string.Format("SCORE : {0}", score);
    }

    public void GameOver()
    {
        if(flappyGameOver) return;

        flappyGameOver = true;

        Instantiate(GameOverWindow).transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    public void SetLifeDown()
    {
        life--;
        txtLife.text = string.Format("Life : {0}", life);
    }

}
