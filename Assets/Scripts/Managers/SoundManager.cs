using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private HealthSystem healthSystem;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private AudioClip playBGM;
    [SerializeField] private AudioClip gameOverBGM;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip playerAttackSFX;
    [SerializeField] private AudioClip enemyDeathSFX;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // 이벤트 함수로 등록
    private void PlayTitleBGM() => PlayBGM(titleBGM);
    private void PlayPlayBGM() => PlayBGM(playBGM);
    private void PlayGameOverBGM() => PlayBGM(gameOverBGM);
    private void PlayPlayerAttackSFX() => PlaySFX(playerAttackSFX);
    private void PlayEnemyDeathSFX() => PlaySFX(enemyDeathSFX);

    public void ReSetBinding(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "TitleSceneHSH")
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
