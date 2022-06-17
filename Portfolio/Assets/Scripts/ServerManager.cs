using System.Collections.Generic;
using NCMB;
using UnityEngine;
using UnityEngine.UI;

public class ServerManager : MonoBehaviour
{
    //현 플레이어의 로드된 정보
    public string id;
    public long coin;
    public long omokScore;
    public long othelloScore;
    public long flappyScore;
    public long angryScore;
    public long dongleScore;
    public long tanksScore;


    public void SignUp(string _id, string _pw)
    {
        NCMBUser user = new NCMBUser();

        user.UserName = _id;
        user.Password = _pw;
        user.First = true;

        user.SignUpAsync((NCMBException e) =>
        {
            if (e != null)
            {
                print("SignUp Fail: " + e.ErrorMessage);
                GameObject.Find("AppManager").GetComponent<AppManager>().bSignUp = false;

            }
            else
            {
                print("SignUp Success");
                GameObject.Find("AppManager").GetComponent<AppManager>().bSignUp = true;
            }
        });
    }

    public void LogIn(string _id, string _pw)
    {
        NCMBUser.LogInAsync(_id, _pw, (NCMBException e) =>
        {
            if (e != null)
            {
                print("LogIn Fail: " + e.ErrorMessage);
            }
            else
            {
                print("Login Success");
                GameObject.Find("FadePanel").GetComponent<FadeInOut>().SetFade("Main"); //페이드 인 아웃
                
                LoadUserData();
            }
        });
    }

    void LoadUserData()
    {
        id = NCMBUser.CurrentUser.UserName;

        //첫 로그인이라면
        if (NCMBUser.CurrentUser.First)
        {
            NCMBUser.CurrentUser.First = false;

            NCMBUser.CurrentUser["Coin"] = 0;
            NCMBUser.CurrentUser["Omok"] = 0;
            NCMBUser.CurrentUser["Othello"] = 0;
            NCMBUser.CurrentUser["FlappyBird"] = 0;
            NCMBUser.CurrentUser["AngryBird"] = 0;
            NCMBUser.CurrentUser["DongleFamily"] = 0;
            NCMBUser.CurrentUser["Tanks"] = 0;

            NCMBUser.CurrentUser.SaveAsync((NCMBException e) =>
            {
                if (e != null)
                {
                    print("Init User Data Fail");
                }
                else
                {
                    print("Init User Data Success");
                }
            });
        }
       
        coin = (long)NCMBUser.CurrentUser["Coin"];
        omokScore = (long)NCMBUser.CurrentUser["Omok"];
        othelloScore = (long)NCMBUser.CurrentUser["Othello"];
        flappyScore = (long)NCMBUser.CurrentUser["FlappyBird"];
        angryScore = (long)NCMBUser.CurrentUser["AngryBird"];
        dongleScore = (long)NCMBUser.CurrentUser["DongleFamily"];
        tanksScore = (long)NCMBUser.CurrentUser["Tanks"];
    }

    public void SaveUserScore(string gameTitle, long score)
    {
        NCMBUser.CurrentUser["Coin"] = coin;
        if(score > (long)NCMBUser.CurrentUser[gameTitle])
        {
            NCMBUser.CurrentUser[gameTitle] = score;
        }

        NCMBUser.CurrentUser.SaveAsync((NCMBException e) =>
        {
            if (e != null)
            {
                print("Save User Score Fail");
            }
            else
            {
                LoadUserData();
                print("Save User Score Success");
            }
        });
    }

    public void SaveRankingData(string gameTitle, long score, Text txtRank)
    {
        NCMBObject obj = new NCMBObject(gameTitle);
        obj.Add("ID", id);
        obj.Add(gameTitle, score);


        obj.SaveAsync((NCMBException e) =>
        {
            if (e != null)
            {
                print("Save " + gameTitle +  " Score Fail");
            }
            else
            {
                LoadRankingData(gameTitle, txtRank);
                print("Save " + gameTitle + " Score Success");
            }
        });
    }

    void LoadRankingData(string gameTitle, Text txtRank)
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(gameTitle);
        query.AddDescendingOrder(gameTitle);

        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                print("Load " + gameTitle + " Score Fail");
            }
            else
            {
                string res = "";
                int rank = 0;
                foreach (NCMBObject obj in objList)
                {
                    if (rank >= 10) break;
                    res = string.Format("{0:D2}. ", ++rank);
                    res += obj["ID"] + ", ";
                    res += obj[gameTitle];
                    txtRank.text += res + "\n";
                }
                print("Load " + gameTitle + " Score Success");
            }
        });
    }

    public void SaveUserCoin(long value)
    {
        coin += value;
        NCMBUser.CurrentUser["Coin"] = coin;

        NCMBUser.CurrentUser.SaveAsync((NCMBException e) =>
        {
            if (e != null)
            {
                print("Save User Score Fail");
            }
            else
            {
                LoadUserData();
                print("Save User Score Success");
            }
        });
    }

    public void LogOut()
    {

        GameObject.Find("FadePanel").GetComponent<FadeInOut>().SetFade("Sign"); //페이드 인 아웃

        NCMBUser.LogOutAsync((NCMBException e) => {
            if (e != null)
            {
                print("LogOut fail");
            }
            else
            {
                print("LogOut SUCCESS");
            }
        });
    }
   

}
