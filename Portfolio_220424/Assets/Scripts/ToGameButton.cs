using UnityEngine;
using UnityEngine.UI;
using HEEUNG;

public class ToGameButton : ObjectAnimation
{
    GameObject obj;
    [HideInInspector]public Text txtBtnNickname, textIF;
    InputField[] IFs = new InputField[2];

    //메인화면
    public void ToMain()
    {
        GameObject.Find("FadePanel").GetComponent<FadeInOut>().SetFade("Main"); //페이드 인 아웃
    }

    //게임화면으로 이동
    public void ToGame()
    {
        //코인 확인
        if (AppManager.AM.SM.coin <= 0) return;
        AppManager.AM.SM.SaveUserCoin(-10);

        GameObject.Find("FadePanel").GetComponent<FadeInOut>().SetFade(name); //페이드 인 아웃
    }


    //재시작
    public void RestartGame()
    {
        if (AppManager.AM.SM.coin <= 0)
        {
            OnObject();
            Invoke("OffObject", 1f);
            return;
        }
        AppManager.AM.SM.SaveUserCoin(-10);

        GameObject.Find("FadePanel").GetComponent<FadeInOut>().SetFade(AppManager.AM.gameTitle); //페이드 인 아웃
    }

    public void GG()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        foreach (var o in objs) o.SendMessage("GameOver");
    }

    public void BtnPlusCoin()
    {
        AppManager.AM.SM.SaveUserCoin(10);
    }

    public void LogInBtn()
    {
        IFs[0] = GameObject.Find("ifID").GetComponent<InputField>();
        IFs[1] = GameObject.Find("ifPW").GetComponent<InputField>();
        AppManager.AM.LogIn(IFs[0].text, IFs[1].text);
    }

    public void LogOutBtn()
    {
        AppManager.AM.LogOut();
    }

    public void SignUpBtn()
    {
        IFs[0] = GameObject.Find("ifID2").GetComponent<InputField>();
        IFs[1] = GameObject.Find("ifPW2").GetComponent<InputField>();
        AppManager.AM.SignUp(IFs[0].text, IFs[1].text);

        if(AppManager.AM.bSignUp == true)
        {
            Invoke("OnObject", 1f);
            Invoke("OffObject", 3f);
        }
        else if (AppManager.AM.bSignUp == false)
        {
            Invoke("OnObject", 1f);
            Invoke("OffObject", 3f);
        }

    }

    //객체 On
    public void OnObject()
    {
        if (this.name == "OpenSetting")
        {
            obj = GameObject.Find("SettingBoard");
            SetObj(true, 1, obj);

        }
        else if (this.name == "btnSignUp")
        {
            obj = GameObject.Find("pSignUp");
            SetObj(true, 0, obj);

        }
        else if (this.name == "btnRestart")
        {
            obj = GameObject.Find("txtWarning");
            SetObj(true, 0, obj);
        }
        else if (this.name == "btnCheck")
        {
            if(AppManager.AM.bSignUp == true)
            {
                obj = GameObject.Find("txtSignUpSuccess");
            }
            else if (AppManager.AM.bSignUp == false)
            {
                obj = GameObject.Find("txtSignUpFail");
            }
            SetObj(true, 0, obj);
        }

    }

    //객체 Off
    public void OffObject()
    {
        if (this.name == "CloseSetting")
        {
            obj = GameObject.Find("SettingBoard");
            SetObj(false, 1, obj);
        }
        else if (this.name == "btnCancel")
        {
            obj = GameObject.Find("pSignUp");
            SetObj(false, 0, obj);

        }
        else if(this.name == "btnRestart")
        {
            obj = GameObject.Find("txtWarning");
            SetObj(false, 0, obj);
        }
        else if (this.name == "btnCheck")
        {
            if (AppManager.AM.bSignUp == true)
            {
                obj = GameObject.Find("txtSignUpSuccess");
            }
            else if (AppManager.AM.bSignUp == false)
            {
                obj = GameObject.Find("txtSignUpFail");
            }
            SetObj(false, 0, obj);


        }


    }

}

