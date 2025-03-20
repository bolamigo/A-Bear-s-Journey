using UnityEngine;

public class BerryIndicator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform BerryIndicatorImage;

    void Update()
    {
        // Si aucun buisson n'est enregistré, on désactive l'indicateur
        if (BerryBush.AllBerryBushes.Count == 0)
        {
            BerryIndicatorImage.gameObject.SetActive(false);
            return;
        }
        else
            BerryIndicatorImage.gameObject.SetActive(true);

        // Recherche du buisson le plus proche
        BerryBush nearestBush = null;
        float minDistance = Mathf.Infinity;
        foreach (var bush in BerryBush.AllBerryBushes)
        {
            float dist = Vector3.Distance(player.position, bush.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearestBush = bush;
            }
        }

        if (nearestBush != null)
        {
            Vector3 direction = nearestBush.transform.position - player.position;
            direction.y = 0; // osef de l'altitude, la map est plate

            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            // +90° pour que l'indicateur pointe vers le haut (avant) par défaut
            BerryIndicatorImage.rotation = Quaternion.Euler(0, 0, angle + 90);
        }
    }
}
