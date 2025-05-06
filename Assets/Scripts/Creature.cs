using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    [Header("Movement")]
    protected CharacterController characterController;
    protected AnimationStateChanger animationStateChanger;
    public virtual float speed { get; protected set; } = 5f;

    [Header("Health")]
    public virtual float health { get; protected set; } = 50f;
    public virtual float maxHealth { get; protected set; } = 50f;
    public EnemyHealthBar healthBar;

    

    [Header("Functional")]
    public int creatureMoney = 50;
    public CharacterStats characterStats;
    public CharacterExperience characterExperience;
    public virtual string creatureType => "Base Creature";
    public virtual int experiencePoints { get; protected set; } = 10;
    public Material baseMaterial;
    public Material hitMaterial;

    public abstract void Move(Vector3 direction);

    public abstract void MoveToward(Vector3 target);
    public abstract void Stop();
    public virtual void Die(int experiencePoints)
    {
        if (characterExperience != null)
    {
        characterExperience.gainExperience(experiencePoints);
    }
        Debug.Log("Give Character experience " + experiencePoints);
    }

    protected virtual void Awake()
    {
        
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            characterStats = player.GetComponent<CharacterStats>();
            characterExperience = player.GetComponent<CharacterExperience>();
        }

        characterController = GetComponent<CharacterController>();
        animationStateChanger = GetComponent<AnimationStateChanger>();

        

        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.SetHealth(maxHealth);
    }

    
    public virtual void SetHealth(float value)
    {
        health = value;
        healthBar?.SetHealth(health);
    }

    public virtual void SetMaxHealth(float value)
    {
        maxHealth = value;
        healthBar?.SetMaxHealth(maxHealth);
    }

    public virtual void SetExperiencePoints(int value)
    {
        experiencePoints = value;
    }

    public virtual void SetSpeed(float value)
    {
        speed = value;
    }
    

    public virtual void TakeDamage(float damageAmount) {

        Debug.Log(gameObject.name + " took " + damageAmount + " damage!");
        health -= damageAmount;
        healthBar.SetHealth(health);
        

    if (health <= 0)
        {
            Debug.Log(gameObject.name + " died.");
            characterStats.GiveMoney(creatureMoney);
            Die(experiencePoints);
            Destroy(gameObject);
        }
    }

   public void EnableOutline()
    {
        GetComponent<Renderer>().material = hitMaterial;
    }

    public void DisableOutline()
    {
        GetComponent<Renderer>().material = baseMaterial;
    }
}