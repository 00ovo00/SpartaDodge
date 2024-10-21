using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private HealthSystem healthSystem;

    [SerializeField] private AudioSource bgmSource;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private AudioClip playBGM;
    [SerializeField] private AudioClip gameOverBGM;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip playerAttackSFX;
    [SerializeField] private AudioClip enemyDeathSFX;

    [Header("Object Pool")]
    [SerializeField] private ObjectPool objectPool;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        objectPool = GetComponent<ObjectPool>();
    }
    private void OnEnable()
    {
        GameManager.OnTitle -= PlayTitleBGM;
        GameManager.OnTitle += PlayTitleBGM;
        SceneManager.sceneLoaded += ReSetBinding;
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void PlaySFX(string poolTag, AudioClip clip)
    {
        // 효과음은 오브젝트 풀링하여 관리
        GameObject audioObject = objectPool.SpawnFromPool(poolTag);

        if (audioObject != null)
        {
            AudioSource source = audioObject.GetComponent<AudioSource>();
            if (source != null)
            {
                source.clip = clip;
                source.Play();
            }
        }
    }

    // 다양한 게임 이벤트에 맞춰 사운드를 재생하는 함수
    private void PlayTitleBGM() => PlayBGM(titleBGM);
    private void PlayPlayBGM() => PlayBGM(playBGM);
    private void PlayGameOverBGM() => PlayBGM(gameOverBGM);
    private void PlayPlayerAttackSFX() => PlaySFX("PlayerAttackSFX" ,playerAttackSFX);
    private void PlayEnemyDeathSFX() => PlaySFX("EnemyDeathSFX", enemyDeathSFX);


    public void ReSetBinding(Scene scene, LoadSceneMode mode)
    {
        // 씬이 재로드마다 실행
        // 플레이어와 체력 시스템을 찾고, 사운드 재생을 위한 이벤트 바인딩
        if (scene.name != "TitleScene")
        {
            playerInputController = FindObjectOfType<PlayerInputController>();
            healthSystem = FindObjectOfType<HealthSystem>();

            GameManager.OnGameStart -= PlayPlayBGM;
            GameManager.OnGameStart += PlayPlayBGM;
            healthSystem.OnGameOver -= PlayGameOverBGM;
            healthSystem.OnGameOver += PlayGameOverBGM;
            playerInputController.OnPlayerAttack -= PlayPlayerAttackSFX;
            playerInputController.OnPlayerAttack += PlayPlayerAttackSFX;
            DataManager.OnEnemyDeath -= PlayEnemyDeathSFX;
            DataManager.OnEnemyDeath += PlayEnemyDeathSFX;
        }
    }
}
