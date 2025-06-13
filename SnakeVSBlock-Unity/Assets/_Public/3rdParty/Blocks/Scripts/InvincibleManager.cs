using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleManager : MonoBehaviour
{
    [SerializeField] private SnakeFollowMouse snakeMovement;
    [SerializeField] private Color[] rainbowColors;
    [SerializeField] private float duration = 5f;

    public bool IsInvincible { get; private set; } = false;

    private Coroutine rainbowEffectCoroutine;

    // 無敵終了後に戻す固定色（EAFF00）
    private static readonly Color restoreColor = new Color32(0xEA, 0xFF, 0x00, 0xFF); // #EAFF00

    public void Activate()
    {
        if (IsInvincible)
        {
            return;
        }
          
        IsInvincible = true;
        snakeMovement.SetSpeedMultiplier(2f);
        rainbowEffectCoroutine = StartCoroutine(RainbowEffect());
        StartCoroutine(ResetAfterTime());
    }

    private IEnumerator ResetAfterTime()
    {
        yield return new WaitForSeconds(duration);
        IsInvincible = false;
        snakeMovement.ResetSpeed();

        if (rainbowEffectCoroutine != null) 
        {
            StopCoroutine(rainbowEffectCoroutine);
        }
            
        RestoreAllToFixedColor();
    }

    private IEnumerator RainbowEffect()
    {
        int index = 0;

        while (IsInvincible)
        {
            Color currentColor = rainbowColors[index % rainbowColors.Length];
            index++;

            ApplyColorToAllSegments(currentColor);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ApplyColorToAllSegments(Color color)
    {
        foreach (Transform segment in snakeMovement.GetAllSegments())
        {
            var sr = segment.GetComponentInChildren<SpriteRenderer>();
           
            if (sr != null) 
            {
                sr.color = color;
            } 
        }
    }

    private void RestoreAllToFixedColor()
    {
        foreach (Transform segment in snakeMovement.GetAllSegments())
        {
            var sr = segment.GetComponentInChildren<SpriteRenderer>();
            
            if (sr != null) 
            {
                sr.color = restoreColor;
            }
        }
    }
}
