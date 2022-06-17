using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TanksManager : MonoBehaviour
{
    public int gamemode;
    public Transform spawnPosition2;
    public CameraControl m_CameraControl;
    public Text m_MessageText;
    public Tanks[] m_Tanks;

    [HideInInspector] public int m_NumRoundsToWin = 5;
    [HideInInspector] public float m_StartDelay = 3f;
    [HideInInspector] public float m_EndDelay = 3f;
    private int m_RoundNumber;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    private Tanks m_RoundWinner;
    private Tanks m_GameWinner;

    [Header("[ Prefabs  ]")]
    public GameObject[] m_TankPrefab;
    public GameObject EnemyPrefab;

    private void Start()
    {
        //초기화
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);    
    }

    public void GameStart()
    {
        //탱크 소환
        //탱크위치들로 카메라 세팅
        SpawnAllTanks();
        SetCameraTargets();

        //게임 시작
        StartCoroutine(GameLoop());
    }
    

    private void SpawnAllTanks()
    {
        switch(gamemode)
        {
            case 1:
                m_Tanks[0].m_Instance =
                       Instantiate(m_TankPrefab[0], m_Tanks[0].m_SpawnPoint.position, m_Tanks[0].m_SpawnPoint.rotation);
                m_Tanks[0].m_PlayerNumber = 1;
                m_Tanks[0].Setup();
                m_Tanks[0].m_Camera.enabled = false;
                m_Tanks[0].m_FirstPersonCanvas.enabled = false;

                m_Tanks[1].m_Instance = Instantiate(EnemyPrefab, m_Tanks[1].m_SpawnPoint.position, m_Tanks[1].m_SpawnPoint.rotation);
                m_Tanks[1].m_PlayerNumber = 99;
                m_Tanks[1].Setup();
                break;
            case 2:
                for (int i = 0; i < m_Tanks.Length; i++)
                {
                    m_Tanks[i].m_Instance =
                        Instantiate(m_TankPrefab[i], m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation);

                    m_Tanks[i].m_PlayerNumber = i + 1;
                    m_Tanks[i].Setup();

                }
                break;
        }
        
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length];
       
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;      
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if(m_GameWinner != null)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        yield return m_StartWait;
    }    

    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        m_MessageText.text = "";

       
        while(!OneTankLeft())
        {
            yield return null;
        }  
    }

    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        m_RoundWinner = null;
        m_RoundWinner = GetRoundWinner();

        if(m_RoundWinner != null)
        {
            m_RoundWinner.m_Wins++;
        }

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_MessageText.text = message;

        yield return m_EndWait;
    }

    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
            {
                numTanksLeft++;
            }
        }             

        return numTanksLeft <= 1;
    }

    private Tanks GetRoundWinner()
    {
        for(int i = 0; i < m_Tanks.Length; i++)
        {
            if(m_Tanks[i].m_Instance.activeSelf)
            {
                return m_Tanks[i];
            }
        }
        return null;
    }

    private Tanks GetGameWinner()
    {
        for(int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }

    private string EndMessage()
    {
        
        string message = "DRAW!";

        if (m_RoundWinner != null)
        {
            message = m_RoundWinner.m_ColoredPlayerText + "WINS THE ROUND!";
        }

        message += "\n\n\n\n";

        for(int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + "WINS\n";
        }

        if(m_GameWinner != null)
        {
            message = m_GameWinner.m_ColoredPlayerText + "WINS THE GAME!";
        }

        return message;
    }

    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }

    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }

    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnalbeControl();
        }
    }
}
