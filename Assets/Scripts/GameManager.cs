using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    [SerializeField] private Transform ted;

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
    }

    //loads the main game scene
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Main");  // it works because the scene was added in the biuld settings
    }
}
