using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform Player { get; private set; }
    public ObjectPool ObjectPool { get; private set; }
    [SerializeField] private string playerTag = "Player";

    private bool isGameOver = false;

    public static event Action OnGameStart;

    private void Awake()
    {
        // 하나만 생성되도록 관리
        if (Instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform;

        ObjectPool = GetComponent<ObjectPool>();
    }
    private void Start()
    {
        HealthSystem playerHealthSystem = Player.GetComponent<HealthSystem>();
        if (playerHealthSystem != null)
            playerHealthSystem.OnGameOver += GameOver;
        OnGameStart?.Invoke();
    }
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0.0f;
        Debug.Log("GameOver");
    }
    public void ReStartGame()
    {
    }
}