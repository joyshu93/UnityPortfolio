using UnityEngine;

namespace Practice
{
    public class Move
    {
        public int horizontalDirection = 0; //수평방향
        float speed = 10f;

        public void Moving(Transform tf)
        {
            float h = Input.GetAxisRaw("Horizontal");
            GetHorizontalDirection(h);
            float v = Input.GetAxisRaw("Vertical");
            Vector3 curPos = tf.position;
            Vector3 nextPos = new Vector3(h, v, 0f) * speed * Time.deltaTime;

            tf.position = curPos + nextPos;
        }

        public void GetHorizontalDirection(float tmp)
        {
            if (tmp > 0) horizontalDirection = 1;
            else if (tmp < 0) horizontalDirection = - 1;
            else horizontalDirection = 0;
        }

    }

}