using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentXp = 0;
    public int xpToNextLevel = 100;

    public void AddXp(int amount)
    {
        currentXp += amount;
        if (currentXp >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentXp -= xpToNextLevel;
        xpToNextLevel = (int)(xpToNextLevel * 2.2f); // On garde la courbe difficile
        Debug.Log("LEVEL UP ! Vous êtes maintenant niveau " + currentLevel);
    }
}