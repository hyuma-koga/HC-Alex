using UnityEngine;

public class CleanupObjects : MonoBehaviour
{
    public float yLimit = 100f;
    public string[] targetTags = { "Block", "PlusOrb" ,"Wall"};

    void Start()
    {
        foreach (string tag in targetTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                if (obj.transform.position.y > yLimit)
                {
                    Debug.Log($"[{tag}] {obj.name} ‚ğ©“®íœiY={obj.transform.position.y}j");
                    Destroy(obj);
                }
            }
        }
    }
}