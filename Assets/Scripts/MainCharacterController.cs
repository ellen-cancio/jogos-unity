using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;     // Horizontal movement speed
    public float jumpForce = 10f;    // Jump strength
    public Transform groundCheck;    // Empty GameObject at feet
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform visual;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = visual.GetComponent<Animator>();
    }

    void Update()
    {
        // Check if touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Horizontal movement (A/D or Left/Right arrows)
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        anim.SetBool("isrunning", Mathf.Abs(moveInput) > 0f && isGrounded);
        anim.SetBool("isjumping", Mathf.Abs(rb.linearVelocity.y) > 0f && !isGrounded);
        if (moveInput > 0.01f)
        {
            visual.localScale = new Vector3(4, 4, 4);
        }
        else if (moveInput < -0.01f)
        {
            visual.localScale = new Vector3(-4, 4, 4);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
