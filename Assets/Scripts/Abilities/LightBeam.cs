using UnityEngine;
using System.Collections;
public class LightBeam : Ability
{
    CharacterStats characterStats;
    public GameObject projectilePrefab;
    public Transform projectilePoint;
    bool onCooldown = false;
    void Awake() {
        abilityName = "LightBeam";
        abilityCooldown = 3;
        abilityDescription = "Large radius beam of light";
        characterStats = GetComponent<CharacterStats>();
        Debug.Log("This character has the LightBeam ability");

        ProjectileReferences refs = GetComponent<ProjectileReferences>();
        if (refs != null) {
            projectilePrefab = refs.lightBeamPrefab;
            projectilePoint = refs.projectilePoint;
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
        TryLightBeam();
        StartCoroutine(Cooldown());
    }

    private void TryLightBeam()
    {
        ShootProjectile();
        // damage adjustments
        // animation
        // light particle
        Debug.Log("LightBeam was used");
    }
    IEnumerator Cooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(abilityCooldown);
        onCooldown = false;
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null)
        {
            if (characterStats.manaPoints >= characterStats.projectileCost)
            {
                characterStats.UseMana(characterStats.ProjectileCost); // flat energy usage
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 999f))
                {
                    Vector3 direction = (hit.point - projectilePoint.position).normalized;
                    GameObject projectileInstance = Instantiate(projectilePrefab, projectilePoint.position, Quaternion.LookRotation(direction));
                    Projectile projectileComponent = projectileInstance.GetComponent<Projectile>();


                    if (projectileComponent != null)
                    {
                        projectileComponent.Launch(direction, characterStats.magicDamage, characterStats.projectileSpeed);
                        projectileComponent.SetProjectileStats(characterStats); // pass stats to projectile
                    }
                }
            }
            else
            {
                Debug.Log("NOT ENOUGH MANA or not a mage or projectile prefab is null");
            }
            
        }
    }
}