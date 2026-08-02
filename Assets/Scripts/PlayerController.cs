using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Constant forward speed of the player.")]
    public float speed = 9.0f;

    [Tooltip("Force applied upwards when jumping.")]
    public float jumpForce = 12.0f;

    [Header("Physics & Grounding")]
    [Tooltip("Layers representing solid ground.")]
    public LayerMask groundLayer;

    [Tooltip("Radius of the ground check circle.")]
    public float groundCheckRadius = 0.28f;

    [Tooltip("Offset from player's center to check for ground.")]
    public Vector3 groundCheckOffset = new Vector3(0f, -0.5f, 0f);

    [Header("Visual Effects & Rotation")]
    [Tooltip("The visual transform of the cube that rotates (should be a child of the player). If null, this GameObject's transform is used.")]
    public Transform visualModel;

    [Tooltip("Rotation speed in degrees per second while in the air.")]
    public float airRotationSpeed = 360f;

    private Rigidbody2D rigidBody;
    private bool isGrounded;
    private bool jumpActionActive;
    private PlayerInput playerInput;
    private InputAction jumpAction;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // Set up Input Action references if PlayerInput is available
        if (playerInput != null)
        {
            jumpAction = playerInput.actions.FindAction("Jump");
        }
    }

    private void Update()
    {
        // Read input state (supports both Input Action Asset and legacy fallback)
        if (jumpAction != null)
        {
            // Read button value (held or pressed)
            jumpActionActive = jumpAction.IsPressed();
        }
        else
        {
            // Legacy/Keyboard fallback (Space bar, Up Arrow, or Mouse Click)
            jumpActionActive = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(0);
        }

        // Handle mid-air rotation
        if (!isGrounded)
        {
            Transform rotTarget = visualModel != null ? visualModel : transform;
            // Geometry Dash cube rotates backwards (clockwise) while in the air
            rotTarget.Rotate(Vector3.forward, -airRotationSpeed * Time.deltaTime);
        }
        else
        {
            // Smoothly or instantly snap to the nearest 90-degree increment when grounded
            Transform rotTarget = visualModel != null ? visualModel : transform;
            Vector3 currentEuler = rotTarget.eulerAngles;
            float snappedZ = Mathf.Round(currentEuler.z / 90f) * 90f;
            rotTarget.rotation = Quaternion.Euler(currentEuler.x, currentEuler.y, snappedZ);
        }
    }

    private void FixedUpdate()
    {
        // 1. Perform constant horizontal movement (scrolling)
        rigidBody.linearVelocity = new Vector2(speed, rigidBody.linearVelocity.y);

        // 2. Perform ground check
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(transform.position + groundCheckOffset, groundCheckRadius, groundLayer) != null;

        // 3. Handle Jump mechanics (buffer/holding support)
        if (isGrounded && jumpActionActive)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    // Helper to visualize the ground check in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);
    }
}
