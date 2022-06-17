using UnityEngine;

public class HScroll : MonoBehaviour
{
    private Rigidbody2D _rb2d;

    public void SetRigidbody(float speed)
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.bodyType = RigidbodyType2D.Kinematic;
        _rb2d.velocity = new Vector2(-speed, 0f);
    }

    public void SetStop()
    {
        _rb2d.velocity = Vector2.zero;
    }
}

public class HRepeat : MonoBehaviour
{
    private BoxCollider2D _box;
    private float _horizontalLength;

    public void SetBoxCollider()
    {
        _box = GetComponent<BoxCollider2D>();
        _horizontalLength = _box.size.x;
    }

    //개체의 위치값을 파악해서 새로운 위치로의 이동이 필요한지 체크
    public void UpdateObject()
    {
        if (transform.position.x < -_horizontalLength)
        {
            ResetPosition();
        }
    }

    //새로운 위치로 개체를 이동시킴
    private void ResetPosition()
    {
        Vector2 addPos = new Vector2(2 * _horizontalLength, 0f);
        transform.position = (Vector2)transform.position + addPos;
    }
}

