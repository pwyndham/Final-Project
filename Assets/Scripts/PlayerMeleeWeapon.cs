using UnityEngine;

public class PlayerMeleeWeapon : MonoBehaviour
{
    public enum WeaponType
    {
        ShortMelee,
        LongMelee
    }

    public WeaponType weaponType;
    CharacterStats characterStats;

    public float damage; // base value, changed in editor

    void Awake()
    {
        characterStats = GetComponentInParent<CharacterStats>();
    }
    void Start()
    {
        SetDamageBasedOnType();
    }

    void SetDamageBasedOnType()
    {
        switch (weaponType)
        {
            case WeaponType.ShortMelee:
                damage = characterStats.meleeShortDamage;
                break;
            case WeaponType.LongMelee:
                damage = characterStats.meleeDamage;
                break;
            default:
                damage = 5f;
                break;
        }
    }
    // public CharacterStats characterStats;
    // CharacterStats characterStats;
    // damage = characterStats.meleeShortDamage;
    void OnCollisionEnter(Collision collision)
    {   
        GameObject hit = collision.gameObject;

    if (CompareTag("PlayerMeleeWeapon") && hit.CompareTag("Enemy"))
        {
        var stats = hit.GetComponent<Creature>();

        float baseDamage = weaponType == WeaponType.ShortMelee
            ? characterStats.meleeShortDamage
            : characterStats.meleeDamage;

        float finalDamage = characterStats.ApplyCriticalStrikeChance(baseDamage);

        stats?.TakeDamage(finalDamage);
        Debug.Log("Player taking melee damage");
        // hit.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}