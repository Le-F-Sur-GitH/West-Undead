using UnityEngine;

public class AutoPistol : MonoBehaviour
{
    [Header("Configuration de l'Arme")]
    public float fireRate = 2f;
    public float attackRange = 8f;
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Statistiques Actuelles")]
    public bool shotgunEnabled = false;
    public int shotgunPelletCount = 5;
    public float shotgunSpreadAngle = 15f;

    private Transform target;
    private float nextFireTime = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void EnableShotgun()
    {
        if (shotgunEnabled) return;
        shotgunEnabled = true;
        fireRate *= 0.8f;
        
        if(animator != null)
        {
            animator.SetBool("HasShotgun", true);
        }
    }
    
    void Update()
    {
        FindClosestZombie();
        
        if (target != null)
        {
            // On réactive la visée automatique ici
            Vector3 targetPositionOnPlane = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(targetPositionOnPlane);

            // On vérifie s'il est temps de tirer
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Shoot();
            }
        }
    }

    void FindClosestZombie()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        Transform closestZombie = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject zombie in zombies)
        {
            float distance = Vector3.Distance(transform.position, zombie.transform.position);
            if (distance < minDistance && distance <= attackRange)
            {
                minDistance = distance;
                closestZombie = zombie.transform;
            }
        }
        target = closestZombie;
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        if (shotgunEnabled)
        {
            for (int i = 0; i < shotgunPelletCount; i++)
            {
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle), 0);
                Quaternion bulletRotation = firePoint.rotation * randomRotation;
                Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            }
        }
        else
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}