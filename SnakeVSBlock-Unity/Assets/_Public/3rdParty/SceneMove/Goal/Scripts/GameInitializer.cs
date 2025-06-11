using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public SnakeFollowMouse snake;
    public BlockSpawner blockSpawner;
    public PlusOrbSpawner orbSpawner;
    public BlockDestroyManager destroyManager;
    public Vector3 playerStartPos = new Vector3(0, 0, -5);
    public SnakeFollowMouse GetCurrentSnake() => snake;

    public void ResetToStartState(bool isGameOver)
    {
        if (snake == null)
        {
            Debug.LogWarning("snake �����ݒ�ł�");
            return;
        }

        if (isGameOver)
        {
            Debug.Log("GameOver�p������");
            snake.ClearAllTail(); // tail �̂ݍ폜
        }

        snake.ResetSnakePosition(playerStartPos);

        if (isGameOver) 
        {
            destroyManager?.ResetScore();
        }
            
        blockSpawner?.ResetSpawner();
        orbSpawner?.ResetSpawner();
    }
}
