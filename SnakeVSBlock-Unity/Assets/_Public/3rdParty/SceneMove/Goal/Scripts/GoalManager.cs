using UnityEngine;
using TMPro;

public class GoalManager : MonoBehaviour
{
    public GameObject goalUI;
    public TMP_Text destroyedHPText;
    public TMP_Text stageClearText; // ← 追加：ステージ番号表示用
    public BlockDestroyManager destroyManager;
    public GameInitializer initializer;
    public ClickToStart clickToStart;
    public WallSpawner WallSpawner;

    private bool isGoalReached = false;

    public void OnPlayerReachedGoal()
    {
        if (destroyManager != null && destroyedHPText != null)
        {
            int totalHP = destroyManager.GetTotalDestroyedHP();
            destroyedHPText.text = $"{totalHP}";
        }

        if (stageClearText != null && StageManager.Instance != null)
        {
            int currentStage = StageManager.Instance.GetCurrentStage();
            stageClearText.text = $"{currentStage}";
        }

        if (WallSpawner != null)
        {
            WallSpawner.ClearWallsOnly();
        }

        Time.timeScale = 0f;
        goalUI.SetActive(true);
        isGoalReached = true;
    }

    private void Update()
    {
        if (isGoalReached && Input.GetMouseButtonDown(0))
        {
            isGoalReached = false;
            goalUI.SetActive(false);

            if (StageManager.Instance != null) 
            {
                StageManager.Instance.IncrementStage();
            }
               
            if (initializer != null) 
            {
                initializer.ResetToStartState(false);
            }
               
            if (clickToStart != null)
            {
                clickToStart.RestartStartScreen(); // ← ここでUI表示まで呼ばれる
            }
        }
    }
}
