using UnityEngine;

public class ObstacleBlocker : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("HeadSprite"))
        {
            return;
        }

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic)
        {
            return;
        }

        Vector3 pushBack = other.transform.position - transform.position;
        pushBack.z = 0;

        // è≠ÇµÇ∏Ç¬âüÇµï‘Ç∑
        rb.MovePosition(rb.position + (Vector2)(pushBack.normalized * 0.05f));
    }
}
