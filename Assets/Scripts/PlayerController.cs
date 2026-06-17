using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float jumpForce = 8.0f;
    public float airControlForce = 10.0f;
    public float airControlMax = 1.5f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Vector2 moveInput;
    private bool isGrounded;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        CheckGroundStatus();
        ApplyMovement();
    }

    private void CheckGroundStatus()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            // Fallback if groundCheck is not assigned
            isGrounded = true;
        }
    }

    private void ApplyMovement()
    {
        if (isGrounded)
        {
            // Ground movement: Directly set horizontal velocity
            rigidBody.linearVelocity = new Vector2(moveInput.x * speed, rigidBody.linearVelocity.y);
        }
        else
        {
            // Air movement: Apply force for control, but clamp it
            if (Mathf.Abs(moveInput.x) > 0.1f)
            {
                float horizontalForce = moveInput.x * airControlForce;
                rigidBody.AddForce(Vector2.right * horizontalForce);

                // Clamp horizontal velocity in air
                float clampedX = Mathf.Clamp(rigidBody.linearVelocity.x, -speed * airControlMax, speed * airControlMax);
                rigidBody.linearVelocity = new Vector2(clampedX, rigidBody.linearVelocity.y);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
