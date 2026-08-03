using UnityEngine;
using UnityEngine.SceneManagement; // Needed to reload the scene
using TMPro; // Needed to control TextMeshPro UI

public class GameManager : MonoBehaviour
{
    // Singleton pattern so other scripts can access this easily
    public static GameManager Instance;

    [Header("References")]
    public Transform player;          // Drag your Player here in the Inspector
    public Transform playerTransform; // Drag your PlayerTransform here in the Inspector
    public TextMeshProUGUI scoreText; // Drag your ScoreText UI here in the Inspector
    public GameObject gameOverUI;     // Drag your GameOver UI Panel here in the Inspector

    private bool isGameOver = false;
    private Vector3 playerStartPos;
    private float score;

    private void Awake()
    {
        // Setup Singleton Instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ensure game time is running normal and UI panel is hidden
        Time.timeScale = 1f;

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // Set playerTransform to player if it is not assigned
        if (playerTransform == null && player != null)
        {
            playerTransform = player;
        }

        if (playerTransform != null)
        {
            playerStartPos = playerTransform.position;
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        // Calculate score based on forward distance traveled from starting position
        Transform targetPlayer = playerTransform != null ? playerTransform : player;
        if (targetPlayer != null)
        {
            score = Mathf.Max(0f, targetPlayer.position.x - playerStartPos.x);

            if (scoreText != null)
            {
                scoreText.text = "Distance: " + Mathf.FloorToInt(score).ToString() + "m";
            }
        }
    }

    /// <summary>
    /// Triggered when the player hits an obstacle. Pauses the game and shows Game Over UI.
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (gameOverUI != null)
        {
            // Pause gameplay
            Time.timeScale = 0f;
            // Display Game Over screen
            gameOverUI.SetActive(true);
        }
        else
        {
            // Reload the current scene to reset the game if no UI is set
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    /// <summary>
    /// Restarts the active level. Linked to the "Try Again" button in UI.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time back to normal before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
