using UnityEngine;
using HEEUNG;

[RequireComponent(typeof(BoxCollider2D))]
public class RepeatBackground : HRepeat
{
    void Start()
    {
        SetBoxCollider();
    }

    void Update()
    {
        UpdateObject();
    }
}
