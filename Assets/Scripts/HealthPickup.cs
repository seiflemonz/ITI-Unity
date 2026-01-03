using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TopDownPlayerController player = collision.GetComponent<TopDownPlayerController>();
            if (player != null)
            {
                player.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
