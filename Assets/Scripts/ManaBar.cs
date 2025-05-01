using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class ManaBar : MonoBehaviour
{
    [SerializeField] CharacterStats characterStats;
    [SerializeField] Slider slider;
    public TMP_Text currentManaText;

    private void OnEnable()
    {
        if (characterStats != null)
            characterStats.onManaChanged += UpdateMana;
    }

    private void OnDisable() {
        if (characterStats != null)
            characterStats.onManaChanged += UpdateMana;
    }

    public void UpdateMana(float currentMana, float maxMana)
    {
        slider.maxValue = maxMana;
        slider.value = currentMana;
        currentManaText.text = currentMana + "/" + maxMana;
    }
    
}
