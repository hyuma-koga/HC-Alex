using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Untagged")) // Snake��Head�ɂ̂ݔ���
        {
            // �Ⴆ�΂����Ŏ~�߂鏈����A���E�ɉ��������Ȃ�
            SnakeFollowMouse snake = other.GetComponentInParent<SnakeFollowMouse>();
            if (snake != null)
            {
                snake.StopScrollY(); // Y�����̈ړ���~�i�܂��͔C�ӂ̉�����W�b�N�j
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
