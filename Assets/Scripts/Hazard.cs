using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has a PlayerController or is tagged "Player"
        if (collision.CompareTag("Player") || collision.GetComponent<PlayerController>() != null)
        {
            // Contact with hazard kills player -> trigger level reload/restart
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.RestartLevel();
            }
            else
            {
                // Fallback direct reload if GameManager doesn't exist in scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
                );
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle solid obstacle physics (e.g. hitting the front side of a standard block hazard)
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // In Geometry Dash, hitting a wall from the side triggers death.
            // Check if the collision normal is mostly horizontal.
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // If normal points left, player hit a wall facing right
                if (Mathf.Abs(contact.normal.y) < 0.5f)
                {
                    GameManager gameManager = FindFirstObjectByType<GameManager>();
                    if (gameManager != null)
                    {
                        gameManager.RestartLevel();
                    }
                    else
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(
                            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
                        );
                    }
                    break;
                }
            }
        }
    }
}
