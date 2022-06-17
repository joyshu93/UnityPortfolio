using UnityEngine;
using UnityEngine.UI;

public class OmokComTurn : MonoBehaviour
{
    OmokManager OM;
    Slider slider;

    private void Awake()
    {
        OM = GameObject.Find("OmokManager").GetComponent<OmokManager>();

        slider = this.GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value += Time.deltaTime;
        if (slider.value == 1)
        {
           OM.bComMakeStone = true;
            Destroy(gameObject);
        }
    }
}
