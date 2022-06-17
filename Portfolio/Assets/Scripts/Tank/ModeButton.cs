using UnityEngine;

public class ModeButton : MonoBehaviour
{
    TanksManager TM;
    GameObject Buttons;

    private void Start()
    {
        TM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TanksManager>();
        Buttons = GameObject.Find("Buttons");
    }

    public void  SoloMode()
    {
        TM.gamemode = 1;
        TM.GameStart();
        Buttons.SetActive(false);
    }
    public void VSMode()
    {
        TM.gamemode = 2;
        TM.GameStart();
        Buttons.SetActive(false);
    }
}
