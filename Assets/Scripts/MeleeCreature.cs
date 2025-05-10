using UnityEngine;

public class MeleeCreature : Creature
{
    public float _health = 30f;
    public float _maxHealth = 30;
    public int _experiencePoints = 30;
    public float _speed = 4f;

    public override float health
    {
        get => _health;
        protected set => _health = value;
    }

    public override float maxHealth
    {
        get => _maxHealth;
        protected set => _maxHealth = value;
    }
    public override float speed 
    {
        get => _speed;
        protected set => _speed = value;
    }
    public override int experiencePoints 
    {
        get => _experiencePoints;
        protected set => _experiencePoints = value;
    }
    


    public override string creatureType => "Melee Creature";
    public override void Move(Vector3 direction)
    {   
        if(direction == Vector3.zero) {
            animationStateChanger.ChangeAnimation("Enemy Idle");
            return;
        }
        direction.Normalize();
        animationStateChanger.ChangeAnimation("Enemy Melee Attack");
        characterController.Move(direction * speed * Time.deltaTime);
    }

    
    public override void MoveToward(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        Move(direction);
    }
    protected override void Awake()
    {
        base.Awake();

        if (characterController == null)
            Debug.LogWarning("CharacterController missing on " + gameObject.name);
        if (animationStateChanger == null)
            Debug.LogWarning("AnimationStateChanger missing on " + gameObject.name);
        // if (healthBar == null)
        //     Debug.LogWarning("HealthBar missing in children of " + gameObject.name);
    }

    public override void Stop()
    {
        animationStateChanger.ChangeAnimation("Enemy Idle");
    }


    public override void TakeDamage(float damageAmount) {
         base.TakeDamage(damageAmount);
        
    }

   
}