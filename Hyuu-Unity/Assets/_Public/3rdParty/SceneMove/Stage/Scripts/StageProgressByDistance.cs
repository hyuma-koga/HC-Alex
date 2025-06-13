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
            // HeadSprite ���Q�ƑΏۂɕύX
            Transform headSprite = snakeTransform.Find("HeadSprite");
            if (headSprite != null)
            {
                snakeTransform = headSprite;
                Debug.Log("[StageProgressByDistance] HeadSprite���Q�ƑΏۂɐ؂�ւ��܂���");
            }

            startY = snakeTransform.position.y;
            goalY = startY + 100f;
            Debug.Log($"[StageProgressByDistance] startY = {startY}, goalY = {goalY}");
        }
    }

    private void Update()
    {
        // �Đ������ SnakeTransform �������ȏꍇ�͍Đڑ�
        if ((snakeTransform == null || !snakeTransform.gameObject.activeInHierarchy) && SnakeFollowMouse.Instance != null)
        {
            var newHead = SnakeFollowMouse.Instance.transform.Find("HeadSprite");
            if (newHead != null)
            {
                snakeTransform = newHead;
                Debug.Log("[StageProgressByDistance] �Đ������HeadSprite�ɍĐڑ����܂���");
            }
        }

        if (snakeTransform == null || progressController == null)
        {
            Debug.LogWarning("[StageProgressByDistance] �K�{�Q�Ƃ����ݒ�ł�");
            return;
        }

        float snakeY = snakeTransform.position.y;
        float progress = Mathf.InverseLerp(startY, goalY, snakeY);
        progressController.SetProgress(progress);

        if (!stageCompleted && progress >= 0.999f)
        {
            stageCompleted = true;
            Debug.Log("[StageProgressByDistance] �S�[�����B �� �X�e�[�W�i�s");
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

        Debug.Log($"[StageProgressByDistance] ResetStartY �Ăяo�� �� startY = {startY}, goalY = {goalY}");
    }
}