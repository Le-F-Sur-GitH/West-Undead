using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    [Header("Boutons d'Amélioration")]
    public Button shotgunButton;
    public Button bombButton;
    public Button fireballButton;

    [Header("Références au Joueur")]
    public PlayerLevel playerLevel;
    // ▼▼▼ LA LIGNE CORRIGÉE ▼▼▼
    public Cowboy playerCowboy; // On référence le script Cowboy au lieu de AutoPistol
    public BombDropper bombDropper;

    [Header("Prefabs d'Améliorations")]
    public GameObject fireballPrefab;

    // Flags pour savoir si les améliorations ont été débloquées
    private bool shotgunUnlocked = false;
    private bool bombsUnlocked = false;
    private bool fireballUnlocked = false;

    void Start()
    {
        // On s'assure que tout est bien configuré pour le départ
        shotgunButton.gameObject.SetActive(false);
        bombButton.gameObject.SetActive(false);
        fireballButton.gameObject.SetActive(false);

        shotgunButton.onClick.AddListener(ActivateShotgun);
        bombButton.onClick.AddListener(ActivateBombs);
        fireballButton.onClick.AddListener(ActivateFireball);
    }

    void Update()
    {
        // On vérifie en permanence si une nouvelle amélioration doit être proposée
        if (playerLevel == null) return;

        // Condition pour débloquer le fusil à pompe (Niveau 2)
        if (!shotgunUnlocked && playerLevel.currentLevel >= 2)
        {
            shotgunUnlocked = true;
            shotgunButton.gameObject.SetActive(true);
            shotgunButton.GetComponentInChildren<TextMeshProUGUI>().text = "Activer Fusil à Pompe";
        }

        // Condition pour débloquer les bombes (Niveau 3)
        if (!bombsUnlocked && playerLevel.currentLevel >= 3)
        {
            bombsUnlocked = true;
            bombButton.gameObject.SetActive(true);
            bombButton.GetComponentInChildren<TextMeshProUGUI>().text = "Activer Bombes";
        }

        // Condition pour débloquer la boule de feu (Niveau 4)
        if (!fireballUnlocked && playerLevel.currentLevel >= 4)
        {
            fireballUnlocked = true;
            fireballButton.gameObject.SetActive(true);
            fireballButton.GetComponentInChildren<TextMeshProUGUI>().text = "Activer Boule de Feu";
        }
    }

    void ActivateShotgun()
    {
        // ▼▼▼ LA LOGIQUE CORRIGÉE ▼▼▼
        if (playerCowboy != null) playerCowboy.EnableShotgun(); // On appelle la fonction sur le script Cowboy
        shotgunButton.interactable = false;
        shotgunButton.GetComponentInChildren<TextMeshProUGUI>().text = "Fusil Acquis";
    }

    void ActivateBombs()
    {
        if (bombDropper != null) bombDropper.enabled = true;
        bombButton.interactable = false;
        bombButton.GetComponentInChildren<TextMeshProUGUI>().text = "Bombes Acquises";
    }

    void ActivateFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, playerLevel.transform.position + new Vector3(3, 0, 0), Quaternion.identity);
        fireball.GetComponent<OrbitingFireball>().targetToOrbit = playerLevel.transform;
        fireballButton.interactable = false;
        fireballButton.GetComponentInChildren<TextMeshProUGUI>().text = "Boule de Feu Acquise";
    }
}