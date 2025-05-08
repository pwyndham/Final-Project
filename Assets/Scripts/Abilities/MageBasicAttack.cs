using UnityEngine;
using System.Collections;
using UnityEditor.Rendering.Universal;
public class MageBasicAttack : Ability
{
    public string animationName;
    CharacterStats characterStats;
    public GameObject projectilePrefab;
     CharacterInput charInput; 
    public Transform projectilePoint;
    bool onCooldown = false;
    void Start()
    {
        animationName = "CharacterMagicAttack";
    }
    void Awake() {
        charInput = GetComponent<CharacterInput>();
        abilityName = "MageBasicAttack";
        abilityCooldown = 2;
        abilityDescription = "Mages shoot a burst of pure magic";
        characterStats = GetComponent<CharacterStats>();
        Debug.Log("This character has the light shot ability");

        ProjectileReferences refs = GetComponent<ProjectileReferences>();
        if (refs != null) {
            projectilePrefab = refs.basicProjectilePrefab;
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
        TryMageBasicAttack();
        StartCoroutine(Cooldown());
    }

    private void TryMageBasicAttack()
    {
        
        ShootProjectile();
        // StartCoroutine(PlayAttackAnimation());
        // damage adjustments
        // animation
        // light particle
        Debug.Log("MageBasicAttack was used");
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