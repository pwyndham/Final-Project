using UnityEngine;
using System.Collections;
public class LightShot : Ability
{
    CharacterStats characterStats;
    public GameObject projectilePrefab;
    public Transform projectilePoint;
    bool onCooldown = false;
    void Awake() {
        abilityName = "LightShot";
        abilityCooldown = 3;
        abilityDescription = "Single target burst of energy.";
        characterStats = GetComponent<CharacterStats>();
        Debug.Log("This character has the light shot ability");

        ProjectileReferences refs = GetComponent<ProjectileReferences>();
        if (refs != null) {
            projectilePrefab = refs.lightShotPrefab;
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
        TryLightShot();
        StartCoroutine(Cooldown());
    }

    private void TryLightShot()
    {
        ShootProjectile();
        // damage adjustments
        // animation
        // light particle
        Debug.Log("Lightshot was used");
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
                        projectileComponent.SetProjectileStats(characterStats); // ✅ pass stats to projectile
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