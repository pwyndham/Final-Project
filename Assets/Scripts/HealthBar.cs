using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

    [SerializeField] CharacterStats characterStats;
    [SerializeField] Slider slider;
    public TMP_Text currentHealthText;

    private void OnEnable()
    {
        if (characterStats != null)
            characterStats.onHealthChanged += UpdateHealth;
    }

    private void OnDisable() {
        if (characterStats != null)
            characterStats.onHealthChanged += UpdateHealth;
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        currentHealthText.text = currentHealth + "/" + maxHealth;
    }
    
}
