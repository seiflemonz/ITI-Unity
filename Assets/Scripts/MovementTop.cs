using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class TopDownPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Health")]
    public int maxHealth = 100;
    public float currentHealth;

    [Header("Shooting")]
    public GameObject bulletPrefab;      // assign your orange ball prefab here
    public float bulletSpeed = 10f;      // how fast the bullet moves
    public Transform firePoint;          // empty GameObject at player front
    public AudioClip gunshotClip;        // assign gunshot sound here

    [Header("Audio")]
    public AudioSource audioSource;      // assign AudioSource component (on Player)

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 movement;
    private float lastRotation;


    public GameObject healthBar;
    public GameObject youDied;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Setup AudioSource if not assigned
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth; // start full health
        lastRotation = rb.rotation;
    }

    void Update()
    {
        // --- Movement ---
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        bool isMoving = movement.sqrMagnitude > 0.01f;
        anim.SetBool("isMove", isMoving);

        if (isMoving)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            lastRotation = angle;
        }
        else
        {
            rb.rotation = lastRotation;
        }

        // --- Shooting ---
        if (Input.GetMouseButtonDown(0)) // left click
        {
            Shoot();
        }

        // --- Health Check ---
        if (currentHealth <= 0)
        {
            Die();
        }

        healthBar.GetComponent<RectTransform>().localScale = new Vector3((currentHealth / 100.0f) * 5.0f,1,1);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // --- Shooting Method ---
    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Instantiate bullet at firePoint position with player rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, rb.rotation));

        // Give it velocity in the direction player is facing
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            Vector2 direction = new Vector2(Mathf.Cos(rb.rotation * Mathf.Deg2Rad), Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
            bulletRb.linearVelocity = direction * bulletSpeed;
        }

        // Play gunshot sound
        if (gunshotClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(gunshotClip, 1f);
        }
    }

    // --- Health Methods ---
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0); // clamp to 0
        Debug.Log($"Player took {amount} damage. Health: {currentHealth}");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // clamp to maxHealth
        Debug.Log($"Player healed {amount}. Health: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("Player died!");
        this.enabled = false;
        youDied.SetActive(true);
        healthBar.GetComponent<RectTransform>().localScale = new Vector3((currentHealth / 100.0f) * 5.0f, 1, 1);
        // Optionally: play death animation, disable sprite, etc.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Health"))
        {
            Heal(5);
            Destroy(other.gameObject);
        }
    }

}
