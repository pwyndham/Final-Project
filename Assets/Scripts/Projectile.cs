using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public CharacterStats characterStats;
    public enum ProjectileType 
    {
        EnemyRangedProjectile,
        EnemyMagicProjectile,
        PlayerMagicProjectile
    }

    public ProjectileType projectileType;

    public float damage = 5f;
    public float speed = 20f;
    public float lifetime = 5f;

    public void Launch(Vector3 direction, float newDamage, float newSpeed)
    {
        speed = newSpeed;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction * speed;
        damage = newDamage;
         //Debug.Log($"Projectile launched with velocity: {rb.linearVelocity}");
        Destroy(gameObject, lifetime);
    }

    void Start()
    {
        SetDamageBasedOnType();
    }

    void SetDamageBasedOnType()
    {
        switch (projectileType)
        {
            case ProjectileType.EnemyMagicProjectile:
                //damage = 5f;
                break;
            case ProjectileType.EnemyRangedProjectile:
                //damage = 7f;
                break;
            case ProjectileType.PlayerMagicProjectile:
                damage = characterStats != null ? characterStats.magicDamage : 10f;
                break;
            default:
                //damage = 5f;
                break;
        }
    }

    public void SetProjectileStats(CharacterStats stats)
    {
        characterStats = stats;
    }
    
    void OnCollisionEnter(Collision collision)
    {   

        GameObject hit = collision.gameObject;

    if (CompareTag("EnemyRangedProjectile") && hit.CompareTag("Player"))
    {
        // Physics.IgnoreLayerCollision(0, 8);
        
        var stats = hit.GetComponent<CharacterStats>();
        stats?.TakeDamage(damage, DamageType.Physical);
        Debug.Log("Player taking damage");
        // hit.GetComponent<PlayerHealth>()?.TakeDamage(damage);
    }
    if (CompareTag("EnemyMagicProjectile") && hit.CompareTag("Player"))
    {
        // Physics.IgnoreLayerCollision(0, 8);

        var stats = hit.GetComponent<CharacterStats>();
        stats?.TakeDamage(damage, DamageType.Magical);
        Debug.Log("Player taking damage");
        // hit.GetComponent<PlayerHealth>()?.TakeDamage(damage);
    }
    else if (CompareTag("PlayerProjectile") && hit.CompareTag("Enemy"))
    {
        var stats = hit.GetComponent<Creature>();

        float finalDamage = characterStats.ApplyCriticalStrikeChance(damage);
        stats?.TakeDamage(finalDamage);
        Debug.Log("Enemy taking damage");
        // hit.GetComponent<Creature>()?.TakingDamage(damage);
    }
       
    }

    
}
