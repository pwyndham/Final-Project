using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Dash : Ability
{
    CharacterStats characterStats;
    CharacterController characterController;
    public float dashCost = 5f;
    public bool isDashing { get; private set; } = false;
    bool onCooldown = false;
    void Awake() {
        abilityName = "Dash";
        abilityCooldown = 3;
        abilityDescription = "Rogues can dash a fixed distance away or to enemies.";
        characterStats = GetComponent<CharacterStats>();
        characterController = GetComponent<CharacterController>();
        
        Debug.Log("This character has the dash ability");
    }
    public override void Activate()
    {
        if (onCooldown)
        {
            Debug.Log("Ability on cooldown");
            return;
        }
        else if (characterStats.energyPoints > dashCost){
            TryDash();
        }
    }

    private void TryDash()
    {
        float dashDistance = 5f;
        float dashDuration = .3f;

        StartCoroutine(DashNow(dashDistance, dashDuration));
        // proportionalize the dash distance with agility?
        // animation
        // dust particle
        Debug.Log("Dash was used");
    }
    IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(abilityCooldown);
        onCooldown = false;
    }

    IEnumerator DashNow(float dashDistance, float dashDuration)
    {
        isDashing = true;
        characterStats.UseEnergy(dashCost);
        StartCoroutine(Cooldown());
        float time = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * dashDistance;

        while (time < dashDuration)
        {
            Vector3 nextPos = Vector3.Lerp(startPos, endPos, time/dashDuration);
            characterController.Move(nextPos - transform.position);
            time += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        //characterController.Move(endPos - transform.position);
    }
}