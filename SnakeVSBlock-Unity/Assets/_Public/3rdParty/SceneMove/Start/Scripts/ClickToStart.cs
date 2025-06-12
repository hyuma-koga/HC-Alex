using UnityEngine;
using TMPro;

public class ClickToStart : MonoBehaviour
{
    public GameObject startScreenUI;
    public PlusOrbSpawner orbSpawner;
    public TMP_Text scoreText;
    public TMP_Text stageNumberText;
    public BlockDestroyManager destroyManager;
    public GameInitializer initializer;
    public WallSpawner wallSpawner;

    private SnakeFollowMouse playerSnake;
    private bool hasStarted = false;
    private float inputDelayTimer = 0f;
    private const float inputDelayDuration = 0.3f;

    private void Start()
    {
        if (initializer != null)
        {
            playerSnake = initializer.GetCurrentSnake();
        }

        if (startScreenUI != null)
        {
            startScreenUI.SetActive(true);
        }

        if (playerSnake != null)
        {
            playerSnake.canMove = false;
        }

        if (orbSpawner != null)
        {
            orbSpawner.canSpawn = false;
        }

        UpdateStageText();
        UpdateScoreText();
    }

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (!hasStarted)
        {
            inputDelayTimer -= Time.unscaledDeltaTime;

            if (inputDelayTimer <= 0f && Input.GetMouseButtonDown(0))
            {
                TryAssignPlayerSnake();

                if (playerSnake == null)
                {
                    return;
                }

                hasStarted = true;
                Time.timeScale = 1f;

                playerSnake.canMove = true;

                if (orbSpawner != null)
                {
                    orbSpawner.canSpawn = true;
                }

                if (startScreenUI != null)
                {
                    startScreenUI.SetActive(false);
                }

                if (wallSpawner != null)
                {
                    wallSpawner.ResetWalls();
                }
            }
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null && ScoreManager.Instance != null)
        {
            scoreText.text = $"{ScoreManager.Instance.TotalScore}";
        }
    }


    private void UpdateStageText()
    {
        if (stageNumberText != null)
        {
            int stage = StageManager.Instance != null ? StageManager.Instance.GetCurrentStage() : 1;
            stageNumberText.text = $"{stage}";
        }
    }

    public void RestartStartScreen()
    {
        if (initializer != null)
        {
            initializer.ResetToStartState(ResetReason.Restart); // ✅ GameOver以外のときは Restart 扱い
            TryAssignPlayerSnake();
        }

        hasStarted = false;
        inputDelayTimer = inputDelayDuration;

        if (playerSnake != null)
        {
            playerSnake.canMove = false;
        }

        if (orbSpawner != null)
        {
            orbSpawner.canSpawn = false;
        }

        UpdateStageText();
        UpdateScoreText();

        if (startScreenUI != null)
        {
            startScreenUI.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private void TryAssignPlayerSnake()
    {
        if (initializer != null)
        {
            playerSnake = initializer.GetCurrentSnake();
        }

        if (playerSnake == null)
        {
            playerSnake = FindFirstObjectByType<SnakeFollowMouse>();
        }
    }
}
