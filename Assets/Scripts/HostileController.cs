using UnityEngine;
using UnityEngine.AI;

public class HostileController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    public GameObject player;

    public string walkForwardAnimation = "walk_forward";
    public string walkBackwardAnimation = "walk_backwards";
    public string turn90LAnimation = "turn_90_L";
    public string turn90RAnimation = "turn_90_R";
    public string runForwardAnimation = "run_forward";
    public string trotAnimation = "trot_forward";
    public string standtositAnimation = "stand_to_sit";
    public string sittostandAnimation = "sit_to_stand";
    public string shuffleRAnimation = "shuffle_R";
    public string shuffleLAnimation = "shuffle_L"; // not in use here but it exists too
    public string eatingAnimation = "eat";
    public string attackAnimation = "attack";
    public string hitReactionAnimation = "hit_reaction";
    public string deathAnimation = "die";

    private float idleTime;
    private float idleTimer;
    private bool isIdle = false;
    private bool isAttacking = false;

    private float minIdleTime = 3f; // Min time to stay idle
    private float maxIdleTime = 10f; // Max time to stay idle
    private float minMoveTime = 3f; // Min time to move
    private float maxMoveTime = 10f; // Max time to move

    private float minDistance = 5f; // Minimum distance to target point
    private float maxDistance = 20f; // Maximum distance for random movement

    private Vector3 targetPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        MoveToRandomPosition(); // moving to random position
    }

    void FixedUpdate()
    {
        if(!agent.enabled)return;
        if(Vector3.Distance(transform.position,player.transform.position)<10){
            isIdle = false;
            Attack();
            isAttacking = true;
        }
        if (isIdle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTime)
            {
                isIdle = false;
                isAttacking = false;
                MoveToRandomPosition();
            }
            else
            {
                // eating animation
                animator.Play(eatingAnimation);
            }
        }
        else
        {
            // the target is reached
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                isIdle = true;
                idleTime = Random.Range(minIdleTime, maxIdleTime); // Set random idle time
                idleTimer = 0f; // Reset idle timer
            }

            // walking animation if moving
            if (agent.velocity.magnitude > 4)
            {
                animator.Play(runForwardAnimation);
            }
            else if (agent.velocity.magnitude > 3)
            {
                animator.Play(walkForwardAnimation);
            }
            else
            {
                // idle animation
                animator.Play(eatingAnimation);
            }
        }
    }

    private void MoveToRandomPosition()
    {
        // random target position within a specified radius
        Vector3 randomDirection = Random.insideUnitSphere * Random.Range(minDistance, maxDistance);
        randomDirection += transform.position;

        // target position is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, maxDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        // run instead of walk
        if (Random.value > 0.5f)
        {
            agent.speed = 3.5f; 
        }
        else
        {
            agent.speed = 1.5f; 
        }
    }
    private void Attack()
    {
        // random target position within a specified radius
        agent.SetDestination(player.transform.position);
        agent.speed = 5.5f;
    }
    private void OnTriggerEnter(Collider other)
    {
        animator.Play(attackAnimation);
        if(other.gameObject==player){
            player.GetComponent<LivingAnimal>().Damage(this.gameObject,2);
        }
    }
}
