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
            Debug.LogWarning("snake ‚ª–¢İ’è‚Å‚·");
            return;
        }

        if (isGameOver)
        {
            Debug.Log("GameOver—p‰Šú‰»");
            snake.ClearAllTail(); // tail ‚Ì‚İíœ
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
