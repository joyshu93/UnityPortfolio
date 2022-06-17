using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    GameObject pSettingBoard;
    Animator aniSettingBoard;

    [Header("[ Texts  ]")]
    Text txtCoin;
    Text txtCoinInMain;
    Text txtNicknameInMain;

    void Start()
    {
        pSettingBoard = GameObject.Find("SettingBoard");
        aniSettingBoard = pSettingBoard.GetComponent<Animator>();
        txtCoin = GameObject.Find("txtCoin").GetComponent<Text>();
        txtNicknameInMain = GameObject.Find("txtNicknameInMain").GetComponent<Text>();
        txtCoinInMain = GameObject.Find("txtCoinInMain").GetComponent<Text>();

    }
        
    private void FixedUpdate()
    {
        //세팅창
        txtCoin.text = string.Format("Coin : {0}", AppManager.AM.SM.coin);

        //메인
        txtNicknameInMain.text = string.Format("Nickname : {0}", AppManager.AM.SM.id);
        txtCoinInMain.text = string.Format("Coin : {0}", AppManager.AM.SM.coin);
    }

    public void OnSettingBoard()
    {
        aniSettingBoard.SetBool("OnOff", true);
    }

    public void OffSettingBoard()
    {
        aniSettingBoard.SetBool("OnOff", false);
    }
}
