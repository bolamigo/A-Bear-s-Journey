using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

//in-game objects that are not creatures but still belong to the game world
public class Resource : WorldEntity
{
    public string resourceName;

    private void Start()
    {
        position = transform.position; // Uses "position" from WorldEntity
    }
}
