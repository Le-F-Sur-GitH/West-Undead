using UnityEngine;
using UnityEngine.SceneManagement; // Essentiel pour gérer les scènes

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    // Cette fonction sera appelée par le script Health du joueur quand il meurt.
    public void OnPlayerDeath()
    {
        // On active le panneau "Game Over" pour le rendre visible.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        // On met le jeu en pause.
        Time.timeScale = 0f;
    }

    // Cette fonction sera appelée par notre bouton "Recommencer".
    public void RestartGame()
    {
        // On s'assure de remettre le temps à sa vitesse normale avant de recharger.
        Time.timeScale = 1f;
        // On recharge la scène actuelle, ce qui relance une nouvelle partie.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}