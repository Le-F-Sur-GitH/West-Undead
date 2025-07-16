using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;

    public int attackDamage = 5;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        if (target == null || agent == null) return;

        // L'ordre de trouver une destination reste le même
        agent.SetDestination(target.position);

        // --- LA CORRECTION EST ICI ---
        // On ne vérifie la distance que si l'agent a un chemin valide (hasPath)
        // et qu'il n'est pas en train d'en calculer un nouveau (pathPending).
        if (agent.hasPath && !agent.pathPending)
        {
            // Si la condition de sécurité est passée, alors on peut vérifier la distance en toute confiance.
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    void Attack()
    {
        // On peut enlever les Debug.LogWarning maintenant que le problème est identifié
        target.GetComponent<Health>()?.TakeDamage(attackDamage);
    }
}