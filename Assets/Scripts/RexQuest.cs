using UnityEngine;
using UnityEngine.UI;

public class RexQuest : MonoBehaviour
{
    [SerializeField] private Transform ted;
    [SerializeField] private Text dialogueText;
    [SerializeField] private GameObject rexRewardImage;
    [SerializeField] private Transform rexHouse;

    [SerializeField] private float followSpeed = 3f;

    [SerializeField] private float triggerDistance = 8f;

    [SerializeField] private float dialogueDuration = 8f;

    private enum QuestState { Idle, DialogueStarted, Following, QuestComplete }
    private QuestState currentState = QuestState.Idle;
    private float stateTimer = 0f;

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
                    dialogueText.text = "Ah, Ted ! Tu as bien grandi depuis la dernière fois ! Tu veux bien m'indiquer le chemin vers ma maison s'il te plait ? J'ai encore oublié. Tu sais, ma maison, juste devant celle de mes maîtres, à côté du terrain de Tennis.";
                    dialogueText.gameObject.SetActive(true);
                    stateTimer = 0f;
                    currentState = QuestState.DialogueStarted;
                }
                break;

            case QuestState.DialogueStarted:
                stateTimer += Time.deltaTime;
                if (stateTimer >= dialogueDuration)
                {
                    dialogueText.gameObject.SetActive(false);
                    currentState = QuestState.Following;
                }
                break;

            case QuestState.Following:
                FollowTed();
                if (distanceTedToHouse < triggerDistance)
                {
                    dialogueText.text = "Merci mon petit ! En guise de récompense, j'ai dessiné ma tête sur ta carte !";
                    dialogueText.gameObject.SetActive(true);
                    rexRewardImage.SetActive(true);
                    stateTimer = 0f;
                    currentState = QuestState.QuestComplete;
                }
                break;

            case QuestState.QuestComplete:
                gameObject.SetActive(false);
                stateTimer += Time.deltaTime;
                if (stateTimer >= dialogueDuration)
                {
                    dialogueText.gameObject.SetActive(false);
                }
                break;
        }
    }

    private void FollowTed()
    {
        Vector3 targetPos = new Vector3(ted.position.x, transform.position.y, ted.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, followSpeed * Time.deltaTime);
        Vector3 direction = targetPos - transform.position;
        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
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
