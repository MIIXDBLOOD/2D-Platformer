using UnityEngine;
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidBody;
    public float speed = 5.0f;
    public float jumpForce = 8.0f;
    public float airControlForce = 10.0f;
    public float airControlMax = 1.5f;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        if (h != 0.0f)
            rigidBody.linearVelocity = new Vector2(h * speed, 0.0f);
    }
}