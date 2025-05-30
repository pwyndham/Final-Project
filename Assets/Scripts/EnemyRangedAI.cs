using System;
using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyRangedAI : BaseAI
{

    //public BaseAI baseAI;
    public Creature creature;

    Vector3 wanderPos = Vector3.zero;
    float pauseTime = 0f;
    NavMeshAgent navMeshAgent;
    [SerializeField] Transform playerTransform;
    public EnemyProjectileController enemyProjectileController;
    //AnimationStateChanger animationStateChanger;

    public Transform[] waypoints;  // Array of waypoint game objects
    private int currentWaypoint = 0;

    public float lookRadius = 20f;
    public float attackRange = 15f;

    public float attackCooldown = 1f; // time between shots
    private float lastAttackTime = 0f;
    private bool isPlayerAlive = true;
    

    void Start()
    {
        if (attackCooldown <= 0)
    {
        Debug.LogWarning("Attack cooldown must be greater than 0. Defaulting to 1.");
        attackCooldown = 1f;
    }

        if (PlayerController.Instance != null)
        playerTransform = PlayerController.Instance.PlayerTransform;
    else
        Debug.LogWarning("Player not found.");
    }
    protected void Awake()
    {
        base.Awake();
        creature = GetComponent<Creature>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyProjectileController = GetComponent<EnemyProjectileController>();

      
        ChangeState(RoamingState);
    }

  

    public void RoamingState(){

        NullCheckPlayer();
        stateImIn = "Roaming State";

        creature.animationStateChanger.ChangeAnimation("CharacterEnemyRangedWalk");

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (navMeshAgent.isOnNavMesh && navMeshAgent.isActiveAndEnabled)
        {
            if (navMeshAgent.remainingDistance < 0.1f)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                transform.LookAt(waypoints[currentWaypoint].position);
                navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
            }
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} is not on a NavMesh!");
        }

        if (distance <= lookRadius)
        {
            ChangeState(FoundPlayerState);
            return;
        }
    }

    public void FoundPlayerState(){

        NullCheckPlayer();

        creature.animationStateChanger.ChangeAnimation("CharacterEnemyRangedWalk");

        stateImIn = "Found Player State";

        navMeshAgent.SetDestination(playerTransform.position);
        
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (distance <= attackRange)
        {
            ChangeState(AttackState);
            return;
        }

        if (distance >= lookRadius)
        {
            ChangeState(RoamingState);
            return;
        }

        if (stateTick == 1) {
            Debug.Log("Found player");
        }
    }
        
   
    public void AttackState(){

        NullCheckPlayer();

        creature.animationStateChanger.ChangeAnimation("CharacterEnemyRangedAttack");

        stateImIn = "Attack State";

        float distance = Vector3.Distance(playerTransform.position, transform.position);
        
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            enemyProjectileController.ShootProjectile(playerTransform.position);
            lastAttackTime = Time.time;
        }
        //enemyProjectileController.ShootProjectile(playerTransform.position);
         //Debug.Log("Attacking Player");
         //attack logic
        if (distance >= attackRange)
        {
            ChangeState(FoundPlayerState);
            return;
        }
        if (stateTick == 1) {
            Debug.Log("Attacking");
        }
    }

    public void PlayerDeadState()
    {
    stateImIn = "Player Dead State";
    
    creature.animationStateChanger.ChangeAnimation("CharacterEnemyRangedIdle");

    if (waypoints != null && waypoints.Length > 0)
        {
            if (navMeshAgent.remainingDistance < 0.1f)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                transform.LookAt(waypoints[currentWaypoint].position);
                navMeshAgent.SetDestination(waypoints[currentWaypoint].position);
            }
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has no waypoints assigned!");
        }
//coroutine for player respawn...
    
    }
    public void NullCheckPlayer()
    {

        if (playerTransform == null || !playerTransform.gameObject.activeInHierarchy)
        {
            ChangeState(PlayerDeadState);
        }
    }

    
}
