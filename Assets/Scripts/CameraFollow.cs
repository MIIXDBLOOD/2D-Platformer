using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("The target transform the camera should follow (usually the player).")]
    public Transform target;

    [Tooltip("Horizontal offset from the target player position.")]
    public float xOffset = 3.0f;

    [Tooltip("Vertical offset from the target player position.")]
    public float yOffset = 1.0f;

    [Tooltip("Smoothness speed of the camera vertical translation.")]
    public float smoothSpeed = 5.0f;

    private void LateUpdate()
    {
        if (target == null) return;

        // Keep the horizontal position perfectly aligned with the target's position + offset,
        // which prevents any visual stuttering of the fast-moving player.
        float targetX = target.position.x + xOffset;

        // Smoothly follow vertically so jump fluctuations don't shake the camera violently
        float targetY = Mathf.Lerp(transform.position.y, target.position.y + yOffset, smoothSpeed * Time.deltaTime);

        // Retain current Z depth
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
