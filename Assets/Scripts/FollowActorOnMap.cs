using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowActorOnMap : MonoBehaviour
{
    [SerializeField] private Transform actor;

    void Update()
    {
        transform.position = new Vector3(
            actor.position.x,
            128,
            actor.position.z
        );
    }
}
