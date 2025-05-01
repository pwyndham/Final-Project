using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class EnemyMageAI : BaseAI
{

    //public BaseAI baseAI;
    public Creature creature;

    NavMeshAgent navMeshAgent;
    [SerializeField] Transform playerTransform;
    public EnemyProjectileController enemyProjectileController;

    public Transform[] waypoints;  // Array of waypoint game objects
    private int currentWaypoint = 0;

    public float lookRadius = 15f;
    public float attackRange = 10f;

    public float attackCooldown = 2f; // time between shots
    private float lastAttackTime = 0f;
    private bool isPlayerAlive = true;
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

    public void AttackState(){

        NullCheckPlayer();



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
        if (!playerTransform.gameObject.activeInHierarchy)
        {
            ChangeState(PlayerDeadState);
        }
    }

}
