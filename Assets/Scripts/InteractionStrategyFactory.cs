using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public static class InteractionStrategyFactory
{
    public static IInteractionStrategy CreateStrategy(InteractionType type)
    {
        switch (type)
        {
            case InteractionType.Flee:
                return new FleeInteraction();
            case InteractionType.Fight:
                return new FightInteraction();
            case InteractionType.Freeze:
                return new FreezeInteraction();
            default:
                throw new ArgumentException("Invalid Interaction Type");
        }
    }
}

public enum InteractionType
{
    Flee,
    Fight,
    Freeze
}
