using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Vector3 currentPosition;
    private Vector3 targetPosition;
    [SerializeField] private Camera cam;
    private static readonly int Run = Animator.StringToHash("Run Forward");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Attack = Animator.StringToHash("Attack2");
    private const UnityEngine.KeyCode M_KEY = KeyCode.Semicolon; // Querty-based naming
    [SerializeField] private GameObject mapCamera;
    private bool isMapActive = false;

    [SerializeField] private Transform fixedAnchor;
    [SerializeField] private GameObject canvas;

    // Pour l'animation
    [SerializeField] private Transform armature;

    // Pour toggle l'animation de nage
    public bool isSwimming { get; private set; } = false;

    private NavMeshAgent agent;
    private Animator bearAnimator;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        targetPosition = currentPosition;
        transform.rotation = Quaternion.Euler(0,0,0);

        agent = GetComponent<NavMeshAgent>();
        bearAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(M_KEY))
        {
            isMapActive = !isMapActive;
            if (mapCamera != null)
            {
                mapCamera.SetActive(isMapActive);
                canvas.SetActive(!isMapActive);
            }
        }
    }

    void FixedUpdate()
    {
        if (isMapActive) return;

        if(!agent.enabled)return;
        if (isSwimming)
        {
            // Mode nage : animation run en continu, vitesse réduite et légère inclinaison en arrière.
            bearAnimator.SetBool(Run, true);
            bearAnimator.SetBool(Idle, false);

            Vector3 newEuler = armature.localEulerAngles;
            newEuler.x = -30f;
            armature.localEulerAngles = newEuler;
        }
        else
        {
            if (agent.remainingDistance > 1)
            {
                bearAnimator.SetBool(Run, true);
                bearAnimator.SetBool(Idle, false);
            }
            else
            {
                bearAnimator.SetBool(Run, false);
                bearAnimator.SetBool(Idle, true);
            }

            // Reset rotation
            Vector3 newEuler = armature.localEulerAngles;
            newEuler.x = 0f;
            armature.localEulerAngles = newEuler;
        }

        currentPosition = transform.position;
        if(Input.GetMouseButton(0)){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit)) {
                agent.destination = hit.point;
            }
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 3, Color.red); 
        if(Input.GetMouseButtonDown(1)){
            bearAnimator.SetTrigger(Attack);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3))
            { 
                LivingAnimal livingAnimal = hit.collider.gameObject.GetComponent<LivingAnimal>();
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.yellow); 
                if(livingAnimal != null){
                    livingAnimal.Damage(this.gameObject,5);
                }
            }
        }
    }

    public void SetSwimming(bool swimming)
    {
        isSwimming = swimming;
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
