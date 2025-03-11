using TMPro;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; //the target is the player
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0, 5, -5);
    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed * Time.deltaTime);

        // smooth rotation instead of normal lookat
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }

}
