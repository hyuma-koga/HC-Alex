using UnityEngine;
using System.Collections.Generic;

public class WallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float startY = 10f;
    [SerializeField] private int maxRows = 20;

    private float lastY;
    private float[] possibleX = { -1.728f, -0.576f, 0.576f, 1.728f };

    private void Start()
    {
        lastY = startY;
        for (int i = 0; i < maxRows; i++)
        {
            SpawnWallRow();
        }
    }

    private void SpawnWallRow()
    {
        float randomYIncrement = Random.Range(5f, 10f);
        lastY += randomYIncrement;

        List<float> xList = new List<float>(possibleX);
        ShuffleList(xList);
        int wallCount = Random.Range(1, 5);

        for (int i = 0; i < wallCount; i++)
        {
            Vector3 spawnPos = new Vector3(xList[i], lastY, 0f);
            Instantiate(wallPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }
}
