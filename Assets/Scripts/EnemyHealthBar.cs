using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetHealth(float health)
    {
        slider.value = health;
    }
}