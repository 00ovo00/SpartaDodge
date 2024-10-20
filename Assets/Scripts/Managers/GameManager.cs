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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void FindGameScene(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "TitleSceneHSH":
                OnTitle?.Invoke();
                Debug.Log("OnTitle");
                break;
            case "TestSceneHSH":
                OnGameStart?.Invoke();
                Debug.Log("OnStart");
                Player = GameObject.FindGameObjectWithTag(playerTag);
                break;

        }
    }
    public void StartGame()
    {


        string sceneName = "TestSceneHSH";
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            if (scene.name == "TestSceneHSH")
            {
                OnGameStart?.Invoke();
                Player = GameObject.FindGameObjectWithTag(playerTag);

                SceneManager.sceneLoaded -= FindGameScene;
            }
        };
    }

   
}