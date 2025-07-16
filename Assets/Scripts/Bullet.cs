using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public int damage = 34; // On met les dégâts ici maintenant

    void Start()
    {
        // On donne une vitesse initiale à la balle pour la propulser en avant
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
        // On s'assure que la balle se détruit après 2 secondes si elle ne touche rien
        Destroy(gameObject, 2f);
    }

    // Cette fonction se déclenche quand la balle entre en contact avec un autre trigger
    void OnTriggerEnter(Collider other)
    {
        // Si l'objet touché est un zombie...
        if (other.CompareTag("Zombie"))
        {
            // On lui inflige des dégâts...
            other.GetComponent<Health>()?.TakeDamage(damage);
            // Et on détruit la balle immédiatement.
            Destroy(gameObject);
        }
    }
}