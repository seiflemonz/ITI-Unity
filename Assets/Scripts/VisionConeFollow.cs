using UnityEngine;

public class VisionConeFollow : MonoBehaviour
{
    public Transform player;
    public float visionScale = 2f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (!player) return;

        // Follow position
        transform.position = player.position + offset;

        // Match player rotation (faces right by default)
        transform.rotation = Quaternion.Euler(0, 0, player.eulerAngles.z+90);

        // Scale cone (vision range)
        transform.localScale = new Vector3(visionScale, visionScale, 1f);
    }
}
