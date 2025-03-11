using UnityEngine;

public class BearController : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    private Vector3 currentPosition;
    private Vector3 targetPosition;
    [SerializeField] private Camera cam;
    private CharacterController characterController;
    private Animator bearAnimator;
    private static readonly int Run = Animator.StringToHash("Run Forward");
    private static readonly int Idle = Animator.StringToHash("Idle");

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        bearAnimator = GetComponent<Animator>();
        currentPosition = transform.position;
        targetPosition = currentPosition;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPosition = hit.point;
            }
        }
    }

    void Update()
    {
        currentPosition = transform.position;
        Vector3 lookDirection = targetPosition - currentPosition;
        lookDirection.y = 0; // no rotation on y-axis

        if (lookDirection.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        if (Vector3.Distance(currentPosition, targetPosition) > 2)
        {
            bearAnimator.SetBool(Run, true);
            bearAnimator.SetBool(Idle, false);
            characterController.Move(Vector3.Normalize(lookDirection) * speed * Time.deltaTime);
        }
        else
        {
            bearAnimator.SetBool(Run, false);
            bearAnimator.SetBool(Idle, true);
        }
    }

}
