using UnityEngine;
using UnityEngine.AI;

public class ZombieStateMachine : MonoBehaviour
{
    public float chaseRadius;
    public float attackRange;
    public float timeInIdle;
    public Transform[] patrolWaypoints;
    public float patrolSpeed;
    public float chaseSpeed;
    public int damage;
    public float maxHealth;

    public EnemyData enemyData;

    public Transform player;

    private ZombieState currentState;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int currentWaypointIndex = 0;
    private float idleTimer = 0.0f;
    public float currentHealth;

    public void Initialize(EnemyData enemyData)
    {
        maxHealth = enemyData.maxHealth;
        chaseRadius = enemyData.chaseRadius;
        attackRange = enemyData.attackRange;
        timeInIdle = enemyData.timeInIdle;
        patrolSpeed = enemyData.patrolSpeed;
        chaseSpeed = enemyData.chaseSpeed;
        damage = enemyData.damage;
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentWaypointIndex = 0;
        idleTimer = 0.0f;
        currentHealth = maxHealth;
        Initialize(enemyData);

        TransitionToState(ZombieState.Idle);
    }

    void Update()
    {
        UpdateState();
    }

    void TransitionToState(ZombieState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        switch (currentState)
        {
            case ZombieState.Idle:
                animator.SetTrigger("Idle");
                idleTimer = 0.0f;
                break;
            case ZombieState.Patrol:
                animator.SetTrigger("Walk");
                navMeshAgent.speed = patrolSpeed;
                SetNextWaypoint();
                break;
            case ZombieState.Chase:
                animator.SetTrigger("Run");
                navMeshAgent.speed = chaseSpeed;
                break;
            case ZombieState.Attack:
                
                // Implement attack logic here
                AttackPlayer();
                break;
            case ZombieState.Death:
                animator.SetTrigger("Death");
                navMeshAgent.isStopped = true;
                Destroy(gameObject, 1.0f); // Destroy the zombie game object after 3 seconds.
                break;
        }
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                idleTimer += Time.deltaTime;
                if (idleTimer >= timeInIdle)
                    TransitionToState(ZombieState.Patrol);
                break;
            case ZombieState.Patrol:
                if (navMeshAgent.remainingDistance < 0.1f)
                    SetNextWaypoint();
                CheckForPlayer();
                break;
            case ZombieState.Chase:
                navMeshAgent.SetDestination(player.position);
                CheckForPlayer();
                break;
            case ZombieState.Attack:
                // Attack logic is implemented in the TransitionToState method
                AttackPlayer();
                break;
        }
    }

    void SetNextWaypoint()
    {
        if (patrolWaypoints.Length == 0)
            return;

        navMeshAgent.SetDestination(patrolWaypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
    }

    void CheckForPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= chaseRadius)
            TransitionToState(ZombieState.Chase);
        else if (Vector3.Distance(transform.position, player.position) <= attackRange)
            TransitionToState(ZombieState.Attack);
    }

    void AttackPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            animator.SetTrigger("Attack");
            // Player is in attack range, apply damage or trigger attack animation.
            PlayerDeath playerHealth = GetComponent<PlayerDeath>();
            if (playerHealth != null)
            {
                playerHealth.GetHurt(damage);
            }
            // Implement the attack logic here.
        }
        else
        {
            TransitionToState(ZombieState.Chase);
        }
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            // Transition to the Death state and destroy the zombie game object.
            TransitionToState(ZombieState.Death);
        }
    }

}

public enum ZombieState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Death
}
