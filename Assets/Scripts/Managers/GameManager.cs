using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Player { get; private set; }
    
    [SerializeField] private string playerTag = "Player";

    private bool isGameOver = false;

    public static event Action OnTitle;
    public static event Action OnGameStart;
    public HealthSystem playerHealthSystem;

    private void Awake()
    {
        // 하나만 생성되도록 관리
        if (Instance != null)
        {
            Destroy(gameObject);
            return;

        }
            
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag);
        Debug.Log(Player.gameObject);
        
    }
    private void OnEnable()
    {
        // 씬 재로드마다 호출
        SceneManager.sceneLoaded += FindGameScene;
    }
    private void Start()
    {
   

    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0.0f;
        Debug.Log("GameOver");
    }
    public void ReLoadGame()
    {
        Time.timeScale = 1.0f;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬을 재로드
    }
    public void FindGameScene(Scene scene, LoadSceneMode mode)
    {
        // 씬 재로드 시에 씬 종류에 따라 다른 이벤트 호출
        switch (scene.name)
        {
            case "TitleScene":
                OnTitle?.Invoke();
                Debug.Log("OnTitle");
                break;
            case "MainScene":
                OnGameStart?.Invoke();
                Debug.Log("OnStart");
                // 메인 씬인 경우 재생성된 플레이어로 바인딩
                Player = GameObject.FindGameObjectWithTag(playerTag);
                break;

        }
    }
    public void StartGame()
    {

        string sceneName = "MainScene";
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            if (scene.name == "MainScene")
            {
                OnGameStart?.Invoke();
                Player = GameObject.FindGameObjectWithTag(playerTag);

                SceneManager.sceneLoaded -= FindGameScene;
            }
        };
    }

   
}