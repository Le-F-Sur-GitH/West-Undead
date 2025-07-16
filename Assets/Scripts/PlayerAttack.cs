using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int clickDamage = 50;

    void Update()
    {
        // Si on appuie sur le bouton gauche de la souris (0)
        if (Input.GetMouseButtonDown(0))
        {
            // On crée un rayon qui part de la caméra et passe par le curseur de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // On lance ce rayon dans la scène
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Si le rayon touche un objet qui a le tag "Zombie"
                if (hit.collider.CompareTag("Zombie"))
                {
                    Debug.Log("Touche un zombie !");
                    // On récupère son script Health et on lui fait des dégâts
                    hit.collider.GetComponent<Health>()?.TakeDamage(clickDamage);
                }
            }
        }
    }
}