using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameInitializer initializer;
    public ClickToStart startScreen;

    private bool isGameOver = false;

    public void ShowGameOver()
    {
        isGameOver = true;
    }

    private void Update()
    {
        if (isGameOver && Input.GetMouseButtonDown(0))
        {
            isGameOver = false;

            initializer?.ResetToStartState(true); // Snake�̈ʒu��Tail�̂݃��Z�b�g
            startScreen?.RestartStartScreen();
        }
    }
}
