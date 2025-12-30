using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offset")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Follow Settings")]
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Follow ONLY X axis
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            transform.position.y,     
            offset.z               
        );

        // Smooth movement
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
