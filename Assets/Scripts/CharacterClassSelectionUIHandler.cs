using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CharacterClassSeelctionUIHandler : MonoBehaviour
{
    public Button warriorButton;
    public Button rogueButton;
    public Button mageButton;

    // public TextMeshPro classNameText;
    // public TextMeshPro statsText;

    public CharacterStats characterStats;
    void Start()
    {
        
        Debug.Log("Start called");
        warriorButton.onClick.AddListener(() => Debug.Log("Warrior button clicked"));
        warriorButton.onClick.AddListener(() => SelectClass("Warrior Class"));
        rogueButton.onClick.AddListener(() => SelectClass("Rogue Class"));
        mageButton.onClick.AddListener(() => SelectClass("Mage Class"));
    }

    public void SelectClass(string selectedClass)
    {
        Debug.Log(selectedClass);
        characterStats.SetCharacterClass(selectedClass);
        UpdateClassUI(selectedClass);
    }

    void UpdateClassUI(string selectedClass)
    {
        Debug.Log("Updated class in ui" + selectedClass);
        // classNameText.text = $"Class: {selectedClass}";
        // statsText.text = $"HP: {characterStats.maxHealthPoints}\n" +
        //                  $"MP: {characterStats.maxManaPoints}\n" +
        //                  $"EP: {characterStats.maxEnergyPoints}\n" +
        //                  $"Speed: {characterStats.CharacterSpeed}\n" +
        //                  $"Jump: {characterStats.JumpSpeed}";
    }
    
}
