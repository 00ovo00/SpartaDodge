using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    private HealthSystem playerHealthSystem;

    [SerializeField] private Slider hpBar;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        playerHealthSystem = FindObjectOfType<HealthSystem>();
        if (playerHealthSystem != null)
            playerHealthSystem.OnHealthChanged += UpdateHealth;
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (hpBar != null)
            hpBar.value = currentHealth / maxHealth;
    }

    public void UpdateScore(float score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }
    private void OnDestroy()
    {
        if (playerHealthSystem != null)
            playerHealthSystem.OnHealthChanged -= UpdateHealth;
    }
}
