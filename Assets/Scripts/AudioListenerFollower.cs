using UnityEngine;

/// <summary>
/// Keep AudioListener at the player's position but with a fixed orientation
/// so rotation of the player doesn't change perceived audio direction.
/// </summary>
[RequireComponent(typeof(AudioListener))]
public class AudioListenerFollower : MonoBehaviour
{
    [Tooltip("The transform (player) whose position the listener should follow.")]
    public Transform target;

    [Tooltip("If true, the listener will face this forward vector (world space).")]
    public Vector3 fixedForward = Vector3.forward;

    [Tooltip("If true, the listener will match the camera's pitch/roll (but not player's yaw).")]
    public bool matchCameraPitchAndRoll = false;

    [Tooltip("Optional camera to copy pitch/roll from if matchCameraPitchAndRoll is true.")]
    public Camera cameraToMatch;

    void LateUpdate()
    {
        if (target != null)
        {
            // Follow position exactly
            transform.position = target.position;
        }

        if (matchCameraPitchAndRoll && cameraToMatch != null)
        {
            // Keep camera pitch/roll, but ignore target/player yaw
            Vector3 camForward = cameraToMatch.transform.forward;
            // Build rotation looking in camera forward while using world up.
            transform.rotation = Quaternion.LookRotation(camForward, Vector3.up);
        }
        else
        {
            // Keep a fixed forward direction in world space (no rotation changes when player rotates)
            transform.rotation = Quaternion.LookRotation(fixedForward, Vector3.up);
        }
    }
}
