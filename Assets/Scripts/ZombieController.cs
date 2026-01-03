using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class ZombieController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int damage = 10;

    [Header("Voice Line")]
    public AudioClip voiceClip;        // Assign your zombie voice line
    public float voiceInterval = 3f;   // seconds between lines

    private Transform player;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private float voiceTimer;

    [Header("Drop Settings")]
    public GameObject healthPickupPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.playOnAwake = false;
        voiceTimer = 0f;
    }

    void Start()
    {
        // Find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        // --- Voice Line Timer ---
        voiceTimer += Time.deltaTime;
        if (voiceTimer >= voiceInterval)
        {
            PlayVoiceLine();
            voiceTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Direction to player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move zombie
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        // Rotate to face player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Deal damage if colliding with player
        if (collision.collider.CompareTag("Player"))
        {
            TopDownPlayerController playerController = collision.collider.GetComponent<TopDownPlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
                Debug.Log("Zombie hit player for " + damage);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TryDropHealth();

            Destroy(collision.gameObject);
            Destroy(gameObject);

            Debug.Log("Zombie hit by bullet!");
        }
    }
    void TryDropHealth()
    {
        if (healthPickupPrefab == null) return;

        if (Random.value <= dropChance)
        {
            Instantiate(
                healthPickupPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }


    private void PlayVoiceLine()
    {
        if (voiceClip != null)
        {
            audioSource.PlayOneShot(voiceClip);
        }
    }
}
