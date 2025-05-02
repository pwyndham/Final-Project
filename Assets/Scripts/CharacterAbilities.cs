using UnityEngine;


public class CharacterAbilities : MonoBehaviour
{


    public void AssignAbilities(string classType)
    {
        switch(classType)
        {
            case "Warrior Class":
            Debug.Log("Warrior abilities are assigned");
            break;
            case "Mage Class":
            Debug.Log("Mage abilities are assigned");
            break;
            case "Rogue Class":
            Debug.Log("Rogue abilities are assigned");
            break;
            default:
            Debug.Log("No class found");
            break;
        }
    }
    
    //reference character class

    
}
