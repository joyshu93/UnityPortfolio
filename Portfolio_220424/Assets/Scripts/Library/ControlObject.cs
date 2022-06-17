using UnityEngine;

namespace HEEUNG
{
    public class Mouse : MonoBehaviour
    {
        public Vector3 mouseWorldPoint;

        float _maxLength = 2.5f; //전방위 최대 제한거리

        //마우스 월드포인터 계산
        public void GetMouseWorldPoint()
        {
            mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        //마우스 포인터에 물체붙이기(고정(물체)이 필요한 x, y값, 속도)
        public void SetObjectToMouse(float _fixX, float _fixY, float _speed)
        {
            mouseWorldPoint.x = _fixX;
            mouseWorldPoint.y = _fixY;
            mouseWorldPoint.z = 0f;
            transform.position = Vector3.Lerp(transform.position, mouseWorldPoint, _speed);
        }

        //마우스(붙어있는 오브젝트) 이동 범위 제한
        public void SetLimitMouseMoving(float _left, float _right, Transform _tf)
        {
            float leftBorder = _left + _tf.localScale.x / 2f;
            float rightBorder = _right - _tf.localScale.x / 2f;

            if (mouseWorldPoint.x < leftBorder)
            {
                mouseWorldPoint.x = leftBorder;
            }
            else if (mouseWorldPoint.x > rightBorder)
            {
                mouseWorldPoint.x = rightBorder;
            }
        }

        //클릭 지점으로 부터 마우스(붙어있는 오브젝트) 움직임 제한
        public void SetLimitDistance(Ray _ray, Transform _Point)
        {
            Vector2 _newVector = mouseWorldPoint - _Point.position; //새 벡터
            if (_newVector.sqrMagnitude > _maxLength * _maxLength) //제한거리보다 멀리있음
            {
                _ray.direction = _newVector; //ray 지정
                mouseWorldPoint = _ray.GetPoint(_maxLength); //제한거리 위치 얻음
            }
        }
    }

    public class Keyboard : MonoBehaviour
    {
        public void ArrowKeysMoving(float _speed)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 curPos = transform.position;
            Vector3 nextPos = new Vector3(h, v, 0) * _speed * Time.deltaTime;

            transform.position = curPos + nextPos;
        }

        public void SetLimitKeyboardMoving(float _left, float _right, float _up, float _down, Transform _tf)
        {
            float leftBorder = _left + _tf.localScale.x / 2f;
            float rightBorder = _right - _tf.localScale.x / 2f;

            float upBorder = _up - _tf.localScale.y / 2f;
            float downBorder = _down + _tf.localScale.y / 2f;

            if (_tf.position.x < leftBorder)
            {
                _tf.position = new Vector3(leftBorder, _tf.position.y, 0f);
            }
            else if (_tf.position.x > rightBorder)
            {
                _tf.position = new Vector3(rightBorder, _tf.position.y, 0f);
            }

            if (_tf.position.y > upBorder)
            {
                _tf.position = new Vector3(_tf.position.x, upBorder, 0f);
            }
            else if (_tf.position.y < downBorder)
            {
                _tf.position = new Vector3(_tf.position.x, downBorder, 0f);
            }
        }
    }
}