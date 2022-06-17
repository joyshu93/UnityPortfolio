using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public static AppManager AM;
    public ServerManager SM;

    public string gameTitle; //현 게임 제목
    public string[] sceneNames = new string[9] { "Logo", "Sign", "Main", "Omok", "Othello", "FlappyBird", "AngryBird", "DongleFamily", "Tanks"};
    public int sceneNumber;
    public bool bSignUp; //회원가입 성공, 실패

    void Awake()
    {
        SM = GetComponent<ServerManager>();

        if (AM == null)
        {
            AM = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (AM != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        GetSceneValue();
    }

    void GetSceneValue()
    {
        sceneNumber = Array.IndexOf(sceneNames, SceneManager.GetActiveScene().name);
        gameTitle = sceneNames[sceneNumber];
    }

    public void LogIn(string _id, string _pw)
    {
        SM.LogIn(_id, _pw);
    }

    public void LogOut()
    {
        SM.LogOut();
    }

    public void SignUp(string _id, string _pw)
    {
        SM.SignUp(_id, _pw);
    }
}
