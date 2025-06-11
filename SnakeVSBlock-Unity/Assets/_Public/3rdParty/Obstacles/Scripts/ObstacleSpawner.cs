using System.Collections.Generic;
using UnityEngine;

public class ObstancleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float spawnY = 10f;

    private readonly float[] xPositions = { -1.728f, -0.576f, 0.576f, 1.728f };

    private void Start()
    {
        SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        int spawnCount = Random.Range(1, xPositions.Length + 1);

        //x座標をシャッフルして、先頭からspawnCount個を仕様
        List<float> shuffledX = new List<float>(xPositions);
        Shuffle(shuffledX);

        for(int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = new Vector3(shuffledX[i], spawnY, 0f);
            Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        }
    }

    private void Shuffle(List<float> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            float temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }
}
