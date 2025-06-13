using UnityEngine;
using System.Collections;

public class BlockCollision : MonoBehaviour
{
    public BlockDestroyManager destroyManager;
    private BlockHP blockHP;
    private Coroutine damageCoroutine;
    private SnakeFollowMouse currentSnake;
    private int initialBlockHP;

    private void Start()
    {
        if (!CompareTag("Block"))
        {
            Destroy(this);
            return;
        }

        blockHP = GetComponent<BlockHP>();
    }

    public void SetInitialHP(int hp)
    {
        initialBlockHP = hp;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("HeadSprite"))
        {
            return;
        }

        SnakeFollowMouse newSnake = other.GetComponentInParent<SnakeFollowMouse>();
        if (newSnake == null || blockHP == null)
        {
            return;
        }

        newSnake.SetCurrentBlock(this);

        if (damageCoroutine == null)
        {
            currentSnake = newSnake;
            damageCoroutine = StartCoroutine(DamageOverTime(currentSnake));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("HeadSprite"))
        {
            return;
        }

        SnakeFollowMouse exitSnake = other.GetComponentInParent<SnakeFollowMouse>();
        if (exitSnake == currentSnake)
        {
            ForceStopDamage();
        }
    }

    private IEnumerator DamageOverTime(SnakeFollowMouse snake)
    {
        snake.StopScrollY();
        bool isInvincible = snake.invincibleManager != null && snake.invincibleManager.IsInvincible;

        if (isInvincible)
        {
            int currentHP = blockHP.GetHP();
            blockHP.ReduceHP(currentHP);

            if (destroyManager != null)
            {
                destroyManager.AddDestroyedHP(currentHP);
            }

            ScoreManager.Instance.AddScore(currentHP);

            if (TryGetComponent(out BlockStarTrigger starTrigger))
            {
                starTrigger.OnBlockDestroyed();
            }

            Destroy(gameObject);
            snake.ResumeScrollY();
            snake.ClearCurrentBlock(this);
            damageCoroutine = null;
            currentSnake = null;
            yield break;
        }

        while (snake.GetSegmentCount() > 0 && blockHP.GetHP() > 0)
        {
            snake.RemoveTail();
            blockHP.ReduceHP(1);

            if (destroyManager != null)
            {
                destroyManager.AddDestroyedHP(1);
            }

            if (blockHP.GetHP() <= 0)
            {
                if (TryGetComponent(out BlockStarTrigger starTrigger))
                {
                    starTrigger.OnBlockDestroyed();
                }

                Destroy(gameObject);
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        // 後処理（Scroll再開など）
        snake.ResumeScrollY();
        snake.ClearCurrentBlock(this);
        damageCoroutine = null;
        currentSnake = null;
    }

    public void ForceStopDamage()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;

            if (currentSnake != null)
            {
                currentSnake.ResumeScrollY();
                currentSnake.ClearCurrentBlock(this);
                currentSnake = null;
            }
        }
    }
}