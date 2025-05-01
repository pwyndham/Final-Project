using System;
using System.Collections;
using System.IO.Compression;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class EnemyLevelManager : MonoBehaviour
{
    
    public int enemyLevel = 0;
    public int meleeLevel = 1;
    public int rangedLevel = 2;
    public int magicLevel = 3;
    public string enemyLevelType;
    float baseHealth;
    float baseSpeed;
    int baseXP;
    public CharacterExperience characterExperience;
    public EnemyLevelUI enemyLevelUI;
    Creature creature;

    [Header("Enemy Combat Stats")]
    public float baseDamage = 20f;
    public float baseFireRate = 5f;
    public float baseProjectileSpeed = 20f;

    // Scaled values to be used by weapons/projectiles
    public float scaledDamage { get; private set; }
    public float scaledFireRate { get; private set; }
    public float scaledFireSpeed {get; private set; }
    //enemy melee ref
    //enemy projectile ref
    void Awake()
    {
        creature = GetComponent<Creature>();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            characterExperience = player.GetComponent<CharacterExperience>();
        }

    if (characterExperience == null)
    {
        Debug.LogWarning("CharacterExperience is null in EnemyLevelManager!");
    }
       
        enemyLevelType = creature.creatureType; // set string in inspector

        baseHealth = creature.health;
        baseSpeed = creature.speed;
        baseXP = creature.experiencePoints;

        enemyLevelUI = GetComponentInChildren<EnemyLevelUI>();
        
        //characterExperience.OnExperienceGained += RecalculateEnemyLevel; // action call from char exp
        //characterExperience.OnLevelUp += RecalculateEnemyLevel; // action call from char exp

        characterExperience.OnLevelUp += OnCharacterLevelUp;

        RecalculateEnemyLevel(); // Initial calculation
        RecalculateEnemyStats(enemyLevel);
        UpdateMeleeWeapon();
        UpdateRangedWeapon();
    }

   
    void OnDestroy()
    {
        if (characterExperience != null)
        {
            characterExperience.OnLevelUp -= OnCharacterLevelUp;
        }
    }

    void OnCharacterLevelUp()
{
    if (this == null || !gameObject) return;
    RecalculateEnemyLevel();
    RecalculateEnemyStats(enemyLevel);
    UpdateMeleeWeapon();
    UpdateRangedWeapon();
}

    void RecalculateEnemyLevel()
    {
        switch(enemyLevelType)
        {
            case "Melee Creature":
            if (characterExperience != null)
            {
                enemyLevel = characterExperience.characterLevel + meleeLevel;
            }
            else
            {
                Debug.LogWarning("CharacterExperience was null!");
            }

            break;

            case "Ranged Creature":
            if (characterExperience != null)
            {
                enemyLevel = characterExperience.characterLevel + rangedLevel;
            }
            else
            {
                Debug.LogWarning("CharacterExperience was null!");
            }

            break;

            case "Magic Creature":
            if (characterExperience != null)
            {
                 enemyLevel = characterExperience.characterLevel + magicLevel;   
            }
            else
            {
                Debug.LogWarning("CharacterExperience was null!");
            }
            break;
            
            default:
            break;
            
        }
        enemyLevelUI.UpdateEnemyDisplay(enemyLevel, enemyLevelType);
    }

   
    void RecalculateEnemyStats(int enemyLevel)
{
    float healthMultiplier = 1f;
    float speedBoost = 0f;
    int xpBoost = 0;
    float damageMultiplier = 1f;
    float fireRateBoost = 0f;
    float projectileSpeed = 0f;

    switch (enemyLevelType)
    {
        case "Melee Creature":
            healthMultiplier = 1.25f;
            speedBoost = 0.5f;
            xpBoost = 10;
            damageMultiplier = 1.3f;
            fireRateBoost = 0.2f;
            break;
        case "Ranged Creature":
            healthMultiplier = 1.15f;
            speedBoost = 1.0f;
            xpBoost = 15;
            damageMultiplier = 1.2f;
            fireRateBoost = 0.4f;
            projectileSpeed = .4f;
            break;
        case "Magic Creature":
            healthMultiplier = 1.10f;
            speedBoost = 0.75f;
            xpBoost = 20;
            damageMultiplier = 1.5f;
            fireRateBoost = 0.5f;
            projectileSpeed = .5f;
            break;
        default:
            Debug.LogWarning("Unknown enemy type in stats scaling.");
            break;
    }

    float finalHealth = baseHealth * (healthMultiplier + (enemyLevel * 0.05f));
    float finalSpeed = baseSpeed + speedBoost + (enemyLevel * 0.25f);
    int finalXP = baseXP + xpBoost + (enemyLevel * 2);

    creature.SetMaxHealth(finalHealth);
    creature.SetHealth(finalHealth);
    creature.SetSpeed(finalSpeed);
    creature.SetExperiencePoints(finalXP);
    // x.set Melee damage
    // x.set Projectile damage
     // Set scaled weapon stats
    scaledDamage = baseDamage * (damageMultiplier + enemyLevel * 0.05f);
    scaledFireRate = baseFireRate - fireRateBoost - (enemyLevel * 0.01f); // Lower is faster
    scaledFireRate = Mathf.Max(0.2f, scaledFireRate); // Clamp to prevent insane speed
    scaledFireSpeed = baseProjectileSpeed - (projectileSpeed + enemyLevel * .01f);
}

void UpdateMeleeWeapon()
{
    var meleeWeapon = GetComponentInChildren<EnemyMeleeWeapon>();
    if (meleeWeapon != null)
    {
        meleeWeapon.damage = scaledDamage;
        Debug.Log($"[EnemyLevelManager] Melee weapon damage updated to: {scaledDamage}");
        Debug.Log($"[EnemyLevelManager] Melee fire ratee updated to: {scaledFireRate}");
    }
}

void UpdateRangedWeapon()
{
    
    EnemyProjectileController rangedWeapon = GetComponent<EnemyProjectileController>();
    
    switch (enemyLevelType)
    {
        case "Ranged Creature":
            if (rangedWeapon != null)
            {
                rangedWeapon.scaledDamage = scaledDamage;
                rangedWeapon.scaledFireSpeed = scaledFireSpeed;
                Debug.Log($"[EnemyLevelManager] Ranged damage set to: {scaledDamage}");
                Debug.Log($"[EnemyLevelManager] Ranged fire rate set to: {scaledFireRate}");
                Debug.Log($"[EnemyLevelManager] Ranged projectile speed set to: {scaledFireSpeed}");
            }
            break;

        case "Magic Creature":
            if (rangedWeapon != null)
            {
                rangedWeapon.scaledDamage = scaledDamage;
                rangedWeapon.scaledFireSpeed = scaledFireSpeed;
                Debug.Log($"[EnemyLevelManager] Magic damage set to: {scaledDamage}");
                Debug.Log($"[EnemyLevelManager] Magic fire rate set to: {scaledFireRate}");
                Debug.Log($"[EnemyLevelManager] Magic projectile speed set to: {scaledFireSpeed}");
            }
            break;

        default:
            Debug.LogWarning("[EnemyLevelManager] No projectile updated — unknown enemy type.");
            break;
    }
}

   
}