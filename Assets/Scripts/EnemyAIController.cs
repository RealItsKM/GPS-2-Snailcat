using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public Transform[] patrolPoints;       // Points the AI will patrol
    public Transform player;               
    public float chaseDistance = 10f;      // Distance at which AI starts chasing
    public float visionAngle = 60f;        // Vision cone angle
    public float lostSightDuration = 2f;   // Time after losing sight to stop chasing
    public float searchDuration = 5f;      // Time to search for the player after losing sight
    public float searchRadius = 3f;        // Radius around last seen position to roam
    public float catchDistance = 2f;       // Distance at which AI can catch the player
    public float stunDuration = 3f;      
    public LayerMask obstructionLayers;    // Layers that can block sight (e.g., walls)

    private NavMeshAgent navMeshAgent;
    private int patrolIndex = 0;
    private float timeSinceLastSeenPlayer;
    public bool isChasing = false;
    private bool isSearching = false;
    public static bool hasCaughtPlayer = false;
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
        //timeSinceLastSeenPlayer = lostSightDuration; 
        timeSinceLastSeenPlayer = 0f;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        /*
        if (TutorialMode.tutorialOn)
        {
            Debug.Log("Tutorial On");
            navMeshAgent.enabled = false;
            return;
        }
        else
        {
            navMeshAgent.enabled = true;
        }
        */

        timeSinceLastSeenPlayer += Time.deltaTime;

        // Debug: Check current state
        //Debug.Log($"AI State: isChasing={isChasing}, isSearching={isSearching}, hasCaughtPlayer={hasCaughtPlayer}, PlayerHiding={PlayerHiding.isHiding}");

        if (hasCaughtPlayer)
        {
            Patrol();  // Keep patrolling after catching the player
            return;
        }

        if (isChasing)
        {
            // Check if player is in sight
            if (PlayerInSight())
            {
                //Debug.Log("AI sees the player");

                // If player is hiding and AI has found a hiding place
                if (PlayerHiding.isHiding && targetHidingPlace != null)
                {
                    Debug.Log("Player is hiding. Moving towards hiding place...");
                    navMeshAgent.destination = targetHidingPlace.hidingTransform.position;  // Move to hiding place

                    // Check if AI reaches the hiding place
                    if (Vector3.Distance(transform.position, targetHidingPlace.hidingTransform.position) <= catchDistance + 1f && !hasCaughtPlayer)
                    {
                        targetHidingPlace.FoundPlayer();  // Found player in hiding place
                        Debug.Log("AI caught the player at the hiding place");
                        CatchPlayer();  // Catch the player
                    }
                }
                else  // If player is not hiding, chase normally
                {
                    //Debug.Log("Chasing player");
                    navMeshAgent.destination = player.position;  // Chase the player
                    timeSinceLastSeenPlayer = 0;
                    lastKnownPosition = player.position;

                    // Check if AI catches the player
                    if (Vector3.Distance(transform.position, player.position) <= catchDistance && !hasCaughtPlayer)
                    {
                        //Debug.Log("AI caught the player");
                        CatchPlayer();  // Catch the player
                    }
                }
            }
            else
            {
                //Debug.Log("Player not in sight, starting to search");
                StartSearching();
            }
        }
        else if (isSearching)
        {
            SearchForPlayer();
        }
        else
        {
            Patrol();  // Patrol if not chasing or searching

            // If player is sighted during patrol, start chasing
            if (PlayerInSight())
            {
                //Debug.Log("Player seen during patrol, start chasing");
                isChasing = true;
            }
        }

        // Debug: Check if AI has a destination
        if (navMeshAgent.destination != null)
        {
            //Debug.Log($"Current AI Destination: {navMeshAgent.destination}");
        }
        else
        {
            //Debug.Log("No destination set for AI");
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

        // If player is found during search
        if (PlayerInSight())
        {
            isSearching = false;
            isChasing = true;  // Resume chasing if player is found during search
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
                    if (PlayerHiding.isHiding && timeSinceLastSeenPlayer <= lostSightDuration)
                    {
                        targetHidingPlace = FindClosestHidingPlace();
                        if (targetHidingPlace != null)
                        {
                            Debug.Log("AI saw player hide. Moving to hiding spot.");
                            timeSinceLastSeenPlayer = 0;
                            return true;
                        }
                    }
                    else if (PlayerHiding.isHiding)
                    {
                        Debug.Log("Player is hiding, but AI did not see them.");
                        targetHidingPlace = null;
                        return false;
                    }
                    else
                    {
                        Debug.Log("Player in sight, chasing.");
                        timeSinceLastSeenPlayer = 0;
                        targetHidingPlace = null;
                        return true;
                    }
                }
            }
        }

        Debug.Log("Player not in sight.");
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

        // Handle post-catch logic (e.g., reset money, suspicion, stun player)
        moneyManager.ClearMoney();
        aiSuspicion.ResetSuspicion();
        playerMovement.StunPlayer(stunDuration);

        Debug.Log("Caught Player");

        GoToNextPatrolPoint();  // Continue patrolling after catching the player
        Invoke("ResetAIState", 6f);  // Reset AI state after some time
    }

    void ResetAIState()
    {
        hasCaughtPlayer = false;  // AI is ready to chase again
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