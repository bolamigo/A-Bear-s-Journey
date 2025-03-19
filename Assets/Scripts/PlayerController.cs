using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    private Vector3 currentPosition;
    private Vector3 targetPosition;
    [SerializeField] private Camera cam;
    private static readonly int Run = Animator.StringToHash("Run Forward");
    private static readonly int Idle = Animator.StringToHash("Idle");
    static Vector3 horizontal(Vector3 vec){
        return new Vector3(vec.x,0.0f,vec.z);
    }

    private const UnityEngine.KeyCode M_KEY = KeyCode.Semicolon; // Query-based naming
    [SerializeField] private GameObject mapCamera;
    private bool isMapActive = false;

    [SerializeField] private Transform fixedAnchor;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        targetPosition = currentPosition;
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    void Update()
    {
        if (Input.GetKeyDown(M_KEY))
        {
            isMapActive = !isMapActive;
            if (mapCamera != null)
                mapCamera.SetActive(isMapActive);
        }
    }

    void FixedUpdate(){
        if (isMapActive) return;
        CharacterController characterController = GetComponent<CharacterController>();
        Animator bearAnimator = GetComponent<Animator>();
        currentPosition = transform.position;
        if(Input.GetMouseButton(0)){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit)) {
                targetPosition = hit.point;
            }
        }
        Vector3 horizontal_target = new(targetPosition.x,transform.position.y,targetPosition.z);

        // Does the ray intersect any objects excluding the player layer
        Vector3 forward = Vector3.Normalize(targetPosition - transform.position);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit ground_hit))

        { 
            transform.LookAt(horizontal_target, ground_hit.normal);
        }
        if(Vector3.Distance(horizontal(currentPosition),horizontal(targetPosition))>1){
            bearAnimator.SetBool(Run, true);
            bearAnimator.SetBool(Idle, false);
            characterController.SimpleMove(Vector3.Normalize(targetPosition-currentPosition) * speed);
        }
        else{
            bearAnimator.SetBool(Run, false);
            bearAnimator.SetBool(Idle, true);
        }
    }
    public float getAge()
    {
        return transform.localScale.x;
    }

    public void grow()
    {
        float currentAge = getAge();
        float maxAge = 1.2f;
        if (currentAge >= maxAge)
        {
            // Si on est d�j� � la taille maximale, on fixe la scale de Ted et de la cam�ra
            transform.localScale = Vector3.one * maxAge;
            if (fixedAnchor != null)
                fixedAnchor.transform.localScale = Vector3.one * 1.0f;
        }
        else
        {
            // Transition progressive
            StartCoroutine(GrowTransition());
        }
    }

    private IEnumerator GrowTransition()
    {
        float duration = 0.5f; // dur�e de la transition en secondes
        float timer = 0f;

        // Valeurs initiales
        float currentAge = getAge();
        Vector3 initialTedScale = transform.localScale;

        // Calcul de la nouvelle taille de Ted apr�s consommation d'une baie (+0.1)
        float newAge = currentAge + 0.1f;
        Vector3 targetTedScale = Vector3.one * newAge;

        // Param�tres pour le calcul du dezoom de la cam�ra :
        // Ted grandit de 0.4 � 1.2 (intervalle de progression)
        float initialAge = 0.4f;
        float maxAge = 1.2f;
        // t varie de 0 (� 0.4) � 1 (� 1.2)
        float t_target = (newAge - initialAge) / (maxAge - initialAge);
        // Interpolation logarithmique
        float a = 4.0f; // Trial & error sur le param�tre de courbure, �a passe bien avec 4 �\_(o.o)_/�
        float startCamera = 2.2f;
        float endCamera = 1.0f;
        float targetCameraScaleValue = startCamera + (endCamera - startCamera) * (Mathf.Log(1 + a * t_target) / Mathf.Log(1 + a));
        Vector3 targetCameraScale = Vector3.one * targetCameraScaleValue;

        Vector3 initialCameraScale = fixedAnchor != null ? fixedAnchor.transform.localScale : Vector3.one;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            // Interpolation lin�aire pour Ted
            transform.localScale = Vector3.Lerp(initialTedScale, targetTedScale, progress);
            // Interpolation lin�aire pour la cam�ra (avec le calcul logarithmique d�j� appliqu� au target)
            if (fixedAnchor != null)
                fixedAnchor.transform.localScale = Vector3.Lerp(initialCameraScale, targetCameraScale, progress);
            yield return null;
        }

        // Valeurs finales
        transform.localScale = targetTedScale;
        if (fixedAnchor != null)
            fixedAnchor.transform.localScale = targetCameraScale;
    }

}
