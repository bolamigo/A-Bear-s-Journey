using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Creature : WorldEntity
{
    //public CreatureData data; //needs to be defined yet. Can be a scriptable object...
                                //it should contain all the usefull information/properties about the creature
    private IInteractionStrategy interactionStrategy;

    private void Start()
    {
        //kingdoms = data.kingdoms;  
        position = transform.position; 
        //interactionStrategy = InteractionStrategyFactory.CreateStrategy(data.defaultInteraction);
    }

    public void InteractWithPlayer(Player player)
    {
        interactionStrategy?.Interact(this, player);
    }
}
