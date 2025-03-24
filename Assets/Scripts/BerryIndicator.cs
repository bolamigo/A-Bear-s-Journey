using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BerryIndicatorManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    // Prefab de l'indicateur (contenant un RectTransform et une Image)
    [SerializeField] private RectTransform indicatorPrefab;

    // Seuils de distance
    [SerializeField] private float minDistanceThreshold = 16f;
    [SerializeField] private float maxDistanceThreshold = 32f;


    // Gestionnaire interne des indicateurs : chaque buisson aura son indicateur
    private Dictionary<BerryBush, RectTransform> indicators = new Dictionary<BerryBush, RectTransform>();

    void Update()
    {
        // Parcours de tous les buissons enregistrés
        foreach (var bush in BerryBush.AllBerryBushes)
        {
            float dist = Vector3.Distance(player.position, bush.transform.position);

            // Si le buisson est trop proche (les baies sont visibles) ou trop éloigné (Ted ne sent pas), on retire l'indicateur
            if (dist > maxDistanceThreshold)
            {
                if (indicators.ContainsKey(bush))
                {
                    Destroy(indicators[bush].gameObject);
                    indicators.Remove(bush);
                }
            }
            else
            {
                // Si le buisson est à portée, on s'assure d'avoir un indicateur
                RectTransform indicator;
                if (!indicators.TryGetValue(bush, out indicator))
                {
                    indicator = Instantiate(indicatorPrefab, transform);
                    // Le pivot est en bas au centre pour que la base de la flèche soit fixe
                    indicator.pivot = new Vector2(0.5f, 0f);
                    indicators.Add(bush, indicator);
                }

                // Calcul de la direction depuis le joueur vers le buisson (projection sur le plan XZ)
                Vector3 direction3D = bush.transform.position - player.position;
                direction3D.y = 0;
                Vector2 direction = new Vector2(direction3D.x, direction3D.z).normalized;

                // Rotation : on fait tourner la direction de 90° pour que l'image (si elle pointe par défaut vers le haut)
                // indique correctement la direction du buisson.
                Vector2 direction90 = new Vector2(-direction.y, direction.x);
                float angle = Mathf.Atan2(direction90.y, direction90.x) * Mathf.Rad2Deg;
                indicator.rotation = Quaternion.Euler(0, 0, angle);

                // Positionnement de l'indicateur sur le Canvas :
                // On place l'indicateur de façon à ce que sa base (pivot) soit décalée depuis le centre.
                Canvas canvas = indicator.GetComponentInParent<Canvas>();
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                float halfWidth = canvasRect.rect.width / 2f;
                float halfHeight = canvasRect.rect.height / 2f;
                float radius = Mathf.Min(halfWidth, halfHeight);

                // La position est calculée de manière similaire à vos tests,
                // ici on déplace l'indicateur depuis le centre, en ajoutant un décalage fixe
                // et une correction pour la hauteur de l'image.
                Vector2 pos = direction90 * (radius / 2f + 64f);
                pos += direction * (indicator.rect.height / 2f);
                indicator.anchoredPosition = pos;

                // Calcul de l'opacité en fonction de la distance :
                // Plus le buisson est proche (distance proche de minDistanceThreshold), plus l'indicateur est opaque.
                float alpha = 1f - Mathf.InverseLerp(minDistanceThreshold, maxDistanceThreshold, dist);

                // Appliquer l'opacité à l'image de l'indicateur
                RawImage img = indicator.GetComponent<RawImage>();
                if (img != null)
                {
                    Color c = img.color;
                    c.a = alpha;
                    img.color = c;
                }

                // Ajustement de l'échelle : 
                // Si la distance est inférieure à minDistanceThreshold, l'échelle est interpolée entre minIndicatorScale et 1.
                // Pour une distance >= minDistanceThreshold, on utilise l'échelle 1.
                float scaleFactor = 1f;
                if (dist < minDistanceThreshold)
                {
                    scaleFactor = Mathf.Lerp(0.05f, 1f, dist / minDistanceThreshold);
                }
                indicator.localScale = Vector3.one * scaleFactor;
            }
        }

        // Nettoyage des indicateurs pour les buissons qui n'existent plus
        List<BerryBush> keys = new List<BerryBush>(indicators.Keys);
        foreach (var bush in keys)
        {
            if (!BerryBush.AllBerryBushes.Contains(bush))
            {
                Destroy(indicators[bush].gameObject);
                indicators.Remove(bush);
            }
        }
    }
}
