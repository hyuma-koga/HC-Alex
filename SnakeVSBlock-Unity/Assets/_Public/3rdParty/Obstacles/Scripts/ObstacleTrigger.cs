using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Untagged")) // SnakeのHeadにのみ反応
        {
            // 例えばここで止める処理や、左右に回避させるなど
            SnakeFollowMouse snake = other.GetComponentInParent<SnakeFollowMouse>();
            if (snake != null)
            {
                snake.StopScrollY(); // Y方向の移動停止（または任意の回避ロジック）
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Untagged"))
        {
            SnakeFollowMouse snake = other.GetComponentInParent<SnakeFollowMouse>();
            if (snake != null)
            {
                snake.ResumeScrollY();
            }
        }
    }
}
