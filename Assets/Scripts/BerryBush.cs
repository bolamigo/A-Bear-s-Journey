using UnityEngine;
using System.Collections;
using static Enums;

public class BerryBush : Resource
{
    [SerializeField] private float respawnDelay = 10f;
    // Référence au prefab du marker de la baie à afficher sur la map
    [SerializeField] private GameObject mapMarkerPrefab;
    // Instance du marker instancié
    private GameObject mapMarkerInstance;
    // Hauteur fixe pour l'affichage sur la map (comme dans FollowActorOnMap)
    private const float BerriesHeightOnMap = 125f;

    private void Start()
    {
        if (mapMarkerPrefab != null)
        {
            mapMarkerInstance = Instantiate(mapMarkerPrefab, GetMapPosition(), Quaternion.identity);
            mapMarkerInstance.transform.SetParent(GameObject.Find("Berries").transform);

            FollowBerryOnMap followScript = mapMarkerInstance.GetComponent<FollowBerryOnMap>();

            // Pour éviter les erreurs de FollowActorOnMap
            if (followScript != null)
                followScript.setActor(transform);
        }
    }

    private Vector3 GetMapPosition()
    {
        // Position du buisson sur la map
        return new Vector3(transform.position.x, BerriesHeightOnMap, transform.position.z);
    }

    private void Update()
    {
        // Met à jour la position du marker pour suivre le buisson
        if (mapMarkerInstance != null)
        {
            mapMarkerInstance.transform.position = GetMapPosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GetComponent<Renderer>().enabled)
        {
            // Incrémente le compteur de baies via le GameManager
            GameManager.Instance.CollectBerry();

            // Désactiver le marker tant que le buisson n'a pas repoussé
            if (mapMarkerInstance != null)
                mapMarkerInstance.SetActive(false);

            // Buisson invisible pour simuler la récolte
            GetComponent<Renderer>().enabled = false;

            // Lance la coroutine pour réactiver le buisson (et son marker) après le délai
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        GetComponent<Renderer>().enabled = true;
        if (mapMarkerInstance != null)
            mapMarkerInstance.SetActive(true);
    }
}
