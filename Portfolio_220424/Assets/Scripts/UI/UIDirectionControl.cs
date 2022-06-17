using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool m_UseRealativeRotation = true;

    private Quaternion m_RelativeRotation;

    private void Start()
    {
        m_RelativeRotation = transform.parent.localRotation;   
    }

    private void Update()
    {
        if(m_UseRealativeRotation)
        {
            transform.rotation = m_RelativeRotation; 
        }
    }
}
