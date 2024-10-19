using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public PlayerInfoUI playerInfoUI;
    public event Action<int> OnKillCountChanged;
    public static event Action OnEnemyDeath;

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
    private void OnEnable()
    {
        SceneManager.sceneLoaded += ResetData;
    }

    public void IncrementKillCount()
    {
        killCount++;
        Debug.Log($"KillCount: {killCount}");
        OnKillCountChanged?.Invoke(killCount);
        OnEnemyDeath?.Invoke();
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
    public void ResetData(Scene scene, LoadSceneMode mode)
    {
        killCount = 0;
        score = 0.0f;
        playerInfoUI = FindObjectOfType<PlayerInfoUI>();
    }
}
