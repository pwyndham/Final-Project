using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class AbilitiesUI : MonoBehaviour
{

    public CharacterAbilities characterAbilities;
    public TMP_Text AbilityOneText, AbilityTwoText, AbilityThreeText, AbilityOneDescriptionText, AbilityTwoDescriptionText, AbilityThreeDescriptionText,
                    AbilityOneCooldownText, AbilityTwoCooldownText, AbilityThreeCooldownText;
    // public Image AbilityOneIcon, AbilityTwoIcon, AbilityThreeIcon;
           
    public CharacterStats characterStats;

    void Awake()
    {
        characterStats = characterStats.GetComponentInParent<CharacterStats>();
    }

    public void UpdateAbilitiesUI()
    {
        if (characterAbilities == null || characterAbilities.abilities.Count < 3) return;

        AbilityOneText.text = characterAbilities.abilities[0].ToString();
        AbilityTwoText.text = characterAbilities.abilities[1].ToString();
        AbilityThreeText.text = characterAbilities.abilities[2].ToString();

        AbilityOneDescriptionText.text = characterAbilities.abilities[0].abilityDescription.ToString();
        AbilityTwoDescriptionText.text = characterAbilities.abilities[1].abilityDescription.ToString();
        AbilityThreeDescriptionText.text = characterAbilities.abilities[2].abilityDescription.ToString();

        AbilityOneCooldownText.text = characterAbilities.abilities[0].abilityCooldown.ToString();
        AbilityTwoCooldownText.text = characterAbilities.abilities[1].abilityCooldown.ToString();
        AbilityThreeCooldownText.text = characterAbilities.abilities[2].abilityCooldown.ToString();
    }
}