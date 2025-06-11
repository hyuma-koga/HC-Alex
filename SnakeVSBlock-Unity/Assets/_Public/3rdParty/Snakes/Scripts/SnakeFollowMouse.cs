using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeFollowMouse : MonoBehaviour
{
    public Transform head;
    public Transform tailRoot;
    public GameObject tailPrefab;
    public TextMeshPro snakeCountText;

    public int tailCount = 10;
    public float spacing = 0.4f;
    public float moveSpeed = 110f;
    public float yScrollSpeed = 10f;
    public bool canMove = false;

    public int GetTailCountUI() => segments.Count - 1;
    public int GetSegmentCount() => segments.Count;
    public void StopScrollY() => yScrollSpeed = 0f;
    public void ResumeScrollY() => yScrollSpeed = 10f;

    private List<Transform> segments = new List<Transform>();
    private bool isGameOver = false;
    private bool isStoppedY = false;
    private BlockCollision currentBlock = null;

    void OnEnable()
    {
        // 再生成後にゲームオーバーフラグをリセット
        isGameOver = false;
    }

    void Start()
    {
        segments.Clear(); // 念のため segments をリセット

        if (head == null)
        {
            var found = transform.Find("HeadSprite");
            if (found != null)
            {
                head = found;
            }
            else
            {
                Debug.LogWarning("HeadSprite が見つかりません！");
            }
        }

        segments.Add(head);

        for (int i = 0; i < tailCount; i++)
        {
            GameObject tail = Instantiate(tailPrefab, tailRoot);
            tail.transform.position = head.position - Vector3.up * spacing * (i + 1);
            segments.Add(tail.transform);
        }

        UpdateSnakeCountUI(); // 初回表示も追加
    }

    void Update()
    {
        if (!canMove || isGameOver) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float yDelta = isStoppedY ? 0f : yScrollSpeed * Time.deltaTime;

        Vector3 targetPos = new Vector3(
            Mathf.Clamp(mousePos.x, -2.88f, 2.88f),
            head.position.y + yDelta,
            0f
        );

        head.position = Vector3.Lerp(head.position, targetPos, moveSpeed * Time.deltaTime);

        for (int i = 1; i < segments.Count; i++)
        {
            Transform prev = segments[i - 1];
            Transform curr = segments[i];
            Vector3 dir = curr.position - prev.position;
            Vector3 targetPosSeg = prev.position + dir.normalized * spacing;
            curr.position = Vector3.MoveTowards(curr.position, targetPosSeg, moveSpeed * Time.deltaTime);
        }

        UpdateSnakeCountUI();
    }

    public void AddTail()
    {
        int index = segments.Count;
        float y = head.position.y - spacing * index;
        Vector3 newPos = new Vector3(head.position.x, y, 0f);
        GameObject newTail = Instantiate(tailPrefab, newPos, Quaternion.identity, tailRoot);
        segments.Add(newTail.transform);
    }

    public void RemoveTail()
    {
        if (segments.Count > 0)
        {
            Transform last = segments[segments.Count - 1];
            segments.RemoveAt(segments.Count - 1);
            Destroy(last.gameObject);
            UpdateSnakeCountUI();

            if (segments.Count == 0)
            {
                OnGameOver();
            }
        }
    }

    private void OnGameOver()
    {
        isGameOver = true;
        canMove = false;

        var gm = FindFirstObjectByType<GameOverManager>();
        if (gm != null)
        {
            gm.ShowGameOver();
        }
    }

    private void UpdateSnakeCountUI()
    {
        if (snakeCountText != null && head != null)
        {
            snakeCountText.text = segments.Count.ToString();
            Vector3 offset = new Vector3(0f, 0.5f, 0f);
            snakeCountText.transform.position = head.position + offset;
        }
    }

    public void SetCurrentBlock(BlockCollision block)
    {
        if (currentBlock != null && currentBlock != block)
        {
            currentBlock.ForceStopDamage();
        }
        currentBlock = block;
    }

    public void ClearCurrentBlock(BlockCollision block)
    {
        if (currentBlock == block)
        {
            currentBlock = null;
        }
    }

    public void ResetSnakePosition(Vector3 pos)
    {
        if (head != null)
        {
            head.position = pos;
        }

        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].position = pos - Vector3.up * spacing * i;
        }
    }

    public void ClearAllTail()
    {
        if (segments.Count <= 1) return;

        for (int i = segments.Count - 1; i >= 1; i--)
        {
            if (segments[i] != null)
            {
                Destroy(segments[i].gameObject);
            }
        }

        segments.RemoveRange(1, segments.Count - 1);
        UpdateSnakeCountUI();
    }

    public void RecreateHead()
    {
        if (head != null) return;

        var newHead = GameObject.Find("HeadSprite");
        if (newHead == null)
        {
            return;
        }

        head = newHead.transform;
        segments.Insert(0, head);
    }
}
