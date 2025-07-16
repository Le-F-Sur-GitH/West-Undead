using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public float HealthPercent => (float)currentHealth / maxHealth;
    public GameObject deathLootPrefab;

    // ▼▼▼ LA LIGNE QUI MANQUAIT PROBABLEMENT ▼▼▼
    public GameManager gameManager; // Référence vers notre manager de jeu

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HealToFull()
    {
        currentHealth = maxHealth;
    }

    private void Die()
    {
        if (deathLootPrefab != null)
        {
            Instantiate(deathLootPrefab, transform.position, Quaternion.identity);
        }

        // Si l'objet qui meurt est le joueur...
        if (gameObject.CompareTag("Player"))
        {
            // On prévient le GameManager que la partie est finie
            if (gameManager != null)
            {
                gameManager.OnPlayerDeath();
            }
            // On désactive le joueur
            gameObject.SetActive(false);
        }
        else // Sinon (si c'est un zombie)...
        {
            // On le détruit simplement
            Destroy(gameObject);
        }
    }
}