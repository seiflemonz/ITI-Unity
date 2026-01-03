using UnityEngine;

public class CameraTop : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 8f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
