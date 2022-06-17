using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DongleManager : MonoBehaviour
{
    [Header("[ Core ]")]
    public int score;
    public int maxLevel; //게임중 현 동글 중 최대 레벨

    [Header("[ Object Pooling ]")]
    public GameObject pDongle;
    public Transform tfDongleGroup; //동글 생성위치
    public List<Dongle> donglePool; //동글 리스트(풀)
    public GameObject pEffect;
    public Transform tfEffectGroup; //이펙트 생성위치
    public List<ParticleSystem> effectPool; //이펙트 리스트(풀)
    [Range(1, 30)]
    public int poolSize;
    public int poolCursor;
    public Dongle lastDongle;

    [Header("[ Audio ]")]
    public AudioSource BGMPlayer; //배경음 플레이어
    public AudioSource[] SFXPlayers; //효과음 플레이어 배열
    public AudioClip[] SFXClips; //효과음 클립 배열
    public enum SFX { LevelUp, Next, Attach, Button, Over };
    int sfxCursor; //효과음 플레이어의 커서

    [Header("[ UI ]")]
    public Text scoreText;
    public Text maxScoreText;

    //게임종료 
    public GameObject GameOverWindow; //결과창
    public bool dongleGameOver;

    void Start()
    {
        //리소스 참조
        pDongle = Resources.Load("Dongle") as GameObject;
        pEffect = Resources.Load("Effect") as GameObject;
        GameOverWindow = Resources.Load("GameOverWindow") as GameObject;

        Application.targetFrameRate = 60;

        //동글리스트, 이펙트리스트 생성
        donglePool = new List<Dongle>();
        effectPool = new List<ParticleSystem>();
        for (int i = 0; i < poolSize; i++)
        {
            MakeDongle();
        }

        BGMPlayer.Play(); //배경음악 시작
        NextDongle();

        maxScoreText.text = AppManager.AM.SM.dongleScore.ToString();
    }

    Dongle MakeDongle()
    {
        //이펙트 생성, 파티클 시스템 참조
        GameObject instantEffectObj = Instantiate(pEffect, tfEffectGroup);
        instantEffectObj.name = "Effect " + effectPool.Count;
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();
        effectPool.Add(instantEffect); //리스트에 저장

        //동글을 생성하고, Dongle 스크립트를 참조 후 반환
        //스크립트에 파티클 시스템 참조
        GameObject instantDongleObj = Instantiate(pDongle, tfDongleGroup); //오브젝트 소환 후 게임오브젝트에 저장
        instantDongleObj.name = "Dongle " + donglePool.Count;
        Dongle instantDongle = instantDongleObj.GetComponent<Dongle>(); //게임오브젝트의 Dongle 속성을 저장
        instantDongle.DM = this;
        instantDongle.effect = instantEffect;
        donglePool.Add(instantDongle); //리스트에 저장

        return instantDongle; //반환
    }

    Dongle GetDongle()
    {
        for(int i = 0; i < donglePool.Count; i++)
        {
            poolCursor = (poolCursor + 1) % donglePool.Count;
            //커서가 가리키는 동글이 활성화되어있지 않다면
            if(!donglePool[poolCursor].gameObject.activeSelf)
            {
                return donglePool[poolCursor];
            }
        }
        //모든 것이 사용중이라면
        return MakeDongle();
    }

    void NextDongle()
    {
        if (dongleGameOver) return; //게임종료시 리턴

        //생성된 동글의 Dongle 스크립트 참조
        lastDongle = GetDongle();

        //생성된 동글 초기화
        lastDongle.level = Random.Range(0, maxLevel);
        lastDongle.gameObject.SetActive(true);

        SFXPlay(SFX.Next);
        StartCoroutine(WaitNext());
    }

    IEnumerator WaitNext()
    {
        while(lastDongle != null)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.5f); //떨구어 지고 2.5초 뒤

        NextDongle();
    }

    public void ClickOn()
    {
        if (lastDongle == null) return;

        lastDongle.Drag();
    }

    public void ClickOff()
    {
        if (lastDongle == null) return;

        lastDongle.Drop();
        lastDongle = null;
    }

    public void GameOver()
    {
        if (dongleGameOver) return; //이미 끝났다면 리턴

        dongleGameOver = true;

        Debug.Log("게임 오버");

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        Dongle[] dongles = GameObject.FindObjectsOfType<Dongle>(); //모든 동글 가져오기

        //지우기 전에 동글들의 물리효과 제거
        for (int i = 0; i < dongles.Length; i++)
        {
            dongles[i].rb2d.simulated = false;
        }

        //동글 목록에 접근하여 지우기
        for (int i = 0; i < dongles.Length; i++)
        {
            if (dongles[i].isUsed == false)
            {
                continue;
            }
            dongles[i].Hide(dongles[i].transform.position, true);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        SFXPlay(SFX.Over);

        
        Instantiate(GameOverWindow).transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    public void SFXPlay(SFX type)
    {
        switch(type)
        {
            case SFX.LevelUp:
                SFXPlayers[sfxCursor].clip = SFXClips[Random.Range(3, 6)];
                break;
            case SFX.Next:
                SFXPlayers[sfxCursor].clip = SFXClips[6];
                break;
            case SFX.Attach:
                SFXPlayers[sfxCursor].clip = SFXClips[0];
                break;
            case SFX.Button:
                SFXPlayers[sfxCursor].clip = SFXClips[1];
                break;
            case SFX.Over:
                SFXPlayers[sfxCursor].clip = SFXClips[2];
                break;
        }

        SFXPlayers[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % SFXPlayers.Length; //0 1 2 순회
    }

    private void LateUpdate()
    {
        scoreText.text = score.ToString();   
    }
}
