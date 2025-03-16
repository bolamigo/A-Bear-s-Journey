using UnityEngine;
using System.Collections;
using static Enums;

public class BerryBush : Resource
{
    [SerializeField] private float respawnDelay = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            gameObject.GetComponent<Renderer>().enabled)
        {
            // Incrémente le compteur de baies via le GameManager
            GameManager.Instance.CollectBerry();

            // Ted grandit
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController?.grow();

            // Rend le buisson invisible pour simuler la récolte
            gameObject.GetComponent<Renderer>().enabled = false;

            // Lance la coroutine pour réactiver le buisson après le délai
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        gameObject.GetComponent<Renderer>().enabled = true;
    }
}
