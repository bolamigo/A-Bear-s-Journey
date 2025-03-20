using UnityEngine;
using System.Collections;
using static Enums;
using System.Collections.Generic;

public class BerryBush : Resource
{
    public static List<BerryBush> AllBerryBushes = new List<BerryBush>();

    [SerializeField] private float respawnDelay = 10f;
    // R�f�rence au prefab du marker de la baie � afficher sur la map
    [SerializeField] private GameObject mapMarkerPrefab;
    // Instance du marker instanci�
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

            // Pour �viter les erreurs de FollowActorOnMap
            if (followScript != null)
                followScript.setActor(transform);

            if (!AllBerryBushes.Contains(this))
                AllBerryBushes.Add(this);
        }
    }

    private Vector3 GetMapPosition()
    {
        // Position du buisson sur la map
        return new Vector3(transform.position.x, BerriesHeightOnMap, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GetComponent<Renderer>().enabled)
        {
            // Incr�mente le compteur de baies via le GameManager
            GameManager.Instance.CollectBerry();

            // Ted grandit
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController?.grow();

            // D�sactiver le marker tant que le buisson n'a pas repouss�
            if (mapMarkerInstance != null)
                mapMarkerInstance.SetActive(false);

            // Buisson invisible pour simuler la r�colte
            GetComponent<Renderer>().enabled = false;

            AllBerryBushes.Remove(this);

            // Lance la coroutine pour r�activer le buisson (et son marker) apr�s le d�lai
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        GetComponent<Renderer>().enabled = true;
        if (mapMarkerInstance != null)
        {
            mapMarkerInstance.SetActive(true);

            if (!AllBerryBushes.Contains(this))
                AllBerryBushes.Add(this);
        }
    }
}
