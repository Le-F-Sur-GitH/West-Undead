using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Références Joueur")]
    public PlayerLevel playerLevel;
    public Health playerHealth;

    [Header("Éléments du HUD Principal")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timerText;
    public RectTransform xpBarFillRect;
    public RectTransform xpBarBackgroundRect;
    public RectTransform healthBarFillRect;     // Pour notre nouvelle barre
    public RectTransform healthBarBackgroundRect; // Pour notre nouvelle barre

    void Update()
    {
        if (playerLevel != null)
        {
            UpdateLevelText();
            UpdateXpBar();
        }
        if (playerHealth != null)
        {
            UpdateHealthBar();
        }
        UpdateTimer();
    }

    void UpdateLevelText()
    {
        levelText.text = "Lv. " + playerLevel.currentLevel;
    }

    void UpdateXpBar()
    {
        float barWidth = xpBarBackgroundRect.rect.width;
        float xpPercent = (float)playerLevel.currentXp / playerLevel.xpToNextLevel;
        float targetX = -barWidth + (barWidth * xpPercent);
        xpBarFillRect.anchoredPosition = new Vector2(targetX, 0);
    }

    void UpdateHealthBar()
    {
        if (healthBarFillRect == null) return; // Sécurité
        float barWidth = healthBarBackgroundRect.rect.width;
        float healthPercent = playerHealth.HealthPercent;
        float targetX = -barWidth + (barWidth * healthPercent);
        healthBarFillRect.anchoredPosition = new Vector2(targetX, 0);
    }

    void UpdateTimer()
    {
        float time = Time.timeSinceLevelLoad;
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}