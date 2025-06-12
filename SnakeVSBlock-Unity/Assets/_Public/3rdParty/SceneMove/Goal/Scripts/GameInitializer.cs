using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public SnakeFollowMouse snake;
    public BlockSpawner blockSpawner;
    public PlusOrbSpawner orbSpawner;
    public WallSpawner wallSpawner;
    public BlockDestroyManager destroyManager;
    public Vector3 playerStartPos = new Vector3(0, 0, -5);
    public SnakeFollowMouse GetCurrentSnake() => snake;

    public void ResetToStartState(bool isGameOver)
    {
        if (snake == null)
        {
            Debug.LogWarning("snake Ç™ñ¢ê›íËÇ≈Ç∑");
            return;
        }

        if (isGameOver)
        {
            Debug.Log("GameOverópèâä˙âª");
            snake.ClearAllTail();

            if(wallSpawner != null)
            {
                wallSpawner.HideAllWalls();
            }
        }

        snake.ResetSnakePosition(playerStartPos);

        if (isGameOver) 
        {
            destroyManager?.ResetScore();
        }
            
        blockSpawner?.ResetSpawner();
        orbSpawner?.ResetSpawner();
        wallSpawner?.ResetWalls();
    }

    public void ResetSnakePosition()
    {
        if (snake != null)
        {
            snake.ResetSnakePosition(playerStartPos);
        }
    }
}
