using UnityEngine;

public class BerryIndicator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform BerryIndicatorImage;

    // Seuils pour la distance qui influencent la taille de l'indicateur
    [SerializeField] private float minDistanceThreshold = 8f;

    void Start()
    {
        // Pivot en bas au centre (base de l'image type flèche)
        BerryIndicatorImage.pivot = new Vector2(0.5f, 0f);
    }

    void Update()
    {
        // Aucun buisson, pas besoin d'indicateur
        if (BerryBush.AllBerryBushes.Count == 0)
        {
            BerryIndicatorImage.gameObject.SetActive(false);
            return;
        }

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
            // Indicateur utile seulement si les baies sont assez loin
            if (minDistance < minDistanceThreshold)
            {
                BerryIndicatorImage.gameObject.SetActive(false);
                return;
            }
            BerryIndicatorImage.gameObject.SetActive(true);

            Vector3 direction3D = nearestBush.transform.position - player.position;
            // La map est plate, pas besoin de y
            Vector2 direction = new Vector2(direction3D.x, direction3D.z).normalized;

            // Rotation de 90° pour que l'indicateur pointe vers le haut (avant) par défaut.
            Vector2 direction90 = new Vector2(-direction.y, direction.x);

            float angle = Mathf.Atan2(direction90.y, direction90.x) * Mathf.Rad2Deg;

            BerryIndicatorImage.rotation = Quaternion.Euler(0, 0, angle);

            Canvas canvas = BerryIndicatorImage.GetComponentInParent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            // Origine : centre du canvas
            float halfWidth = canvasRect.rect.width / 2f;
            float halfHeight = canvasRect.rect.height / 2f;
            float radius = Mathf.Min(halfWidth, halfHeight);

            // Décalage de la base de l'indicateur, pour qu'il parte du centre
            Vector2 position = direction90 * (radius / 2f + 64f);
            // Correction pour prendre en compte la largeur (hauteur) de l'image
            position += direction * BerryIndicatorImage.rect.height / 2;

            BerryIndicatorImage.anchoredPosition = position;
        }
    }
}
