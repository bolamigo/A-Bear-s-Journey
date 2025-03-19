using UnityEngine;

public class FollowBerryOnMap : MonoBehaviour
{
    private Transform actor;

    public void setActor(Transform actor)
    {
        this.actor = actor;
    }

    void Update()
    {
        transform.position = new Vector3(
            actor.position.x,
            125,
            actor.position.z
        );
    }
}
