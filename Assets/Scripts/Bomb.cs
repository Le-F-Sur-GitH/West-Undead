using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [Header("Paramètres de la Bombe")]
    public float delay = 2.5f;
    public float explosionRadius = 7f;
    public int damage = 60;
    public GameObject explosionEffectPrefab; // <-- C'EST CETTE LIGNE QUI MANQUAIT

    void Start()
    {
        StartCoroutine(ExplosionCoroutine());
    }

    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    void Explode()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Zombie"))
            {
                hit.GetComponent<Health>()?.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}