using UnityEngine;
using System.Collections;
public class RagingSlashes : Ability
{
    CharacterStats characterStats;
    WarriorBasicAttack warriorBasicAttack;
    float ragingSlashesCost = 5f;
    int ragingSlashesDuration = 5;
    bool isInRagingSlashes = false;
    bool onCooldown = false;
    void Awake() {
        abilityName = "RagingSlashes";
        abilityCooldown = 3;
        abilityDescription = "Warriors increase damage and attack speed for a fixed amount of time";
        warriorBasicAttack = GetComponent<WarriorBasicAttack>();
        characterStats = GetComponent<CharacterStats>();
        Debug.Log("This character has the RagingSlashes ability");
    }
   public override void Activate()
    {
        if (onCooldown)
        {
            Debug.Log("Ability on cooldown");
            return;
        }
        TryRagingSlashes();
        
    }

    private void TryRagingSlashes()
    {
        StartCoroutine(RagingSlashesActive());
        characterStats.UseEnergy(ragingSlashesCost);
        // swing blades
        // decrease basic attack cooldown
        // animation
        // dust particle
        // take energy
        // put ability on cooldown
        Debug.Log("RagingSlashes was used");
    }
    IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(abilityCooldown);
        onCooldown = false;
    }
    
    IEnumerator RagingSlashesActive()
    {
        StartCoroutine(Cooldown());
        isInRagingSlashes = true;
        warriorBasicAttack.abilityCooldown = 0;
        characterStats.meleeDamage *= 2;
        yield return new WaitForSeconds(ragingSlashesDuration);
        warriorBasicAttack.abilityCooldown = warriorBasicAttack.defaultCooldown;
        characterStats.meleeDamage /= 2;
        isInRagingSlashes = false;
    }
}