using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int CurrentScore { get; private set; } = 0;
    public int TotalScore { get; private set; } = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        TotalScore += amount;
        PlayerPrefs.SetInt("TotalScore", TotalScore); // ‰i‘±•Û‘¶
    }

    public void ResetCurrentScore()
    {
        CurrentScore = 0;
    }
}
