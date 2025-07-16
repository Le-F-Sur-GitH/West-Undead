using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 3.0f;
    public PlayerLevel playerLevel;

    [Header("Taille de la Zone de Spawn")]
    public float spawnAreaX = 90f; // Un peu plus petit que votre sol de 100 pour éviter les bords
    public float spawnAreaZ = 90f;

    void Start()
    {
        if (playerLevel != null)
        {
            StartCoroutine(SpawnWaveRoutine());
        }
        else
        {
            Debug.LogError("La référence PlayerLevel n'est pas assignée sur le ZombieSpawner !");
        }
    }

    IEnumerator SpawnWaveRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            int currentWaveSize = (playerLevel != null) ? (2 + playerLevel.currentLevel - 1) : 2;

            for (int i = 0; i < currentWaveSize; i++)
            {
                TrySpawnZombie();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void TrySpawnZombie()
    {
        // 1. On choisit des coordonnées X et Z au hasard dans les limites de l'arène
        float randomX = Random.Range(-spawnAreaX / 2, spawnAreaX / 2);
        float randomZ = Random.Range(-spawnAreaZ / 2, spawnAreaZ / 2);

        // On se place haut dans le ciel à ces coordonnées
        Vector3 rayStartPoint = new Vector3(randomX, 100f, randomZ);

        // 2. On lance un rayon vers le bas (sur une distance de 200m)
        RaycastHit hit;
        if (Physics.Raycast(rayStartPoint, Vector3.down, out hit, 200f))
        {
            // 3. Si on a touché quelque chose (normalement le sol), on crée le zombie à l'endroit de l'impact
            Instantiate(zombiePrefab, hit.point, Quaternion.identity);
        }
        else
        {
            // Ce message ne devrait apparaître que si votre sol n'a pas de collider
            Debug.LogWarning("Le Raycast de spawn n'a touché aucun sol à la position " + rayStartPoint);
        }
    }
}