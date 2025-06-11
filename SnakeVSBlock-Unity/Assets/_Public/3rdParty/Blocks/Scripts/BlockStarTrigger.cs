using UnityEngine;

public class BlockStarTrigger : MonoBehaviour
{
    [Tooltip("このブロックがスター発動対象かどうか(BlockSpawnerから設定)")]
    public bool isStarBlock = false;

    [Header("無敵状態マネージャー")]
    public InvincibleManager invincibleManager;

    private bool hasTriggered = false;

    public void OnBlockDestroyed()
    {
        if (hasTriggered)
        {
            return;
        }

        if (!isStarBlock)
        {
           return;
        }

        hasTriggered = true;
        
        if (invincibleManager != null)
        {
            invincibleManager.Activate();
        }
    }
}
