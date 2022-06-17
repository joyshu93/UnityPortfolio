using UnityEngine;

namespace ManageMent
{
    //abstract method를 가지는 클래스는 명시적으로 abstract(추상) 클래스가 되어야 한다
    public abstract class Manage : MonoBehaviour
    {

        //자식 클래스 override 사용할 목적
        protected virtual void Awake()
        {
            
        }

        public GameObject InstantiateUI(string prefabName, string canvasName, bool isFull)
        {
            GameObject resource = Resources.Load(prefabName) as GameObject;
            GameObject obj = Instantiate(resource, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(GameObject.Find(canvasName).transform);

            if (isFull)
            {
                ((RectTransform)obj.transform).offsetMax = Vector2.zero; //가득 채우도록 배치
            }
            else
            {
                ((RectTransform)obj.transform).anchoredPosition = Vector2.zero; //캔버스 중앙 배치
            }

            return obj;
        }

        //추상 함수이다.
        //자식 클래스에서 반드시 재정의를 해서 사용하도록 강제화 시킨다.
        public abstract void SetStart();
    }


}
