using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class Frenzy : Ability
{
    CharacterStats characterStats;
    RogueBasicAttack rogueBasicAttack;
    float frenzyCost = 5f;
    int frenzyDuration = 4;
    int yieldDuration = 1;
    bool isInFrenzy = false;
    bool onCooldown = false;
    void Awake() {
        abilityName = "Frenzy";
        abilityCooldown = 3;
        abilityDescription = "Rogues can boost their attack speed and movement speed by 100% for 5 seconds";
        characterStats = GetComponent<CharacterStats>();
        rogueBasicAttack = GetComponent<RogueBasicAttack>();
        Debug.Log("This character has the Frenzy ability");
    }
    public override void Activate()
    {
        if (onCooldown)
        {
            Debug.Log("Ability on cooldown");
            return;
        }
        TryFrenzy();
        
    }

    private void TryFrenzy()
    {
        StartCoroutine(Frenzying());
        // swing knife
        // stab knife
        // take energy
        Debug.Log("Frenzy was used");
    }
    IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(abilityCooldown);
        onCooldown = false;
    }

    IEnumerator Frenzying()
    {
        // unfortunate bug you have to press shift before e in order for jump and base speed to udpate
        isInFrenzy = true;
        StartCoroutine(Cooldown());
        characterStats.UseEnergy(frenzyCost);
        rogueBasicAttack.abilityCooldown = 0;
        characterStats.CharacterSpeed *= 2;
        characterStats.JumpSpeed *= 2;
        characterStats.SprintSpeed *= 2;
        yield return new WaitForSeconds(frenzyDuration);
        rogueBasicAttack.abilityCooldown = rogueBasicAttack.defaultCooldown;
        characterStats.CharacterSpeed /= 2;
        characterStats.JumpSpeed /= 2;
        characterStats.SprintSpeed /= 2;
        yield return new WaitForSeconds(yieldDuration);
        isInFrenzy = false;
    }

}