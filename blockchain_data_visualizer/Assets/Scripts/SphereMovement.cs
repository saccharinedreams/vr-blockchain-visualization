using UnityEngine;

public class SphereMovement : MonoBehaviour
{
    private float speed = 5f;
    private float ACCELERATION = 2f;

    void FixedUpdate()
    {
        speed += ACCELERATION * Time.fixedDeltaTime;
        transform.position += new Vector3(speed * Time.fixedDeltaTime, 0f, 0f);
    }
}
