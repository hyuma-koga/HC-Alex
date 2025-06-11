using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("HeadSprite")) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            // velocity Å® linearVelocity Ç…ïœçX
            rb.linearVelocity = Vector2.zero;

            Vector3 pushBack = other.transform.position - transform.position;
            pushBack.z = 0;
            rb.MovePosition(rb.position + (Vector2)(pushBack.normalized * 0.05f));
        }
    }

}
