using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class FreezeInteraction : IInteractionStrategy
{
    public void Interact(Creature creature, Player player)
    {
        Debug.Log($"{creature.name} freezes when seeing {player.name}!");
    }
}
