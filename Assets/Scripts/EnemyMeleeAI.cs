using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMeleeAI : BaseAI
{

    //public BaseAI baseAI;
    public Creature creature;

    Vector3 wanderPos = Vector3.zero;
    float pauseTime = 0f;
    NavMeshAgent navMeshAgent;
    public Transform playerTransform;

    public Transform[] waypoints;  // Array of waypoint game objects
    private int currentWaypoint = 0;

    public float lookRadius = 25f;
    public float attackRange = 2f;

    public float attackCooldown = .3f; // time between shots
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

        ChangeState(RoamingState);
    }

    

    public void RoamingState(){

        NullCheckPlayer();
        stateImIn = "Roaming State";

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (navMeshAgent.remainingDistance < 0.1f) { // If agent is close enough to destination
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length; // Go to next waypoint
            //slerp this
            transform.LookAt(waypoints[currentWaypoint].position);
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position); // Set new destination
        }

        if (distance <= lookRadius)
        {
            ChangeState(FoundPlayerState);
            return;
        }
    }
    

    public void AttackState(){

        NullCheckPlayer();

        stateImIn = "Attack State";

        float distance = Vector3.Distance(playerTransform.position, transform.position);
        
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            //swing sword
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

    public void FoundPlayerState(){

        NullCheckPlayer();

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
   public void PlayerDeadState()
    {
    stateImIn = "Player Dead State";
    if (navMeshAgent.remainingDistance < 0.1f) { // If agent is close enough to destination
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length; // Go to next waypoint
            //slerp this
            transform.LookAt(waypoints[currentWaypoint].position);
            navMeshAgent.SetDestination(waypoints[currentWaypoint].position); // Set new destination
        }
//coroutine for player respawn...
    
    }
    public void NullCheckPlayer()
    {
        // if(playerTransform == null && !playerTransform.gameObject.activeInHierarchy)
        // {
        //     return;
        // }
        if (playerTransform == null|| !playerTransform.gameObject.activeInHierarchy)
        {
            ChangeState(PlayerDeadState);
        }
    }

}
