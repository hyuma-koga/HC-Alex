using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float yOffset = 2f;
    public float followSpeed = 5f;

    private float fixedX;
    private float fixedZ;

    private void Start()
    {
        fixedX = transform.position.x;
        fixedZ = transform.position.z;
    }

    private void LateUpdate()
    {
        if(target == null)
        {
            return;
        }

        Vector3 targetPosition = new Vector3(
            fixedX,
            target.position.y + yOffset,
            fixedZ
            );

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
