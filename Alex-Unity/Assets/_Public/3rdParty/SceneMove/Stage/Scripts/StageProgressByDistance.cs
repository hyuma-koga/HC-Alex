using UnityEngine;

public class StageProgressByDistance : MonoBehaviour
{
    [SerializeField] private Transform snakeTransform;
    [SerializeField] private StageProgressController progressController;
    [SerializeField] private float startY = float.NaN;  // ���ݒ��Ԃ̃t���O�Ƃ���NaN���g��
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
                Debug.Log("[StageProgressByDistance] HeadSprite���Q�ƑΏۂɐ؂�ւ��܂���");
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
                Debug.Log("[StageProgressByDistance] �Đ������HeadSprite�ɍĐڑ����܂���");
            }
        }

        // Snake���ړ��\�ȂƂ��̂ݏ���
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
        hasIncrementedStage = false;

        Debug.Log($"[StageProgressByDistance] ResetStartY �Ăяo�� �� startY = {startY}, goalY = {goalY}");
    }
}
