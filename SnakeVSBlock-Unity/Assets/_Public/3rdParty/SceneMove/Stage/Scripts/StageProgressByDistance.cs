using UnityEngine;

public class StageProgressByDistance : MonoBehaviour
{
    [SerializeField] private Transform snakeTransform;
    [SerializeField] private StageProgressController progressController;
    [SerializeField] private float startY = 0f;
    [SerializeField] private float goalY = 100f;

    private bool stageCompleted = false;

    private void Start()
    {
        if (snakeTransform != null)
        {
            // HeadSprite を参照対象に変更
            Transform headSprite = snakeTransform.Find("HeadSprite");
            if (headSprite != null)
            {
                snakeTransform = headSprite;
                Debug.Log("[StageProgressByDistance] HeadSpriteを参照対象に切り替えました");
            }

            startY = snakeTransform.position.y;
            goalY = startY + 100f;
            Debug.Log($"[StageProgressByDistance] startY = {startY}, goalY = {goalY}");
        }
    }

    private void Update()
    {
        // 再生成後に SnakeTransform が無効な場合は再接続
        if ((snakeTransform == null || !snakeTransform.gameObject.activeInHierarchy) && SnakeFollowMouse.Instance != null)
        {
            var newHead = SnakeFollowMouse.Instance.transform.Find("HeadSprite");
            if (newHead != null)
            {
                snakeTransform = newHead;
                Debug.Log("[StageProgressByDistance] 再生成後のHeadSpriteに再接続しました");
            }
        }

        if (snakeTransform == null || progressController == null)
        {
            Debug.LogWarning("[StageProgressByDistance] 必須参照が未設定です");
            return;
        }

        float snakeY = snakeTransform.position.y;
        float progress = Mathf.InverseLerp(startY, goalY, snakeY);
        progressController.SetProgress(progress);

        if (!stageCompleted && progress >= 0.999f)
        {
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

        Debug.Log($"[StageProgressByDistance] ResetStartY 呼び出し → startY = {startY}, goalY = {goalY}");
    }
}