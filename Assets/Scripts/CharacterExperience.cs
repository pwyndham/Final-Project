using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterExperience : MonoBehaviour
{
   //Character character;
    public event Action OnExperienceGained;
    public event Action OnLevelUp;

    public int characterLevel = 0;
    public int experienceToLevel = 20;
    public int characterExperiencePoints = 0;
    public float experienceGrowth = 1.1825f;

    public Slider expSlider;
    public TMP_Text currentLevelText;
    public TMP_Text currentExperienceText;

     void Awake()
    {
        UpdateUI();
        //character = GetComponent<Character>();
    }


    public void gainExperience(int experiencePoints)
    {
        characterExperiencePoints += experiencePoints;
        OnExperienceGained?.Invoke();

        if (characterExperiencePoints >= experienceToLevel)
        {
            levelUp();
        }
        UpdateUI();
        // Debug.Log("Character Experience: " + characterExperiencePoints);
    }

    public void levelUp()
    {
        
        //Debug.Log("Leveled Up!");
        characterLevel++;
        characterExperiencePoints -= experienceToLevel;
        experienceToLevel = Mathf.RoundToInt(experienceToLevel * experienceGrowth);

        OnLevelUp?.Invoke();
    }

    public void UpdateUI()
    {
        expSlider.maxValue = experienceToLevel;
        expSlider.value = characterExperiencePoints;
        currentLevelText.text = "Level: " + characterLevel;
        currentExperienceText.text = "XP: " + characterExperiencePoints + "/" + experienceToLevel;
    }

    

}