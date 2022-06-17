using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    GameObject Event;
    Image imgFade;
    bool bFading; //페이딩 진행중

    void Awake()
    {      
        imgFade = this.GetComponent<Image>();
        imgFade.raycastTarget = false; //클릭 투과
        bFading = false;

        SetFade("Sign");
    }

    public void SetFade(string _scene)
    {
        if (bFading) return;

        Event = GameObject.Find("EventSystem");

        Event.SetActive(false);
        bFading = true;
        StartCoroutine(Fade(_scene));
    }

   IEnumerator Fade(string _scene)
    {
        float fInit = 0f;

        //페이드 아웃
        while (fInit < 1.0f)
        {
            fInit += 0.04f;
            yield return new WaitForSeconds(0.01f);
            imgFade.color = new Color(0, 0, 0, fInit);
        }

        Event.SetActive(true);
        //씬 변환
        SceneManager.LoadScene(_scene);

        //페이드 인
        while (fInit > 0f)
        {
            fInit -= 0.04f;
            yield return new WaitForSeconds(0.01f);
            imgFade.color = new Color(0, 0, 0, fInit);       
        }

        bFading = false;
    }
}
