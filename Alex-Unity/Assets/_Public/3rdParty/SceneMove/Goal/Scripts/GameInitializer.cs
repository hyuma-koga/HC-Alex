using UnityEngine;
using TMPro;

public enum ResetReason
{
    GameOver,
    GameClear,
    Restart
}

public class GameInitializer : MonoBehaviour
{
    public SnakeFollowMouse snake;
    public BlockSpawner blockSpawner;
    public PlusOrbSpawner orbSpawner;
    public WallSpawner wallSpawner;
    public BlockDestroyManager destroyManager;
    public CameraFollow cameraFollow;
    public Vector3 playerStartPos = new Vector3(0, 0, -5);

    public SnakeFollowMouse GetCurrentSnake() => snake;

    public void ResetToStartState(ResetReason reason)
    {
        if (snake == null)
        {
            return;
        }

        //snakeCountText が失われている可能性があるため再設定
        if (snake.snakeCountText == null)
        {
            var textObj = GameObject.Find("SnakeCountText");
            if (textObj != null)
            {
                snake.snakeCountText = textObj.GetComponent<TextMeshPro>();
            }
        }

        // Snake 初期化（Tailや位置のリセットも含む）
        snake.ResetState(reason, playerStartPos);
        snake.ForceUpdateSnakeCountUI();

        if (reason == ResetReason.GameOver)
        {
            Debug.Log("GameOver用初期化");

            // スコア初期化（GameOver時のみ）
            destroyManager?.ResetScore();

            // 壁だけ削除
            wallSpawner?.ClearWallsOnly();

            // 🔻 ブロック全削除してリセット（内部で1列生成含む）
            blockSpawner?.ClearAllBlocks();
        }

        // Snakeの位置を元にブロック再生成（必ず最後に呼ぶ）
        blockSpawner?.ResetSpawner();

        // オーブ再生成
        orbSpawner?.ResetSpawner();

        // 壁の再配置
        wallSpawner?.ResetWalls();

        // カメラ追従の再設定
        if (cameraFollow != null && snake.head != null)
        {
            cameraFollow.target = snake.head;
        }

        // 最終的なUI更新（HPテキストなど）
        snake.ForceUpdateSnakeCountUI();
    }

    public void ResetSnakePosition()
    {
        if (snake != null)
        {
            snake.ResetSnakePosition(playerStartPos);
        }
    }
}
