using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterStatsUI : MonoBehaviour
{

    public TMP_Text characterClassText, characterStrengthText, characterConstitutionText, characterDexterityText, characterIntelligenceText,
                        characterPerceptionText, characterEnduranceText, characterAgilityText, characterArmorResistanceText, characterMagicResistanceText,
                        characterCriticalStrikeText, levelUpPointsText;
    public Button   characterStrengthButton, characterConstitutionButton, characterDexterityButton, characterIntelligenceButton,
                        characterPerceptionButton, characterEnduranceButton, characterAgilityButton, characterArmorResistanceButton, characterMagicResistanceButton,
                        characterCriticalStrikeButton;                  
    //Make level up button visible when level up points > 1
    public CharacterStats characterStats;

    void Awake()
    {
        characterStats = characterStats.GetComponentInParent<CharacterStats>();
        UpdateText();
        AddCharacterStat();
    }
    void Update()
    {
        UpdateText();
        UnhideButton();
    }
    void UpdateText()
    {
        levelUpPointsText.text = "Level Up Points: " + characterStats.levelUpPoints.ToString();
        characterClassText.text = "Character Class: " + characterStats.characterClass.ToString();
        characterStrengthText.text = "Strength: " + characterStats.characterClass.strength.ToString();
        characterIntelligenceText.text = "Intelligence: " + characterStats.characterClass.intelligence.ToString();
        characterDexterityText.text = "Dexterity: " + characterStats.characterClass.dexterity.ToString();
        characterPerceptionText.text = "Perception: " + characterStats.characterClass.perception.ToString();
        characterEnduranceText.text = "Endurance: " + characterStats.characterClass.endurance.ToString();
        characterAgilityText.text = "Agility: " + characterStats.characterClass.agility.ToString();
        characterArmorResistanceText.text = "ArmorResistance: " + characterStats.characterClass.armorResistance.ToString();
        characterMagicResistanceText.text =  "MagicResistance: " + characterStats.characterClass.magicResistance.ToString();
        characterConstitutionText.text = "Constitution: " + characterStats.characterClass.constitution.ToString();
        characterCriticalStrikeText.text = "Critical Strike: " + characterStats.characterClass.criticalStrike.ToString();
    }

    void UnhideButton()
    {
        if (characterStats.levelUpPoints >= 1)
        {
            characterAgilityButton.gameObject.SetActive(true);
            characterConstitutionButton.gameObject.SetActive(true);
            characterStrengthButton.gameObject.SetActive(true);
            characterDexterityButton.gameObject.SetActive(true);
            characterIntelligenceButton.gameObject.SetActive(true);
            characterEnduranceButton.gameObject.SetActive(true);
            characterArmorResistanceButton.gameObject.SetActive(true);
            characterMagicResistanceButton.gameObject.SetActive(true);
            characterCriticalStrikeButton.gameObject.SetActive(true);
            characterPerceptionButton.gameObject.SetActive(true);
        }
        else 
        {
            characterAgilityButton.gameObject.SetActive(false);
            characterConstitutionButton.gameObject.SetActive(false);
            characterStrengthButton.gameObject.SetActive(false);
            characterDexterityButton.gameObject.SetActive(false);
            characterIntelligenceButton.gameObject.SetActive(false);
            characterEnduranceButton.gameObject.SetActive(false);
            characterArmorResistanceButton.gameObject.SetActive(false);
            characterMagicResistanceButton.gameObject.SetActive(false);
            characterCriticalStrikeButton.gameObject.SetActive(false);
            characterPerceptionButton.gameObject.SetActive(false);
        }
    }

    void AddCharacterStat()
    {
        characterAgilityButton.onClick.AddListener(() => {characterStats.characterClass.IncreaseAgility(1); UseLevelUpPoint(); UpdateText(); });
        characterConstitutionButton.onClick.AddListener(() => {characterStats.characterClass.IncreaseConstitution(1); UseLevelUpPoint(); UpdateText(); });
        characterStrengthButton.onClick.AddListener(() => {characterStats.characterClass.IncreaseStrength(1); UseLevelUpPoint(); UpdateText(); });
        characterDexterityButton.onClick.AddListener(() => {characterStats.characterClass.IncreaseDexterity(1); UseLevelUpPoint(); UpdateText(); });
        characterIntelligenceButton.onClick.AddListener(() => {characterStats.characterClass.IncreaseIntelligence(1); UseLevelUpPoint(); UpdateText(); });
        characterEnduranceButton.onClick.AddListener(() => {characterStats.characterClass.IncreaseEndurance(1); UseLevelUpPoint(); UpdateText(); });
        characterArmorResistanceButton.onClick.AddListener(() => {characterStats.characterClass.IncreaseArmorResistance(1); UseLevelUpPoint(); UpdateText(); });
        characterMagicResistanceButton.onClick.AddListener(() => {characterStats.characterClass.IncreaseMagicResistance(1); UseLevelUpPoint(); UpdateText(); });
        characterCriticalStrikeButton.onClick.AddListener(() => {characterStats.characterClass.IncreaseCriticalStrike(1); UseLevelUpPoint(); UpdateText(); });
        characterPerceptionButton.onClick.AddListener(() => {characterStats.characterClass.IncreasePerception(1); UseLevelUpPoint(); UpdateText(); });
    }

    void UseLevelUpPoint()
    {
        characterStats.levelUpPoints--;
        characterStats.ApplyStatCalculation();
    }
}
