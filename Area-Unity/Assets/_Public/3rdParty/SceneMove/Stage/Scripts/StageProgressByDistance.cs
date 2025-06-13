using UnityEngine;

public class StageProgressByDistance : MonoBehaviour
{
    [SerializeField] private Transform snakeTransform;
    [SerializeField] private StageProgressController progressController;
    [SerializeField] private float startY = float.NaN;  // 未設定状態のフラグとしてNaNを使う
    [SerializeField] private float goalY = 100f;

    private bool stageCompleted = false;
    private bool hasIncrementedStage = false;

    private void Start()
    {
        if (snakeTransform != null)
        {
            Transform headSprite = snakeTransform.Find("HeadSprite");
            if (headSprite != null)
            {
                snakeTransform = headSprite;
                Debug.Log("[StageProgressByDistance] HeadSpriteを参照対象に切り替えました");
            }
        }
    }

    private void Update()
    {
        if ((snakeTransform == null || !snakeTransform.gameObject.activeInHierarchy) && SnakeFollowMouse.Instance != null)
        {
            var newHead = SnakeFollowMouse.Instance.transform.Find("HeadSprite");
            
            if (newHead != null)
            {
                snakeTransform = newHead;
                Debug.Log("[StageProgressByDistance] 再生成後のHeadSpriteに再接続しました");
            }
        }

        // Snakeが移動可能なときのみ処理
        if (!SnakeFollowMouse.Instance || !SnakeFollowMouse.Instance.canMove)
        {
            return;
        }

        if (snakeTransform == null || progressController == null || float.IsNaN(startY))
        {
            return;
        }

        float snakeY = snakeTransform.position.y;
        float progress = Mathf.InverseLerp(startY, goalY, snakeY);
        progressController.SetProgress(progress);

        if (!hasIncrementedStage && progress >= 0.999f)
        {
            hasIncrementedStage = true;
            stageCompleted = true;
            Debug.Log("[StageProgressByDistance] ゴール到達 → ステージ進行");
            StageManager.Instance?.IncrementStage();
        }
    }

    public void ResetStartY()
    {
        if (snakeTransform == null)
        {
            return;
        }

        startY = snakeTransform.position.y;
        goalY = startY + 100f;
        stageCompleted = false;
        hasIncrementedStage = false;

        Debug.Log($"[StageProgressByDistance] ResetStartY 呼び出し → startY = {startY}, goalY = {goalY}");
    }
}
