using UnityEngine;
using System.Collections;
public class RogueBasicAttack : Ability
{
    CharacterStats characterStats;
    float basicAttackCost = 1f;
    bool onCooldown = false;
    void Awake() {
        abilityName = "RogueBasicAttack";
        defaultCooldown = 3;
        abilityCooldown = 3;
        abilityDescription = "Rogues slash their enemy";
        characterStats = GetComponent<CharacterStats>();
        Debug.Log("This character has the light shot ability");
    }
    public override void Activate()
    {
        if (onCooldown && abilityCooldown > 0)
        {
            Debug.Log("Ability on cooldown");
            return;
        }
        TryRogueBasicAttack();
        StartCoroutine(Cooldown());
    }

    private void TryRogueBasicAttack()
    {
        // animation swing and stab
        // dust particle
        Debug.Log("RogueBasicAttack was used");
    }
    IEnumerator Cooldown()
    {
        onCooldown = true;
        characterStats.UseEnergy(basicAttackCost);
        yield return new WaitForSeconds(abilityCooldown);
        onCooldown = false;
    }
}