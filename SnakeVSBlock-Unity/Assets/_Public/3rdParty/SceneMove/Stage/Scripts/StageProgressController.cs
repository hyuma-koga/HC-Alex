using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageProgressController : MonoBehaviour
{
    [Header("スライダーUI")]
    [SerializeField] private Image fillBar;  // 進捗バー（Image Type: Filled）

    [Header("ステージ番号UI")]
    [SerializeField] private TMP_Text currentStageText;
    [SerializeField] private TMP_Text nextStageText;

    private float progress = 0f;
    private int currentStage = 1;

    private void Start()
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.OnStageChanged += OnStageChanged;
            OnStageChanged(StageManager.Instance.GetCurrentStage());
        }

        UpdateFillBar(); // 初期化
    }

    private void OnDestroy()
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.OnStageChanged -= OnStageChanged;
        }
    }

    private void UpdateFillBar()
    {
         if (fillBar != null)
         {
             fillBar.fillAmount = progress;
             Debug.Log($"[StageProgressController] fillAmount = {progress}");
         }
         else
         {
             Debug.LogWarning("fillBar が null です！");
         }
    }

    private void OnStageChanged(int stageNumber)
    {
        currentStage = stageNumber;
        UpdateUI();
    }

    public void SetProgress(float value)
    {
        progress = Mathf.Clamp01(value);
        UpdateFillBar(); // 即時反映
    }

    private void UpdateUI()
    {
        if (currentStageText != null) 
        {
            currentStageText.text = currentStage.ToString();
        }
           
        if (nextStageText != null) 
        {
            nextStageText.text = (currentStage + 1).ToString();
        }   
    }
}
