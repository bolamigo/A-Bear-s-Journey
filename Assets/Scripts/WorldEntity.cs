using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using static Enums;

public abstract class WorldEntity : MonoBehaviour
{
    public Kingdom kingdoms;  // Shared by all entities
    public Vector3 position;  // Common position field
}
