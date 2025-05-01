using UnityEngine;



public class CharacterClassHandler : MonoBehaviour
{

    CharacterStats characterStats;
    public GameObject characterWeapon;
    //public Transform weaponTransform;
    public GameObject warriorDefaultWeapon;
    public GameObject rogueDefaultWeapon;
    public GameObject mageDefaultWeapon;

    private GameObject currentWeapon;

    void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        //could maybe do characterstats.selectedclass but its runtime so unsure.
    }

    public void ClassDefaultInstantiation(string selectedClass)
    {

        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        switch(selectedClass)
        {
            
            case "Warrior Class":
            characterWeapon = warriorDefaultWeapon;
            // DISABLE PROJECTILES
            break;

            case "Rogue Class":
            characterWeapon = rogueDefaultWeapon;
            // DISABLE PROJECTILES
            break;

            case "Mage Class":
            characterWeapon = mageDefaultWeapon;
            break;

        }
        if (characterWeapon != null && transform != null)
        {
            currentWeapon = Instantiate(characterWeapon, transform);
            currentWeapon.transform.localPosition = Vector3.zero;
            //characterWeapon.transform.localRotation = Quaternion.identity;
        }
    }






}