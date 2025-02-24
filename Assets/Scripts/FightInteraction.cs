using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class FightInteraction : IInteractionStrategy
{
    public void Interact(Creature creature, Player player)
    {
        Debug.Log($"{creature.name} fights against {player.name}!");
    }
}
