using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWalking : MonoBehaviour
{
    public List<Transform> walkingPoints;
    public float walkSpeed = 3f;     
    public bool canWalk = true;          

    private int currentPointIndex = 0;     // The current point the NPC is walking to
    private NavMeshAgent navMeshAgent;     // Reference to the NavMeshAgent for movement

    public NPCMinigame npcMinigame;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;

        if (walkingPoints.Count > 0)
        {
            navMeshAgent.destination = walkingPoints[currentPointIndex].position;  // Start walking to the first point
        }
    }

    void Update()
    {
        if (npcMinigame.minigameOn == false && canWalk && walkingPoints.Count > 0)
        {
            // Check if the NPC has reached its current destination
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
            {
                GoToNextPoint();
            }
        }
        else if (npcMinigame.minigameOn == true)
        {
            //navMeshAgent.isStopped = true; // Stop movement when canWalk is false
            StopWalking();
        }
        else if (canWalk == true)
        {
            ResumeWalking();
        }
    }

    // Move to the next point in the list
    void GoToNextPoint()
    {
        if (walkingPoints.Count == 0) return;

        //loop back to the first point if at the end
        currentPointIndex = (currentPointIndex + 1) % walkingPoints.Count;

        // Set the next destination
        navMeshAgent.destination = walkingPoints[currentPointIndex].position;
    }

    public void StopWalking()
    {
        canWalk = false;
        navMeshAgent.isStopped = true;
    }

    public void ResumeWalking()
    {
        canWalk = true;
        navMeshAgent.isStopped = false;
        navMeshAgent.destination = walkingPoints[currentPointIndex].position;  // Resume walking to the next point
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            StopWalking();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ResumeWalking();
        }
    }
}
