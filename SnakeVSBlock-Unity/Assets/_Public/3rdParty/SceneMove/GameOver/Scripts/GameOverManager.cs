using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameInitializer initializer;
    public ClickToStart startScreen;

    private bool isGameOver = false;

    public void ShowGameOver()
    {
        isGameOver = true;

        if(gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (isGameOver && Input.GetMouseButtonDown(0))
        {
            isGameOver = false;
            Time.timeScale = 1f;

            if(gameOverUI != null)
            {
                gameOverUI.SetActive(false);
            }

            initializer.ResetToStartState(ResetReason.GameOver);
            startScreen?.RestartStartScreen();
        }
    }
}
