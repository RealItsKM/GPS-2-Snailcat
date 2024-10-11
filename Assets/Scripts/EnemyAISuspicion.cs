using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAISuspicion : MonoBehaviour
{
    public Transform player;
    public float suspicionIncreaseRate = 10f; // How fast the suspicion increases
    public float suspicionDecreaseRate = 5f;  // How fast the suspicion decreases when the player is out of the circle
    public float maxSuspicion = 100f;         // Maximum suspicion before AI starts chasing
    public float detectionRadius = 5f;        // Radius within which suspicion increases
    public Image suspicionBar;                // Reference to the UI element for the suspicion bar
    public LayerMask obstructionLayers;       // Layers that block suspicion detection (like walls)

    private float currentSuspicion = 0f;
    private bool isPlayerDetected = false;
    private EnemyAIController aiController;

    void Start()
    {
        // Reference to the AI Controller (ensure both scripts are on the same GameObject)
        aiController = GetComponent<EnemyAIController>();
        UpdateSuspicionBar();
    }

    void Update()
    {
        // Check if the player is within the detection radius and not obstructed by walls
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius && !IsObstructed() && aiController.hasCaughtPlayer == false && PlayerHiding.isHiding == false)
        {
            isPlayerDetected = true;
            IncreaseSuspicion();
        }
        else
        {
            isPlayerDetected = false;
            DecreaseSuspicion();
        }

        // If suspicion reaches the max, start chasing the player
        if (currentSuspicion >= maxSuspicion)
        {
            //Debug.Log("Suspicion Full");
            aiController.StartChasing();  // Let the AI know the player's position and start chasing
        }
    }

    void IncreaseSuspicion()
    {
        if (currentSuspicion < maxSuspicion)
        {
            currentSuspicion += suspicionIncreaseRate * Time.deltaTime;
            UpdateSuspicionBar();
        }
    }

    void DecreaseSuspicion()
    {
        if (currentSuspicion > 0 && !aiController.isChasing)
        {
            currentSuspicion -= suspicionDecreaseRate * Time.deltaTime;
            UpdateSuspicionBar();
        }
    }

    public void ResetSuspicion()
    {
        //Debug.Log("Suspicion Reset");
        currentSuspicion = 0;
        UpdateSuspicionBar();
    }

    void UpdateSuspicionBar()
    {
        // Assuming suspicionBar is a UI slider or fillable image
        suspicionBar.fillAmount = currentSuspicion / maxSuspicion;
    }

    // Call this when the AI catches the player or loses sight
    public void OnPlayerCaughtOrLost()
    {
        ResetSuspicion();
    }

    // Check if there is an obstruction between the AI and the player
    bool IsObstructed()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Raycast between the AI and the player
        if (Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionLayers))
        {
            // If the ray hits something on the obstruction layer, return true (player is behind an obstacle)
            return true;
        }

        // No obstruction, the player is visible
        return false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a semi-transparent circle to visualize the detection radius in the scene view
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}

