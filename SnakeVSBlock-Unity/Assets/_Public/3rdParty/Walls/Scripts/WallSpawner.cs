using UnityEngine;
using System.Collections.Generic;

public class WallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float previewOffset = 10f;
    [SerializeField] private float spawnYOffset = 8f;
    [SerializeField] private float spawnSpacingY = 4f;
    [SerializeField] private float maxY = 100f;
    [SerializeField] private SnakeFollowMouse snake;

    private float lastSpawnY;
    private readonly float[] possibleX = { -1.57f, -0.5f, 0.68f, 1.8f };
    private List<GameObject> spawnedWalls = new List<GameObject>();

    private void Start()
    {
        if (snake == null || snake.head == null)
        {
            Debug.LogWarning("Snake が未設定のため WallSpawner を無効化します");
            enabled = false;
            return;
        }

        lastSpawnY = snake.transform.position.y + spawnSpacingY;
        float firstSpawnY = lastSpawnY + spawnYOffset;
        SpawnWallRow(firstSpawnY);
    }

    private void Update()
    {
        if (snake == null || snake.head == null)
        {
            return; //head が破棄されていたら早期リターン
        }

        float snakeY = snake.head.position.y;
        float nextSpawnY = lastSpawnY + spawnSpacingY;

        if ((snakeY + previewOffset) >= nextSpawnY && nextSpawnY < maxY)
        {
            SpawnWallRow(nextSpawnY);
            lastSpawnY = nextSpawnY; //ここで正しく進めていく
        }
    }

    private void SpawnWallRow(float spawnY)
    {
        List<float> xList = new List<float>(possibleX);
        ShuffleList(xList);
        int wallCount = Random.Range(1, 5);

        for (int i = 0; i < wallCount; i++)
        {
            Vector3 spawnPos = new Vector3(xList[i], spawnY, 0f);
            GameObject wall = Instantiate(wallPrefab, spawnPos, Quaternion.identity);
            spawnedWalls.Add(wall);
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

    public void ResetWalls()
    {
        // 全削除
        foreach (var wall in spawnedWalls)
        {
            if (wall != null) 
            {
                Destroy(wall);
            }
        }

        spawnedWalls.Clear();

        if (snake != null)
        {
            float snakeY = snake.transform.position.y;
            lastSpawnY = snakeY + spawnYOffset; // ← ★ ここも確実にスタート位置に更新
            SpawnWallRow(lastSpawnY);
        }
    }

    public void ClearWallsOnly()
    {
        foreach (var wall in spawnedWalls)
        {
            if (wall != null) 
            {
                Destroy(wall);
            }
        }
        spawnedWalls.Clear();
    }

    public void HideAllWalls()
    {
        foreach (var wall in spawnedWalls)
        {
            if (wall != null) 
            {
                wall.SetActive(false);
            }
        }
    }
}
