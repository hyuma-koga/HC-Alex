using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ObstacleSetup : MonoBehaviour
{
    void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.simulated = true;
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
    }
}

