using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{

    // public CharacterStats characterStats;
    public float damage = 5f;
    void OnCollisionEnter(Collision collision)
    {   

        GameObject hit = collision.gameObject;

    if (CompareTag("EnemyMeleeWeapon") && hit.CompareTag("Player"))
        {
        var stats = hit.GetComponent<CharacterStats>();
        stats?.TakeDamage(damage, DamageType.Physical);
        Debug.Log("Player taking melee damage");
        // hit.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }

    
}
