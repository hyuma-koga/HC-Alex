using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameInitializer initializer;
    public ClickToStart startScreen;

    [Header("スコア表示UI")]
    public TMP_Text scoreValueText;
    public TMP_Text totalScoreValueText;

    private bool isGameOver = false;

    public void ShowGameOver()
    {
        isGameOver = true;

        if(scoreValueText != null)
        {
            scoreValueText.text = ScoreManager.Instance.CurrentScore.ToString();
        }

        if(totalScoreValueText != null)
        {
            totalScoreValueText.text = ScoreManager.Instance.TotalScore.ToString();
        }

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

            ScoreManager.Instance.ResetCurrentScore();
            initializer.ResetToStartState(ResetReason.GameOver);
            startScreen?.RestartStartScreen();
        }
    }
}
