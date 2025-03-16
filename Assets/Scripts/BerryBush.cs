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
            // Incr�mente le compteur de baies via le GameManager
            GameManager.Instance.CollectBerry();

            // Ted grandit
            PlayerController playerController = other.GetComponent<PlayerController>();
            playerController?.grow();

            // Rend le buisson invisible pour simuler la r�colte
            gameObject.GetComponent<Renderer>().enabled = false;

            // Lance la coroutine pour r�activer le buisson apr�s le d�lai
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        gameObject.GetComponent<Renderer>().enabled = true;
    }
}
