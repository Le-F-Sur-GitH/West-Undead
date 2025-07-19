using UnityEngine;
using UnityEngine.UI;

// Ce script doit être placé sur l'objet Slider de la barre de vie.
[RequireComponent(typeof(Slider))]
public class HealthBarFollow : MonoBehaviour
{
    [Tooltip("Faites glisser ici le GameObject du Cowboy que la barre doit suivre.")]
    public Transform targetToFollow;

    [Tooltip("Ajuste la position de la barre par rapport à la tête du personnage.")]
    public Vector3 offset = new Vector3(0, 2f, 0);

    private Slider healthSlider;
    private Health targetHealth;
    private Transform cameraTransform;

    void Awake()
    {
        healthSlider = GetComponent<Slider>();
        cameraTransform = Camera.main.transform;

        if (targetToFollow != null)
        {
            targetHealth = targetToFollow.GetComponent<Health>();
        }
    }

    // On utilise LateUpdate pour que la barre se mette à jour APRES que le cowboy ait bougé.
    // Cela évite les saccades.
    void LateUpdate()
    {
        if (targetToFollow == null || targetHealth == null)
        {
            // Si on n'a pas de cible, on cache la barre.
            gameObject.SetActive(false);
            return;
        }

        // Met à jour la valeur de la barre de vie grâce à votre propriété HealthPercent
        healthSlider.value = targetHealth.HealthPercent;

        // Place la barre à la position de la cible + le décalage (offset)
        transform.position = targetToFollow.position + offset;

        // Fait en sorte que la barre fasse toujours face à la caméra
        transform.rotation = cameraTransform.rotation;
    }
}