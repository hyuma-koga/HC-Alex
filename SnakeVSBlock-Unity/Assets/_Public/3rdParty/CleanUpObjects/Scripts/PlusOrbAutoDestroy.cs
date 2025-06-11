using UnityEngine;

public class PlusOrbAutoDestroy : MonoBehaviour
{
    public bool isAutoSpawned = true;
    private Camera mainCamera;
    private float offsetBelowView = 6f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!isAutoSpawned) return;

        float screenBottomY = mainCamera.transform.position.y - offsetBelowView;

        if (transform.position.y < screenBottomY)
        {
            Destroy(gameObject);
        }
    }
}
