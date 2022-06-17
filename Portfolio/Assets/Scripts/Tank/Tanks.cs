using System;
using UnityEngine;

//공통 속성을 가진 변수들을 그룹지어 클래스를 만들고, Serializable 속성을 부여한다
//생성한 클래스의 변수들을 모두 public으로 선언한다
//다른 스크립트에서 시작시 초기화할 필요가없다
//변수 선언시 초기화 값을 지정할 수 있다.
[Serializable]
public class Tanks
{
    public Color m_PlayerColor;
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance; //인스턴스 게임오브젝트
    [HideInInspector] public int m_Wins;
    [HideInInspector] public Camera m_Camera;
    [HideInInspector] public Canvas m_FirstPersonCanvas;

    private TankMovement m_Movement;
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;

    public void Setup()
    {
        m_FirstPersonCanvas = GameObject.Find("FirstPersonCanvas").GetComponent<Canvas>();
        m_Camera = m_Instance.GetComponentInChildren<Camera>();
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        if(m_PlayerNumber < 99)
        {
            m_Movement.m_PlayerNumber = m_PlayerNumber;
            m_Shooting.m_PlayerNumber = m_PlayerNumber;
        }
       
        if(m_PlayerNumber < 99)
        {
            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER" + m_PlayerNumber + "</color>";
        }
        else
        {
           m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">COM" + "</color>";
        }

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for(int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }

    public void DisableControl()
    {
        if (m_PlayerNumber < 99)
        {
            m_Movement.enabled = false;
            m_Shooting.enabled = false;
        }

        m_CanvasGameObject.SetActive(false);
    }

    public void EnalbeControl()
    {
        if (m_PlayerNumber < 99)
        {
            m_Movement.enabled = true;
            m_Shooting.enabled = true;
        }

        m_CanvasGameObject.SetActive(true);
    }

    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;


        m_Instance.SetActive(false);
        m_Instance.SetActive(true);

    }

}
