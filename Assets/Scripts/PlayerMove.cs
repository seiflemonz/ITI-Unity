using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Animator anim;
    public float moveSpeed = 4f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public bool isGrounded = true;
    public GameObject win;
    // Respawn position
    private Vector2 respawnPosition = new Vector2(-7.13f, -0.98f);

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!win.active)
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetTrigger("Attack");
            }

            anim.SetBool("isGrounded", isGrounded);

            // 🔻 Fell off the map
            if (transform.position.y < -30f)
            {
                Respawn();
            }


            if (transform.position.x >= 50 && isGrounded)
            {
                win.SetActive(true);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isRun", false);
        }
    }

    void FixedUpdate()
    {
        if (!win.active)
        {
            float moveDirection = Input.GetAxisRaw("Horizontal");

            rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

            rb.rotation = 0f;

            float clampedX = Mathf.Clamp(rb.position.x, -8.5f, 100.0f);
            rb.position = new Vector2(clampedX, rb.position.y);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isRun", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Rope"))
        {
            isGrounded = true;
        }

        // 🔺 Hit spike
        if (collision.gameObject.CompareTag("Spike"))
        {
            Respawn();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Rope"))
        {
            isGrounded = false;
        }
    }
    
    void Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = respawnPosition;
        isGrounded = true;
    }
}
