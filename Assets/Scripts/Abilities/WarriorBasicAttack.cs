using System.Collections;
using UnityEngine;

public class WarriorBasicAttack : Ability
{
    CharacterStats characterStats;
    float basicAttackCost = 2f;
    bool onCooldown = false;

    void Awake() {
        abilityName = "WarriorBasicAttack";
        defaultCooldown = 3;
        abilityCooldown = 3;
        abilityDescription = "Warriors perform a basic slash.";
        characterStats = GetComponent<CharacterStats>();
        Debug.Log("This character has the light shot ability");
    }
    public override void Activate()
    {
        if (onCooldown)
        {
            Debug.Log("Ability on cooldown");
            return;
        }
        TryWarriorBasicAttack();
        StartCoroutine(Cooldown());
    }

    private void TryWarriorBasicAttack()
    {
        // swing blade
        // animation
        // dust particle
        // take energy
        // put ability on cooldown
        
    }

    IEnumerator Cooldown()
    {
        onCooldown = true;
        characterStats.UseEnergy(basicAttackCost);
        yield return new WaitForSeconds(abilityCooldown);
        onCooldown = false;
    }
}