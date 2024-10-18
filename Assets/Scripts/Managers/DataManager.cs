using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private PlayerInfoUI playerInfoUI;

    private int killCount = 0;
    private float score = 0.0f;

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
}
