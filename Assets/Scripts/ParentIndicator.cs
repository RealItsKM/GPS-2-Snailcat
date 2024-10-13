using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentIndicator : MonoBehaviour
{
    public List<GameObject> parentIndicators;  // List of UI indicators
    public RectTransform indicatorUI;          // The UI element that will rotate
    public Transform player;                   // Reference to the player's Transform
    public Transform enemy;                    // Reference to the enemy's Transform

    private bool isPlayerInTrigger = false;
    private bool isEnemyInTrigger = false;

    public void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
        else if (collision.CompareTag("Enemy"))
        {
            isEnemyInTrigger = true;
        }

        if (isPlayerInTrigger && isEnemyInTrigger)
        {
            // Enable the indicator UI when both Player and Enemy are in the room
            foreach (GameObject indicator in parentIndicators)
            {
                indicator.SetActive(true);
            }

            //RotateIndicatorTowardsEnemy();
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
        else if (collision.CompareTag("Enemy"))
        {
            isEnemyInTrigger = false;
        }

        if (!isPlayerInTrigger || !isEnemyInTrigger)
        {
            // Disable the indicator UI when either the Player or Enemy leaves the room
            foreach (GameObject indicator in parentIndicators)
            {
                indicator.SetActive(false);
            }
        }
    }

    /*
    private void RotateIndicatorTowardsEnemy()
    {
        // Calculate the direction from the player to the enemy
        Vector3 directionToEnemy = enemy.position - player.position;

        // Get the angle to rotate the UI element towards the enemy
        float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;

        // Rotate the indicator UI to point towards the enemy
        indicatorUI.rotation = Quaternion.Euler(0, 0, angle);
    }
    */
}
