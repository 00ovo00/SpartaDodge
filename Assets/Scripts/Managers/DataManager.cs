using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    private PlayerInfoUI playerInfoUI;
    public event Action<int> OnKillCountChanged;

    private int killCount = 0;
    private float score = 0.0f;
    private float bestScore = 0.0f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;

        playerInfoUI = FindObjectOfType<PlayerInfoUI>();
    }

    public void IncrementKillCount()
    {
        killCount++;
        Debug.Log($"KillCount: {killCount}");
        OnKillCountChanged?.Invoke(killCount);
    }

    public int GetKillCount()
    {
        return killCount;
    }
    public void IncrementScore(float maxHp)
    {
        score += maxHp * 0.5f;
        playerInfoUI.UpdateScore(score);
    }

    public float GetScore()
    {
        return score;
    }
    public float GetBestScore()
    {
        return bestScore;
    }
    public void SetBestScore(float newBestScore)
    {
        bestScore = newBestScore;
    }
}
