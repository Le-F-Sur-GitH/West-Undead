using UnityEngine;

public class OrbitingFireball : MonoBehaviour
{
    public Transform targetToOrbit; // Le joueur autour duquel tourner
    public float orbitSpeed = 150f; // Vitesse de rotation en degrés par seconde
    public float orbitDistance = 2.5f; // La distance constante par rapport au joueur
    public int damage = 15;
    public float damageTickRate = 0.5f; // Dégâts toutes les 0.5s
    
    private float nextDamageTime;
    private float currentAngle = 0f; // On garde en mémoire l'angle actuel de l'orbite

    void Update()
    {
        if (targetToOrbit == null)
        {
            // Si la cible (le joueur) est détruite, la boule de feu se détruit aussi
            Destroy(gameObject);
            return;
        }

        // --- NOUVELLE LOGIQUE D'ORBITE ---

        // 1. On augmente l'angle en fonction de la vitesse et du temps écoulé
        currentAngle += orbitSpeed * Time.deltaTime;
        if (currentAngle > 360f)
        {
            currentAngle -= 360f; // On remet l'angle entre 0 et 360 pour éviter les trop grands nombres
        }

        // 2. On calcule le décalage horizontal (x) et en profondeur (z) grâce à la trigonométrie (cosinus et sinus)
        float offsetX = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitDistance;
        float offsetZ = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitDistance;
        
        // 3. On calcule la nouvelle position de la boule de feu : la position du joueur + notre décalage
        Vector3 newPosition = targetToOrbit.position + new Vector3(offsetX, 0, offsetZ);

        // 4. On applique cette position directement. C'est la méthode la plus stable.
        transform.position = newPosition;
    }

    // La logique de dégâts ne change pas
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Zombie") && Time.time > nextDamageTime)
        {
            nextDamageTime = Time.time + damageTickRate;
            other.GetComponent<Health>()?.TakeDamage(damage);
        }
    }
}