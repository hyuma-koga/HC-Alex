using UnityEngine;
using System.Collections.Generic;

public class PlusOrbSpawner : MonoBehaviour
{
    public GameObject plusOrbPrefab;
    public float spawnSpacing = 5f;
    public float aheadDistance = 2f;
    public int maxPerLine = 4;
    public bool canSpawn = false;

    private Transform playerHead;
    private float nextSpawnY;

    private readonly float[] fixedXPositions = { -1.92f, -0.96f, 0f, 0.96f, 1.92f };
    private static HashSet<float> spawnedYSet = new HashSet<float>();

    private void Start()
    {
        SnakeFollowMouse snake = FindFirstObjectByType<SnakeFollowMouse>();
        if (snake != null && snake.head != null)
        {
            playerHead = snake.head;
            nextSpawnY = Mathf.Floor(playerHead.position.y / spawnSpacing) * spawnSpacing + spawnSpacing;
        }
    }

    private void Update()
    {
        if (!canSpawn || playerHead == null)
        {
            return;
        }

        float currentY = playerHead.position.y;

        while (nextSpawnY <= currentY + aheadDistance)
        {
            TrySpawnAtY(nextSpawnY + 6f);
            nextSpawnY += spawnSpacing;
        }
    }

    void TrySpawnAtY(float y)
    {
        if (y >= 100f) return;

        float roundedY = Mathf.Round(y * 10f) / 10f;
        if (spawnedYSet.Contains(roundedY)) return;

        spawnedYSet.Add(roundedY);

        int orbCount = Random.value < 0.8f ? Random.Range(1, 3) : Random.Range(3, maxPerLine + 1);

        List<int> availableIndices = new List<int>() { 0, 1, 2, 3, 4 };
        for (int i = 0; i < orbCount && availableIndices.Count > 0; i++)
        {
            int randIndex = Random.Range(0, availableIndices.Count);
            float x = fixedXPositions[availableIndices[randIndex]];
            availableIndices.RemoveAt(randIndex);

            Vector3 spawnPos = new Vector3(x, roundedY, 0f);
            Instantiate(plusOrbPrefab, spawnPos, Quaternion.identity);
        }
    }

    public void ResetSpawner()
    {
        if (playerHead == null)
        {
            return;
        }

        spawnedYSet.Clear();
        nextSpawnY = Mathf.Floor(playerHead.position.y / spawnSpacing) * spawnSpacing + spawnSpacing;
    }
}
