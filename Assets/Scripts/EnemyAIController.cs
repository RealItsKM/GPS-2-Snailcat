using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;
    public float chaseDistance = 10f;
    public float visionAngle = 60f;
    public float lostSightDuration = 2f;
    public float searchDuration = 5f;
    public float searchRadius = 3f;
    public float catchDistance = 2f;
    public float stunDuration = 3f;
    public LayerMask obstructionLayers;
    public LayerMask obstacleLayer;  // Layer for obstacles
    public float obstacleDetectionRadius = 5f;  // Detection radius for obstacles

    private NavMeshAgent navMeshAgent;
    private int patrolIndex = 0;
    private float timeSinceLastSeenPlayer;
    public bool isChasing = false;
    private bool isSearching = false;
    public static bool hasCaughtPlayer = false;
    private Vector3 lastKnownPosition;
    private float searchStartTime;
    private bool fixingObstacle = false;
    private Obstacle currentObstacle = null; // Reference to the current obstacle to fix
    private Vector3 eyePositionOffset = new Vector3(0, 1.5f, 0);
    private Vector3 eyePosition => transform.position + eyePositionOffset;

    public PlayerMovement playerMovement;
    public MoneyManager moneyManager;
    public EnemyAISuspicion aiSuspicion;

    public List<HidingPlace> hidingPlaces;
    private HidingPlace targetHidingPlace = null;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        timeSinceLastSeenPlayer = 0f;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        timeSinceLastSeenPlayer += Time.deltaTime;

        if (hasCaughtPlayer)
        {
            Patrol();  // Keep patrolling after catching the player
            return;
        }

        // Check for obstacles that need fixing
        if (!fixingObstacle)
        {
            DetectObstacle();
        }

        if (fixingObstacle)
        {
            // Move towards the obstacle
            if (Vector3.Distance(transform.position, currentObstacle.transform.position) <= 1f)
            {
                currentObstacle.ResetObstacle();
                fixingObstacle = false;
                currentObstacle = null;

                // Check if player is still in sight
                if (PlayerInSight())
                {
                    isChasing = true;
                }
                else
                {
                    // Resume patrolling or searching
                    GoToNextPatrolPoint();
                }
            }
            else
            {
                // Move towards the obstacle
                navMeshAgent.destination = currentObstacle.transform.position;
            }

            return;
        }

        if (isChasing)
        {
            if (PlayerInSight())
            {
                if (PlayerHiding.isHiding && targetHidingPlace != null)
                {
                    navMeshAgent.destination = targetHidingPlace.hidingTransform.position;
                    if (Vector3.Distance(transform.position, targetHidingPlace.hidingTransform.position) <= catchDistance + 1f && !hasCaughtPlayer)
                    {
                        targetHidingPlace.FoundPlayer();
                        CatchPlayer();
                    }
                }
                else
                {
                    navMeshAgent.destination = player.position;
                    timeSinceLastSeenPlayer = 0;
                    lastKnownPosition = player.position;

                    if (Vector3.Distance(transform.position, player.position) <= catchDistance && !hasCaughtPlayer)
                    {
                        CatchPlayer();
                    }
                }
            }
            else
            {
                StartSearching();
            }
        }
        else if (isSearching)
        {
            SearchForPlayer();
        }
        else
        {
            Patrol();
            if (PlayerInSight())
            {
                isChasing = true;
            }
        }
    }

    void Patrol()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        navMeshAgent.destination = patrolPoints[patrolIndex].position;
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    void DetectObstacle()
    {
        Collider[] obstacles = Physics.OverlapSphere(transform.position, obstacleDetectionRadius, obstacleLayer);

        if (obstacles.Length > 0)
        {
            currentObstacle = obstacles[0].GetComponent<Obstacle>(); 
            if (currentObstacle != null)
            {
                if (currentObstacle.isUsed == true)
                {
                    Debug.Log("fixing");
                    fixingObstacle = true;
                    isChasing = false;  // Stop chasing player while fixing
                    navMeshAgent.destination = currentObstacle.transform.position;
                }
            }
        }
    }

    public void StartSearching()
    {
        isChasing = false;
        isSearching = true;
        searchStartTime = Time.time;
        navMeshAgent.destination = lastKnownPosition;
    }

    void SearchForPlayer()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            if (Time.time - searchStartTime < searchDuration)
            {
                Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
                randomDirection += lastKnownPosition;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, searchRadius, 1))
                {
                    navMeshAgent.destination = hit.position;
                }
            }
            else
            {
                isSearching = false;
                GoToNextPatrolPoint();  // Resume patrolling after search ends
            }
        }

        if (PlayerInSight())
        {
            isSearching = false;
            isChasing = true;
        }
    }

    bool PlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - eyePosition).normalized;
        float distanceToPlayer = Vector3.Distance(eyePosition, player.position);

        if (distanceToPlayer < chaseDistance)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer < visionAngle / 2f)
            {
                if (!Physics.Raycast(eyePosition, directionToPlayer, distanceToPlayer, obstructionLayers))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void CatchPlayer()
    {
        hasCaughtPlayer = true;
        isChasing = false;
        moneyManager.ClearMoney();
        aiSuspicion.ResetSuspicion();
        playerMovement.StunPlayer(stunDuration);
        DetectObstacle();
        GoToNextPatrolPoint();
        Invoke("ResetAIState", 6f);
    }

    void ResetAIState()
    {
        hasCaughtPlayer = false;
        isChasing = false;
        isSearching = false;
        aiSuspicion.ResetSuspicion();
    }

    public void StartChasing()  // Called when suspicion is full
    {
        if (!hasCaughtPlayer && !isChasing)
        {
            isChasing = true;
            navMeshAgent.destination = player.position;
            timeSinceLastSeenPlayer = 0;
            lastKnownPosition = player.position;
        }
    }

    void OnDrawGizmos()
    {
        // Draw vision cone in Scene view
        DrawVisionCone();

        if (isSearching)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(lastKnownPosition, searchRadius);
        }
    }

    void DrawVisionCone()
    {
        Gizmos.color = Color.green;
        Vector3 forward = transform.forward;

        // Draw vision cone (center line)
        Gizmos.DrawRay(eyePosition, forward * chaseDistance);

        // Draw left edge of the vision cone
        Quaternion leftRayRotation = Quaternion.AngleAxis(-visionAngle / 2, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * forward;
        Gizmos.DrawRay(eyePosition, leftRayDirection * chaseDistance);

        // Draw right edge of the vision cone
        Quaternion rightRayRotation = Quaternion.AngleAxis(visionAngle / 2, Vector3.up);
        Vector3 rightRayDirection = rightRayRotation * forward;
        Gizmos.DrawRay(eyePosition, rightRayDirection * chaseDistance);
    }
}