using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RexQuest : MonoBehaviour
{
    [SerializeField] private Transform ted;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject rexRewardImage;
    [SerializeField] private Transform rexHouse;
    [SerializeField] private GameObject rexRenderer;

    [SerializeField] private float followSpeed = 3f;

    [SerializeField] private float triggerDistance = 8f;

    [SerializeField] private float dialogueDuration = 8f;

    private NavMeshAgent agent;

    private enum QuestState { Idle, DialogueStarted, Following, QuestComplete }
    private QuestState currentState = QuestState.Idle;
    private float stateTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = followSpeed;
        }
        dialogueText.gameObject.SetActive(false);
        rexRewardImage.SetActive(false);
    }

    void Update()
    {
        // Calcul de distances horizontales (en ignorant la composante Y)
        float distanceTedToRex = HorizontalDistance(ted.position, transform.position);
        float distanceTedToHouse = HorizontalDistance(ted.position, rexHouse.position);

        switch (currentState)
        {
            case QuestState.Idle:
                if (distanceTedToRex < triggerDistance)
                {
                    dialogueText.text = "Ah, Ted ! Tu as bien grandi depuis la dernière fois ! Tu veux bien m'indiquer le chemin vers ma maison s'il te plait ? Je suis perdu. Tu sais, ma maison, juste devant celle de mes maîtres, à côté du terrain de Tennis.";
                    dialogueText.gameObject.SetActive(true);
                    stateTimer = 0f;
                    currentState = QuestState.DialogueStarted;
                }
                break;

            case QuestState.DialogueStarted:
                if (agent != null && distanceTedToRex > 4f)
                {
                    agent.SetDestination(ted.position);
                }
                stateTimer += Time.deltaTime;
                if (stateTimer >= dialogueDuration)
                {
                    dialogueText.gameObject.SetActive(false);
                    currentState = QuestState.Following;
                }
                break;

            case QuestState.Following:
                if (agent != null && distanceTedToRex > 4f)
                {
                    agent.SetDestination(ted.position);
                }
                if (distanceTedToHouse < triggerDistance)
                {
                    dialogueText.text = "Merci mon petit ! En guise de récompense, je t'ai dessiné ma tête !";
                    dialogueText.gameObject.SetActive(true);
                    rexRewardImage.SetActive(true);
                    stateTimer = 0f;
                    currentState = QuestState.QuestComplete;
                }
                break;

            case QuestState.QuestComplete:
                stateTimer += Time.deltaTime;
                rexRenderer.SetActive(false);
                if (stateTimer >= dialogueDuration)
                {
                    dialogueText.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    // Calcule la distance horizontale entre deux positions (ignore Y)
    private float HorizontalDistance(Vector3 a, Vector3 b)
    {
        Vector2 a2 = new Vector2(a.x, a.z);
        Vector2 b2 = new Vector2(b.x, b.z);
        return Vector2.Distance(a2, b2);
    }
}
