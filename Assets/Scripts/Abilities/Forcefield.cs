using UnityEngine;
using System.Collections;
public class Forcefield : Ability
{
    CharacterStats characterStats;
    public GameObject forcefieldPrefab;
    public Transform forcefieldTransform;
    float forcefieldCost = 5f;
    bool onCooldown = false;
    void Awake() {
        abilityName = "Forcefield";
        abilityCooldown = 10;
        abilityDescription = "Warriors put up a spherical shield that blocks all projectiles for 10 seconds";
        characterStats = GetComponent<CharacterStats>();
        Debug.Log("This character has the forcefield ability");

        ForcefieldReference refs = GetComponent<ForcefieldReference>();
        if (refs != null) {
            forcefieldPrefab = refs.forcefieldPrefab;
            forcefieldTransform = refs.forcefieldTransform;
        }
        else
        {
            Debug.Log("projectileRefs no found");
        }
    }
    public override void Activate()
    {
        if (onCooldown)
        {
            Debug.Log("Ability on cooldown");
            return;
        }
        TryForcefield();
        StartCoroutine(Cooldown());
    }

    private void TryForcefield()
    {
        
        StartCoroutine(UseForcefield());
        // animation
        // forcefield shader
        // forcefield particle
        Debug.Log("Forcefield was used");
    }
    IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(abilityCooldown);
        onCooldown = false;
    }

    IEnumerator UseForcefield()
    {
        float time = 0f;
        
            characterStats.UseEnergy(forcefieldCost);
            GameObject forcefield = Instantiate(forcefieldPrefab, forcefieldTransform.position, Quaternion.identity);
            
        while(time < abilityCooldown)
        {
            forcefield.transform.position = transform.position;
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(forcefield);
    }

}