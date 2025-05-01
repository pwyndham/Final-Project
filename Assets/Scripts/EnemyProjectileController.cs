using Unity.Cinemachine;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectilePoint;
    EnemyLevelManager levelManager;
    public float scaledDamage;
    public float scaledFireSpeed;

    public void ShootProjectile(Vector3 targetPosition)
    {
        //direction is the player pos as vector3
        
            //direction = projectilePoint.position.normalized;
            Vector3 direction = (targetPosition - projectilePoint.position).normalized;

            Debug.DrawRay(projectilePoint.position, direction * 5f, Color.red, 2f);
            
            GameObject projectileGO = Instantiate(projectilePrefab, projectilePoint.position, Quaternion.identity);
            Projectile projectile = projectileGO.GetComponent<Projectile>();

            scaledDamage = levelManager != null ? levelManager.scaledDamage : 5f;
            scaledFireSpeed = levelManager != null ? levelManager.scaledFireSpeed : 5f;
            // Debug.Log(scaledDamage);
            // Debug.Log(scaledFireSpeed);
            projectile.Launch(direction, scaledDamage, scaledFireSpeed);
        
    }

    void Awake()
    {
        levelManager = GetComponent<EnemyLevelManager>();
    }

}
