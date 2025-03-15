using UnityEngine;

public class FerrisWheelRotation : MonoBehaviour
{
    [SerializeField] private Transform[] cabins;
    [SerializeField] private float rotationSpeed = 10f;

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Chaque cabine annule la rotation du parent pour rester droite
        foreach (Transform cabin in cabins)
            cabin.rotation = Quaternion.identity;
    }
}
