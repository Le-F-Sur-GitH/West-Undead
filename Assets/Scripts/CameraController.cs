using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // 1. On calcule la nouvelle position de la caméra
        transform.position = target.position + offset;

        // 2. On force la caméra à toujours regarder vers la cible (le cowboy)
        // C'est LA ligne qui va régler ton problème de visibilité !
        transform.LookAt(target);
    }
}