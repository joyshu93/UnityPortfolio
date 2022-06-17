using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    Text txtCount;
    public int count;
    void Start()
    {
        count = 3;
        txtCount = GetComponent<Text>();
    }

    // Update is called once per frame
    public void ChangeCount()
    {
        count--; //카운트다운
        txtCount.text = (count >= 0) ? "" + count : "GO"; //시작

        if(count < -1)
        {
            if (GameObject.Find("FlappyManager"))
            {
                //장애물 생성
                GameObject.Find("FlappyManager").GetComponent<ObstaclePool>().InitColumnCreate();
                //새 rigidbody 활성화
                GameObject.Find("HeroBird").SendMessage("GameStart");
                //tag로 horzscroll로 지정된 오브젝트를 찾아
                GameObject[] objs = GameObject.FindGameObjectsWithTag("HorzScroll");
                //횡스크롤 시작
                foreach (var o in objs) o.SendMessage("GameStart");
            }
            else if (GameObject.Find("AngryManager"))
            {
                AngryManager AM =  GameObject.Find("AngryManager").GetComponent<AngryManager>();

                AM.Create();
            }
            Destroy(gameObject);
        }


    }
}
