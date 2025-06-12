using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeFollowMouse : MonoBehaviour
{
    public Transform head;
    public Transform tailRoot;
    public GameObject tailPrefab;
    public TextMeshPro snakeCountText;
    public InvincibleManager invincibleManager;
    public GameObject headPrefab;

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

    private float defaultMoveSpeed;
    private float defaultScrollSpeed;

    private void OnEnable()
    {
        // 再生成後にゲームオーバーフラグをリセット
        isGameOver = false;
    }

    private void Start()
    {
        segments.Clear(); //念のため segments をリセット

        if (head == null)
        {
            var found = transform.Find("HeadSprite");
            if (found != null)
            {
                head = found;
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

    private void Awake()
    {
        defaultMoveSpeed = moveSpeed;
        defaultScrollSpeed = yScrollSpeed;
    }

    private void Update()
    {
        if (!canMove || isGameOver)
        {
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float yDelta = isStoppedY ? 0f : yScrollSpeed * Time.deltaTime;

        Vector3 intendedPos = new Vector3(
            Mathf.Clamp(mousePos.x, -2.88f, 2.88f),
            head.position.y + yDelta,
            0f
        );

        Vector3 proposedMove = intendedPos - head.position;
        Vector3 nextPos = head.position;

        //個別にX, Yの動きについて判定
        Vector3 xMove = new Vector3(proposedMove.x, 0, 0);
        Vector3 yMove = new Vector3(0, proposedMove.y, 0);

        //X方向に壁がなければ移動
        if (!IsWallInFront(head.position + xMove))
        {
            nextPos += xMove;
        }

        //Y方向に壁がなければ移動（通常はtrue）
        if (!IsWallInFront(head.position + yMove))
        {
            nextPos += yMove;
        }

        head.position = Vector3.Lerp(head.position, nextPos, moveSpeed * Time.deltaTime);

        //tail追従
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
        UpdateSnakeCountUI();
    }

    public void RemoveTail()
    {
        if (segments.Count > 1)
        {
            Transform last = segments[segments.Count - 1];
            segments.RemoveAt(segments.Count - 1);
            Destroy(last.gameObject);
            UpdateSnakeCountUI();
            ScoreManager.Instance.AddScore(1);
        }
        else if (segments.Count == 1)
        {
            // Headを削る
            Transform last = segments[0];
            segments.RemoveAt(0);
            Destroy(last.gameObject);
            UpdateSnakeCountUI();
            ScoreManager.Instance.AddScore(1);
            OnGameOver();
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
        if (snakeCountText == null)
        {
            return;
        }

        if (head == null)
        {
            return;
        }

        snakeCountText.text = segments.Count.ToString();
        snakeCountText.transform.position = head.position + new Vector3(0f, 0.5f, 0f);
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
        if (segments.Count <= 1)
        {
            return;
        }

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
        if (head != null)
        {
            return;
        }

        GameObject newHead = GameObject.Find("HeadSprite");

        if (newHead == null && headPrefab != null)
        {
            Vector3 safeStartPos = new Vector3(0f, 0f, 0f);
            newHead = Instantiate(headPrefab, safeStartPos, Quaternion.identity, transform);
            newHead.name = "HeadSprite";
        }

        if (newHead == null)
        {
            return;
        }

        head = newHead.transform;
        head.position = new Vector3(0f, 0f, 0f);

        if (snakeCountText == null)
        {
            snakeCountText = head.GetComponentInChildren<TextMeshPro>();
        }

        if (segments.Count == 0)
        {
            segments.Add(head);
        }
        else if (segments[0] != head)
        {
            segments.Insert(0, head);
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        moveSpeed = defaultMoveSpeed * multiplier;
        yScrollSpeed = defaultScrollSpeed * multiplier;
    }

    public void ResetSpeed()
    {
        moveSpeed = defaultMoveSpeed;
        yScrollSpeed = defaultScrollSpeed;
    }

    public List<Transform> GetAllSegments()
    {
        return new List<Transform>(segments);
    }

    private bool IsWallInFront(Vector3 targetPos)
    {
        Vector2 origin = head.position;
        Vector2 direction = (targetPos - head.position).normalized;
        float distance = Vector2.Distance(origin, targetPos);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask("Wall"));
        return hit.collider != null;
    }

    public void ResetState(ResetReason reason, Vector3 pos)
    {
        if (reason == ResetReason.GameOver)
        {
            RecreateHead();

            if (head == null)
            {
                return;
            }

            ClearAllTail();

            for (int i = 0; i < 9; i++) // HP10
            {
                AddTail();
            }

            UpdateSnakeCountUI();
            isGameOver = false;
        }

        ResetSnakePosition(pos);
        UpdateSnakeCountUI();
        StartCoroutine(DelayUIUpdate());
    }

    private System.Collections.IEnumerator DelayUIUpdate()
    {
        yield return null;
        UpdateSnakeCountUI();
    }

    public void ForceUpdateSnakeCountUI()
    {
        UpdateSnakeCountUI();
    }
}
