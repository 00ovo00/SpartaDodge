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
    private float currentScore = 0.0f;
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
    public void IncrementScore(float score)
    {
        currentScore += score;
        playerInfoUI.UpdateScore(currentScore);
    }

    public float GetScore()
    {
        return currentScore;
    }
    public float GetBestScore()
    {
        bestScore = PlayerPrefs.GetFloat("BestScore");
        return bestScore;
    }
    public void SetBestScore(float newBestScore)
    {
        bestScore = newBestScore;
        PlayerPrefs.SetFloat("BestScore", bestScore);
    }
    public void ResetData(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드될 때마다 데이터 리셋, UI 바인딩 
        killCount = 0;
        currentScore = 0.0f;
        playerInfoUI = FindObjectOfType<PlayerInfoUI>();
    }
}
