using UnityEngine;
using System.Collections;

public class BlockCollision : MonoBehaviour
{
    private BlockHP blockHP;
    private Coroutine damageCoroutine;
    private SnakeFollowMouse currentSnake;

    private int initialBlockHP;

    public BlockDestroyManager destroyManager;

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
        if (!other.CompareTag("HeadSprite")) return;

        SnakeFollowMouse exitSnake = other.GetComponentInParent<SnakeFollowMouse>();
        if (exitSnake == currentSnake)
        {
            ForceStopDamage();
        }
    }

    private IEnumerator DamageOverTime(SnakeFollowMouse snake)
    {
        snake.StopScrollY();

        while (snake.GetSegmentCount() > 0 && blockHP.GetHP() > 0)
        {
            snake.RemoveTail();
            blockHP.ReduceHP(1);

            if (destroyManager != null)
            {
                destroyManager.AddDestroyedHP(1); //削ったHPを即加算
            }

            yield return new WaitForSeconds(0.1f);
        }

        // HP0にならなくても終わる
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
