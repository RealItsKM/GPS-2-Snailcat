using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    public Transform[] patrolPoints;  // Points the AI will follow in a loop
    public Transform player;          // Reference to the player
    public float chaseDistance = 10f; // Distance at which the AI will start chasing the player
    public float sightAngle = 45f;    // Field of view angle for AI vision
    public float lostSightTime = 3f;  // Time AI waits before returning to patrol after losing sight of the player

    private NavMeshAgent agent;
    private int currentPatrolIndex;
    private bool isChasing;
    private float lastTimePlayerSeen;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPatrolIndex = 0;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (isChasing)
        {
            // If chasing, move towards the player
            ChasePlayer();
        }
        else
        {
            // Patrol the set path if not chasing
            Patrol();

            // Check if the player is in sight and within the chase range
            if (IsPlayerInSight())
            {
                isChasing = true;
            }
        }

        // If AI is chasing but loses sight of the player for a while, go back to patrol
        if (isChasing && Time.time - lastTimePlayerSeen > lostSightTime)
        {
            isChasing = false;
            GoToNextPatrolPoint();
        }
    }

    void Patrol()
    {
        // Move to the next patrol point when the agent reaches the current one
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void ChasePlayer()
    {
        agent.destination = player.position;
        lastTimePlayerSeen = Time.time;
    }

    bool IsPlayerInSight()
    {
        // Check if the player is within the chase distance
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < chaseDistance)
        {
            // Check if the player is within the AI's field of view
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < sightAngle / 2f)
            {
                // Check if there is a clear line of sight to the player
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, chaseDistance))
                {
                    if (hit.transform == player)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
