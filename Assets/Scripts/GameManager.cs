using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton
    public int berryCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectBerry()
    {
        berryCount++;
        Debug.Log("Berry eaten! Total : " + berryCount);
        // TODO : Mise à jour de l'UI, Ted progresse...
    }
}
