using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cowboy : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float moveSpeed     = 5f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Combat")]
    [SerializeField] private float     attackRange   = 10f;
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Shotgun")]
    [SerializeField] private float fireRate       = 2f;
    [SerializeField] private int   shotgunPellets = 5;
    [SerializeField] private float shotgunSpread  = 15f;

    [Header("Animation Overrides")]
    [SerializeField] private RuntimeAnimatorController pistolAOC;
    [SerializeField] private RuntimeAnimatorController rifleAOC;

    // --- Variables internes ---
    private Rigidbody rb;
    private Animator  animator;
    private Transform cam;
    private Vector3   moveDir;
    private Transform currentTarget;
    private float     nextFireTime;
    private bool      shotgunEnabled;

    private void Awake()
    {
        rb       = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        cam      = Camera.main.transform;

        // ▼▼▼ LE RÉGLAGE DÉFINITIF ▼▼▼
        // On s'assure que le composant qui lâche les bombes est bien DÉSACTIVÉ au démarrage.
        // Peu importe comment il est réglé dans l'inspecteur, ce code aura le dernier mot.
        BombDropper bombDropper = GetComponent<BombDropper>();
        if (bombDropper != null)
        {
            bombDropper.enabled = false;
        }
    }

    private void Start()
    {
        // On démarre toujours avec le pistolet
        if (animator != null && pistolAOC != null)
            animator.runtimeAnimatorController = pistolAOC;
    }

    private void Update()
    {
        HandleInputs();
        HandleCombat();
    }

    private void FixedUpdate()
    {
        MoveAndRotate();
    }

    private void HandleInputs()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 f = cam.forward; Vector3 r = cam.right;
        f.y = r.y = 0f;
        moveDir = (f.normalized * v + r.normalized * h).normalized;

        animator?.SetFloat("Speed", moveDir.magnitude, 0.05f, Time.deltaTime);
    }

    // Gère le déplacement et la rotation, en donnant la priorité à la visée.
    private void MoveAndRotate()
    {
        if (moveDir.sqrMagnitude > 0.001f)
        {
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        }

        // La rotation avec le clavier est ignorée si une cible est acquise.
        if (currentTarget == null && moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion target = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, target, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void HandleCombat()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, zombieLayer);
        Transform best  = null;
        float     bestSqr = Mathf.Infinity;

        foreach (var col in hits)
        {
            float d = (col.transform.position - transform.position).sqrMagnitude;
            if (d < bestSqr)
            {
                bestSqr = d;
                best    = col.transform;
            }
        }
        currentTarget = best;

        if (currentTarget != null && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            AimAndShoot(currentTarget);
        }
    }

    private void AimAndShoot(Transform target)
    {
        // Pivotage vers la cible
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
            rb.MoveRotation(Quaternion.LookRotation(dir));

        // Instantiation des projectiles
        if (shotgunEnabled)
        {
            for (int i = 0; i < shotgunPellets; i++)
            {
                float angle = Random.Range(-shotgunSpread, shotgunSpread);
                Quaternion spreadRot = firePoint.rotation * Quaternion.Euler(0f, angle, 0f);
                Instantiate(bulletPrefab, firePoint.position, spreadRot);
            }
        }
        else
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void EnableShotgun()
    {
        if (shotgunEnabled) return;
        shotgunEnabled = true;
        fireRate *= 1.2f;

        if (animator != null && rifleAOC != null)
            animator.runtimeAnimatorController = rifleAOC;
    }
}