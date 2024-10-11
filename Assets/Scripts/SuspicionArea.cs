using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspicionArea : MonoBehaviour
{
    public Transform aiTransform;         // Reference to the AI
    public float suspicionRadius = 5f;    // Radius of the suspicion area
    public Color suspicionColor = new Color(1f, 0f, 0f, 0.3f); // Red, semi-transparent
    public LayerMask obstructionLayers;   // LayerMask for walls or obstacles

    private SpriteRenderer spriteRenderer;  // For visualizing the circle
    private int rayCount = 36;              // Number of rays for checking obstruction (more rays = more accurate)

    void Start()
    {
        // Create a GameObject for the suspicion circle
        GameObject suspicionCircle = new GameObject("SuspicionCircle");
        suspicionCircle.transform.SetParent(aiTransform);
        suspicionCircle.transform.localPosition = Vector3.zero;

        // Add SpriteRenderer component
        spriteRenderer = suspicionCircle.AddComponent<SpriteRenderer>();

        // Assign a circle sprite (make sure you have a sprite assigned in Unity)
        spriteRenderer.sprite = Resources.Load<Sprite>("CircleSprite"); // Replace with the path to your sprite
        spriteRenderer.color = suspicionColor; // Set color with transparency

        // Scale the circle to match the suspicion radius
        float diameter = suspicionRadius * 2;
        suspicionCircle.transform.localScale = new Vector3(diameter, diameter, 1f);
    }

    void Update()
    {
        // Ensure the suspicion circle always stays with the AI
        spriteRenderer.transform.position = aiTransform.position;

        // Perform raycast checks to simulate area being blocked by walls
        CheckObstructions();
    }

    void CheckObstructions()
    {
        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the direction of each ray
            float angle = i * angleStep;
            Vector3 rayDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));

            Ray ray = new Ray(aiTransform.position, rayDirection);
            RaycastHit hit;

            // Cast ray and check if it hits an obstruction layer
            if (Physics.Raycast(ray, out hit, suspicionRadius, obstructionLayers))
            {
                // If the ray hits an obstruction, scale down the suspicion area visual accordingly
                float distanceToObstacle = hit.distance;

                // Optionally, you could reduce the alpha transparency or show that part of the circle differently
                // Here, you could implement some visual feedback to represent the blocked part
                // For now, we're just doing a debug draw to visualize it
                Debug.DrawRay(aiTransform.position, rayDirection * distanceToObstacle, Color.red);
            }
            else
            {
                // If the ray doesn't hit an obstruction, we can draw the full radius
                Debug.DrawRay(aiTransform.position, rayDirection * suspicionRadius, Color.green);
            }
        }
    }
}
