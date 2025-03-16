using System;
using UnityEngine;
using static Enums;

public class Shelter : MonoBehaviour
{
    public Enums.Kingdom kingdom;
    public Enums.DietType dietType;
    public Enums.InteractionType interactionType;
    public Enums.ShelterType shelterType;

    //event triggered when the player enters a shelter
    public static event Action<ShelterType> OnPlayerEnterShelter;

    //if the bear touches the shelter, it enters on it
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player has entered a {shelterType}.");

            OnPlayerEnterShelter?.Invoke(shelterType);
        }
    }
}
