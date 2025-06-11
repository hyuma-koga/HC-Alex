using UnityEngine;
using System;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public event Action<int> OnStageChanged;

    private int currentStage = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        OnStageChanged?.Invoke(currentStage);
    }

    public void IncrementStage()
    {
        currentStage++;
        OnStageChanged?.Invoke(currentStage);
    }

    public void ResetStage()
    {
        currentStage = 1;
        OnStageChanged?.Invoke(currentStage);
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }
}
