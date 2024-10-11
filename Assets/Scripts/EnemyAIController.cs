using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public Transform[] patrolPoints;       // Points the AI will patrol
    public Transform player;               // Reference to the player
    public float chaseDistance = 10f;      // Distance at which AI starts chasing
    public float visionAngle = 60f;        // Vision cone angle
    public float lostSightDuration = 2f;   // Time after losing sight to stop chasing
    public float searchDuration = 5f;      // Time to search for the player after losing sight
    public float searchRadius = 3f;        // Radius around last seen position to roam
    public float catchDistance = 2f;       // Distance at which AI can catch the player
    public float stunDuration = 3f;        // How long the player will be stunned
    public LayerMask obstructionLayers;    // Layers that can block sight (e.g., walls)

    private NavMeshAgent navMeshAgent;
    private int patrolIndex = 0;
    private float timeSinceLastSeenPlayer;
    public bool isChasing = false;
    private bool isSearching = false;
    public bool hasCaughtPlayer = false;
    private Vector3 lastKnownPosition;     // Last known position of the player
    private float searchStartTime;
    private Vector3 eyePositionOffset = new Vector3(0, 1.5f, 0); // Adjust where the ray starts from
    private Vector3 eyePosition => transform.position + eyePositionOffset;

    public PlayerMovement playerMovement;
    public MoneyManager moneyManager;
    public EnemyAISuspicion aiSuspicion;

    public List<HidingPlace> hidingPlaces;
    private HidingPlace targetHidingPlace = null;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //timeSinceLastSeenPlayer = lostSightDuration; // Start by patrolling
        timeSinceLastSeenPlayer = 0f;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        timeSinceLastSeenPlayer += Time.deltaTime;

        if (hasCaughtPlayer)
            return;

        if (isChasing)
        {
            if (PlayerInSight())
            {
                if (PlayerHiding.isHiding && targetHidingPlace != null)
                {
                    // Move to the hiding place
                    navMeshAgent.destination = targetHidingPlace.transform.position;

                    Debug.Log("1");

                    // Check if AI has reached the hiding place
                    if (Vector3.Distance(transform.position, targetHidingPlace.hidingTransform.position) <= catchDistance && !hasCaughtPlayer)
                    {
                        Debug.Log("2");
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

    /*
    void Update()
    {
        if (hasCaughtPlayer)
            return; // Skip chasing logic while catching

        if (isChasing)
        {
            if (PlayerInSight())
            {
                navMeshAgent.destination = player.position;
                timeSinceLastSeenPlayer = 0;
                lastKnownPosition = player.position; // Update last known position

                // If AI is close enough to catch the player
                if (Vector3.Distance(transform.position, player.position) <= catchDistance && !hasCaughtPlayer)
                {
                    CatchPlayer();
                }
            }
            else
            {
                // If player is not in sight, search for a limited time
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

            // Immediately start chasing if player comes into sight
            if (PlayerInSight())
            {
                isChasing = true;
            }
        }
    }
    */

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

    public void StartSearching()
    {
        isChasing = false;
        isSearching = true;
        searchStartTime = Time.time;
        navMeshAgent.destination = lastKnownPosition;

        Debug.Log("Searching");
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
                GoToNextPatrolPoint();
            }
        }

        // Check if player is in sight again while searching
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
                    if (PlayerHiding.isHiding == true && timeSinceLastSeenPlayer <= lostSightDuration) // ai saw player hide
                    {
                        // Find the closest hiding place
                        targetHidingPlace = FindClosestHidingPlace();
                        if (targetHidingPlace != null)
                        {
                            timeSinceLastSeenPlayer = 0;
                            Debug.Log("a");
                            Debug.Log(targetHidingPlace.gameObject);
                            return true; 
                        }
                    }
                    else if (PlayerHiding.isHiding) //ai did not saw player hide
                    {
                        targetHidingPlace = null;
                        Debug.Log("b");
                        return false;
                    }
                    else //player in range
                    {
                        timeSinceLastSeenPlayer = 0;
                        targetHidingPlace = null;  
                        //Debug.Log("c");
                        return true;  
                    }
                }
            }
        }

        //Debug.Log("d"); 
        //player not in range
        return false;
    }

    HidingPlace FindClosestHidingPlace()
    {
        HidingPlace closestHidingPlace = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hidingPlace in hidingPlaces)
        {
            float distance = Vector3.Distance(player.position, hidingPlace.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHidingPlace = hidingPlace;
            }
        }

        return closestHidingPlace;
    }

    void CatchPlayer()
    {
        hasCaughtPlayer = true;
        isChasing = false;

        moneyManager.ClearMoney();
        aiSuspicion.ResetSuspicion();
        playerMovement.StunPlayer(stunDuration);

        Debug.Log("Caught");

        GoToNextPatrolPoint();
        Invoke("ResetAIState", 6f);
    }

    void ResetAIState()
    {
        hasCaughtPlayer = false; // AI is ready to chase again
        //GoToNextPatrolPoint();   // Return to patrolling

        isChasing = false;
        isSearching = false;
        aiSuspicion.ResetSuspicion();
    }

    public void StartChasing() //suspicion full
    {
        if (!hasCaughtPlayer && !isChasing)
        {
            isChasing = true;
            navMeshAgent.destination = player.position;  // Start heading toward the player's position
            timeSinceLastSeenPlayer = 0;
            lastKnownPosition = player.position;         // Set the last known position
        }
    }

    // Debugging in Scene View
    void OnDrawGizmos()
    {
        if (player != null)
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(eyePosition, player.position);
        }

        // Draw AI vision cone in Scene view
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