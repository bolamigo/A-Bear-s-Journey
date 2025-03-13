using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class BearController : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    private Vector3 currentPosition;
    private Vector3 targetPosition;
    [SerializeField] private Camera cam;
    private static readonly int Run = Animator.StringToHash("Run Forward");
    private static readonly int Idle = Animator.StringToHash("Idle");
    static Vector3 horizontal(Vector3 vec){
        return new Vector3(vec.x,0.0f,vec.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        targetPosition = currentPosition;
        transform.rotation = Quaternion.Euler(0,0,0);
    }
    void Update(){
    }
    // Update is called once per frame
    void FixedUpdate(){
        CharacterController characterController = GetComponent<CharacterController>();
        Animator bearAnimator = GetComponent<Animator>();
        currentPosition = transform.position;
        if(Input.GetMouseButton(0)){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit)) {
                targetPosition = hit.point;
                // Do something with the object that was hit by the raycast.
            }
        }
        Vector3 horizontal_target = new(targetPosition.x,transform.position.y,targetPosition.z);

        // Does the ray intersect any objects excluding the player layer
        Vector3 forward = Vector3.Normalize(targetPosition - transform.position);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit ground_hit))

        { 
            transform.LookAt(horizontal_target, ground_hit.normal);
        }

        if(Vector3.Distance(horizontal(currentPosition),horizontal(targetPosition))>2){
            bearAnimator.SetBool(Run, true);
            bearAnimator.SetBool(Idle, false);
            characterController.SimpleMove(Vector3.Normalize(targetPosition-currentPosition) * speed);
        }
        else{
            bearAnimator.SetBool(Run, false);
            bearAnimator.SetBool(Idle, true);
        }
    }
}
