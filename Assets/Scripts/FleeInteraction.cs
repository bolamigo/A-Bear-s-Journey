using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeInteraction : IInteractionStrategy
{
    public void Interact(Creature creature, Player player)
    {
        Debug.Log($"{creature.name} flees from {player.name}!");
    }
}