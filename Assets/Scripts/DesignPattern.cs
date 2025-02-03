// Enum Flags for Kingdoms and DietTypes
// [Flags] lets an entity have multiple Kingdoms and DietTypes at once

using UnityEngine;

[System.Flags]
public enum Kingdom
{
    Mineral = 1 << 0,
    Vegetal = 1 << 1,
    Animal = 1 << 2,
    Fungus = 1 << 3
}

[System.Flags]
public enum DietType
{
    Omnivore = 1 << 0,
    Carnivore = 1 << 1,
    Insectivore = 1 << 2,
    Herbivore = 1 << 3,
    Ruminant = 1 << 4,
    Piscivore = 1 << 5,
    Granivore = 1 << 6,
    Scavenger = 1 << 7
}

// ScriptableObjects for configurable data
using UnityEngine;

[CreateAssetMenu(menuName = "Game/CreatureData")]
public class CreatureData : ScriptableObject
{
    public string creatureName;
    public Kingdom kingdoms;
    public DietType diets;
    public InteractionType defaultInteraction; // Strategy pattern
    public Sprite spriteRepresentation;
    // Other data (marchingSpeed, runningSpeed, swimmingSpeed, flyingSpeed, weight, health, etc.)
}

// Strategy pattern for interactions
// Each creature except Ted has a default behavior (flee, fight, freeze, etc.)
// Strategy pattern is great here: we can make distinct classes for these behaviors.
public interface IInteractionStrategy
{
    void Interact(Creature creature, Player player);
}

// So let's implement some strategies
public class FleeInteraction : IInteractionStrategy
{
    public void Interact(Creature creature, Player player)
    {
        Debug.Log($"{creature.name} flees away from you!");
    }
}

public class FightInteraction : IInteractionStrategy
{
    public void Interact(Creature creature, Player player)
    {
        Debug.Log($"{creature.name} attacks you!");
    }
}

public class FreezeInteraction : IInteractionStrategy
{
    public void Interact(Creature creature, Player player)
    {
        Debug.Log($"{creature.name} freezes at your sight!");
    }
}

// interaction Factory
public enum InteractionType { Flee, Fight, Freeze /* , ... */ }

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
            // ...
            default:
                return null;
        }
    }
}

// Component pattern for Entity hierarchy
using UnityEngine;

public abstract class WorldEntity : MonoBehaviour
{
    public Kingdom kingdoms;
    // other common properties (position, etc.)
}

// derived classes
public class Resource : WorldEntity
{
    public string resourceName;
    // other common properties (quantity, etc.)
}

// Creature, using Component & Factory
public class Creature : WorldEntity
{
    public CreatureData data;
    private IInteractionStrategy interactionStrategy;

    void Start()
    {
        kingdoms = data.kingdoms;
        interactionStrategy = InteractionStrategyFactory.CreateStrategy(data.defaultInteraction);
    }

    public void InteractWithPlayer(Player player)
    {
        interactionStrategy?.Interact(this, player);
    }
}

// Shelters & player state
// State pattern or just conditionnal logic
using UnityEngine;

public enum ShelterType { DenseForest, Cave }

public class Shelter : MonoBehaviour
{
    public ShelterType shelterType;

    public void EnterShelter(Player player)
    {
        Debug.Log($"Ted enters a shelter: {shelterType}.");
        GameEvents.PlayerEnteredShelter(shelterType); // observer
        player.EnterSleepMode();
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{
    // Ted data (Health, Energy, etc.)

    public void EnterSleepMode()
    {
        Debug.Log("L'ours s'endort...");
    }
}

// Factory to dinamically generate creatures & ressources in game
// e.g. for procedural generation
using UnityEngine;

public class CreatureFactory : MonoBehaviour
{
    public Creature creaturePrefab;

    public Creature CreateCreature(CreatureData data, Vector3 position)
    {
        Creature creature = Instantiate(creaturePrefab, position, Quaternion.identity);
        creature.data = data;
        creature.kingdoms = data.kingdoms;
        // interaction strategy initialized in creature.Start().
        return creature;
    }
}

// Observer (Event System) for communication between components
public class GameEvents
{
    public static event System.Action<ShelterType> OnPlayerEnterShelter;

    public static void PlayerEnteredShelter(ShelterType type)
    {
        OnPlayerEnterShelter?.Invoke(type);
    }
}

// And then we can sub to the event, e.g. in UI
void OnEnable()
{
    GameEvents.OnPlayerEnterShelter += HandlePlayerEnterShelter;
}

void OnDisable()
{
    GameEvents.OnPlayerEnterShelter -= HandlePlayerEnterShelter;
}

void HandlePlayerEnterShelter(ShelterType type)
{
    Debug.Log($"Notification reçue : le joueur est dans un abri de type {type}");
}