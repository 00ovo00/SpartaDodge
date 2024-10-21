using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            HealthSystem playerHealthSystem = GameManager.Instance.Player.GetComponent<HealthSystem>();
            if (playerHealthSystem != null)
                playerHealthSystem.OnGameOver += ShowGameOverScreen;
        }

        gameOverPanel.SetActive(false); // 시작 시 비활성화
        restartButton.onClick.AddListener(RestartGame); // 재시작 버튼 클릭 시 게임 재시작 처리
    }

    private void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);

        float currentScore = DataManager.Instance.GetScore();
        float bestScore = DataManager.Instance.GetBestScore();

        if (currentScore > bestScore)
        {
            // 갱신한 최고점 저장하도록 메소드 호출
            DataManager.Instance.SetBestScore(currentScore);
        }

        scoreText.text = currentScore.ToString();
        bestScoreText.text = DataManager.Instance.GetBestScore().ToString();
    }

    private void RestartGame()
    {
        GameManager.Instance.ReLoadGame();
    }
}