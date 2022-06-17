using UnityEngine;
using UnityEngine.UI;

public class OthelloComTurn : MonoBehaviour
{

    OthelloManager OthM;
    Slider slider;

    string title;
    private void Awake()
    {
        OthM = GameObject.Find("OthelloManager").GetComponent<OthelloManager>();

        slider = this.GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value += Time.deltaTime;
        if (slider.value == 1)
        {
            OthM.bComMakeStone = true;
            Destroy(gameObject);
        }
    }
}
