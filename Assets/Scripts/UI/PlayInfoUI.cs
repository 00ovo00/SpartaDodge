using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private TextMeshProUGUI scoreText;

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
}
