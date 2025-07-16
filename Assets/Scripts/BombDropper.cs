using UnityEngine;
using System.Collections;

public class BombDropper : MonoBehaviour
{
    public GameObject bombPrefab;
    public float dropInterval = 3f;

    // OnEnable est appelée quand le composant est activé
    void OnEnable()
    {
        StartCoroutine(DropBombRoutine());
    }

    IEnumerator DropBombRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(dropInterval);
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
    }
}