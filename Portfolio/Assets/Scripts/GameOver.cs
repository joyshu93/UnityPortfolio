using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    Animator aniGameOver;

    OmokManager OM;
    OthelloManager OthM;
    AngryManager AM;
    FlappyManager FM;
    DongleManager DM;

    Text txtResult; //랭킹 정보

    void Start()
    {
        //초기화
        txtResult = GameObject.Find("txtRank").GetComponent<Text>();
        aniGameOver = this.GetComponent<Animator>();

        SetRank();

        this.transform.localScale = new Vector3(0f, 0f, 0f);
        aniGameOver.SetBool("OnOff", true); //등장
    }

    public void SetRank()
    {
        if (GameObject.Find("OmokManager"))
        {
            OM = GameObject.Find("OmokManager").GetComponent<OmokManager>();

            AppManager.AM.SM.SaveRankingData(AppManager.AM.gameTitle, OM.score, txtResult);
            AppManager.AM.SM.SaveUserScore(AppManager.AM.gameTitle, OM.score);
        }
        else if (GameObject.Find("OthelloManager"))
        {
            OthM = GameObject.Find("OthelloManager").GetComponent<OthelloManager>();

            AppManager.AM.SM.SaveRankingData(AppManager.AM.gameTitle, OthM.BlackCnt, txtResult);
            AppManager.AM.SM.SaveUserScore(AppManager.AM.gameTitle, OthM.BlackCnt);
        }
        else if (GameObject.Find("FlappyManager"))
        {
            FM = GameObject.Find("FlappyManager").GetComponent<FlappyManager>();

            AppManager.AM.SM.SaveRankingData(AppManager.AM.gameTitle, FM.score, txtResult);
            AppManager.AM.SM.SaveUserScore(AppManager.AM.gameTitle, FM.score);
        }
        else if (GameObject.Find("AngryManager"))
        {
            txtResult = GameObject.Find("txtRank").GetComponent<Text>();

            AM = GameObject.Find("AngryManager").GetComponent<AngryManager>();

            AppManager.AM.SM.SaveRankingData(AppManager.AM.gameTitle, AM.score, txtResult);
            AppManager.AM.SM.SaveUserScore(AppManager.AM.gameTitle, AM.score);
        }
        else if (GameObject.Find("DongleManager"))
        {
            txtResult = GameObject.Find("txtRank").GetComponent<Text>();

            DM = GameObject.Find("DongleManager").GetComponent<DongleManager>();

            AppManager.AM.SM.SaveRankingData(AppManager.AM.gameTitle, DM.score, txtResult);
            AppManager.AM.SM.SaveUserScore(AppManager.AM.gameTitle, DM.score);
        }
    }


}
