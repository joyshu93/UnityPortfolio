using UnityEngine;

public class AboutCamera
{
    Transform tfPlayer; //플레이어 위치

    //이동
    float smoothing; //속도

    //카메라 제한
    float height;
    float width;
    Vector2 center; //중앙
    Vector2 size; //범위

    //플레이어 추적하는 카메라
    public void SetToPlayer(Camera _cam)
    {
        tfPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        _cam.transform.position = new Vector3(tfPlayer.position.x, tfPlayer.position.y, -10f);
    }

    //오브젝트 추적하는 카메라의 부드러운 이동(목표좌표, 카메라)
    public void SetSmoothMoving(Vector3 offset, Camera _cam)
    {
        smoothing = 2f;
        offset.z = -10f;
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, offset, smoothing * Time.deltaTime);
    }

    //카메라 제한범위 설정
    public void SetLimitStep1(Vector2 _center, Vector2 _size)
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.width;

        center = _center;
        size = _size;
    }
    public void SetLimitStep2(Camera _cam)
    {
        float lx = size.x * 0.5f - width;
        float clampX = Mathf.Clamp(_cam.transform.position.x, -lx + center.x, lx + center.x);

        float ly = size.y * 0.5f - height;
        float clampY = Mathf.Clamp(_cam.transform.position.y, -ly + center.y, ly + center.y);

        _cam.transform.position = new Vector3(clampX, clampY, -10f);
    }

}