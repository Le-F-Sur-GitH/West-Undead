using UnityEngine;

public class XpOrb : MonoBehaviour
{
    private Transform playerTransform;
    private bool isMoving = false;

    public float moveSpeed = 8f;
    public float detectionRadius = 5f;
    public int xpValue = 20;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < detectionRadius)
        {
            isMoving = true;
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerLevel>()?.AddXp(xpValue);
            Destroy(gameObject);
        }
    }
}