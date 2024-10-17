using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private int killCount = 0;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
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
}
