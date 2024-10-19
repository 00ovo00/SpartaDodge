using UnityEngine;

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

        ReSetBinding();
    }


    private void Start()
    {
        GameManager.OnGameStart += PlayPlayBGM;
        healthSystem.OnGameOver += PlayGameOverBGM;
        playerInputController.OnPlayerAttack += PlayPlayerAttackSFX;
        DataManager.Instance.OnEnemyDeath += PlayEnemyDeathSFX;
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= PlayPlayBGM;
        healthSystem.OnGameOver -= PlayGameOverBGM;
        playerInputController.OnPlayerAttack -= PlayPlayerAttackSFX;
        DataManager.Instance.OnEnemyDeath += PlayEnemyDeathSFX;
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
    private void PlayPlayBGM() => PlayBGM(playBGM);
    private void PlayGameOverBGM() => PlayBGM(gameOverBGM);
    private void PlayPlayerAttackSFX() => PlaySFX(playerAttackSFX);
    private void PlayEnemyDeathSFX() => PlaySFX(enemyDeathSFX);

    public void ReSetBinding()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
        healthSystem = FindObjectOfType<HealthSystem>();
    }
}
