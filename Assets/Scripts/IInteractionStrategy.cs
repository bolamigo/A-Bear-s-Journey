using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionStrategy
{
    void Interact(Creature creature, Player player);
}
