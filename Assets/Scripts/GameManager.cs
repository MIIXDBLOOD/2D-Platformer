using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int attemptCount = 1;
    private bool isGameOver = false;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("Geometry Dash GameManager initialized. Attempt: " + attemptCount);
    }

    public void RestartLevel()
    {
        if (isGameOver) return;

        isGameOver = true;
        attemptCount++;
        Debug.Log("Player Died! Restarting level. Attempt #" + attemptCount);

        // Reload current active scene
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);

        isGameOver = false;
    }

    // Reset attempts if starting a brand new game session
    public void ResetAttempts()
    {
        attemptCount = 1;
    }
}
