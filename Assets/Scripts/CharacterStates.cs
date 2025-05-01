using UnityEngine;

public class CharacterStates : MonoBehaviour
{

    CharacterStats characterStats;

    void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    void playerTakeDamage() 
    {
        //characterStats-=damageTaken;
    }

}   