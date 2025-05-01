using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnergyBar : MonoBehaviour
{

    [SerializeField] CharacterStats characterStats;
    [SerializeField] Slider slider;
    public TMP_Text currentEnergyText;

    private void OnEnable()
    {
        if (characterStats != null)
            characterStats.onEnergyChanged += UpdateEnergy;
    }

    private void OnDisable() {
        if (characterStats != null)
            characterStats.onEnergyChanged += UpdateEnergy;
    }

    public void UpdateEnergy(float currentEnergy, float maxEnergy)
    {
        slider.maxValue = maxEnergy;
        slider.value = currentEnergy;
        currentEnergyText.text = currentEnergy + "/" + maxEnergy;
    }
    
}
