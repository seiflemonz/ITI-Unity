using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Animator anim;
    public float moveSpeed = 4f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public bool isGrounded=true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");

        if (moveDirection != 0)
        {
            sr.flipX = moveDirection < 0;
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }


        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            
            anim.SetTrigger("Jump");
        }

        if ((Input.GetKeyDown(KeyCode.E)))
        {
            anim.SetTrigger("Attack");
        }

        anim.SetBool("isGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

        rb.rotation = 0f;

        float clampedX = Mathf.Clamp(rb.position.x, -8.5f, 24.5f);
        rb.position = new Vector2(clampedX, rb.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
