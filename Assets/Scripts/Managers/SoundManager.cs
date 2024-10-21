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

    // 이벤트 함수로 등록
    private void PlayTitleBGM() => PlayBGM(titleBGM);
    private void PlayPlayBGM() => PlayBGM(playBGM);
    private void PlayGameOverBGM() => PlayBGM(gameOverBGM);
    private void PlayPlayerAttackSFX() => PlaySFX("PlayerAttackSFX" ,playerAttackSFX);
    private void PlayEnemyDeathSFX() => PlaySFX("EnemyDeathSFX", enemyDeathSFX);


    public void ReSetBinding(Scene scene, LoadSceneMode mode)
    {
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
