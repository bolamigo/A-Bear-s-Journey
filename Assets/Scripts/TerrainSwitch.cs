using UnityEngine;

public class TerrainSwitch : MonoBehaviour
{
    private enum TerrainType
    {
        Ground,
        Water
    }

    [SerializeField] private TerrainType terrain;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                if (terrain == TerrainType.Water)
                {
                    pc.SetSwimming(true);
                }
                else if (terrain == TerrainType.Ground)
                {
                    pc.SetSwimming(false);
                }
            }
        }
    }
}
