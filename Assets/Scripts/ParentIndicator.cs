using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentIndicator : MonoBehaviour
{
    public List<GameObject> parentIndicators;
    private bool isPlayerInTrigger = false;
    private bool isEnemyInTrigger = false;

    public void OnTriggerStay(Collider collision)
    {
        // Check if the object entering the trigger is the Player
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
        // Check if the object entering the trigger is the Enemy
        else if (collision.CompareTag("Enemy"))
        {
            isEnemyInTrigger = true;
        }

        if (isPlayerInTrigger && isEnemyInTrigger)
        {
            // Enable the indicator if both Player and Enemy are in the trigger
            foreach (GameObject indicator in parentIndicators)
            {
                indicator.SetActive(true); // Enable or disable each indicator
            }
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        // Check if the object exiting the trigger is the Player
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
        // Check if the object exiting the trigger is the Enemy
        else if (collision.CompareTag("Enemy"))
        {
            isEnemyInTrigger = false;
        }

        if (!isPlayerInTrigger || !isEnemyInTrigger)
        {
            // Disable the indicator if either the Player or the Enemy leaves the trigger
            foreach (GameObject indicator in parentIndicators)
            {
                indicator.SetActive(false); // Enable or disable each indicator
            }
        }
    }
}
