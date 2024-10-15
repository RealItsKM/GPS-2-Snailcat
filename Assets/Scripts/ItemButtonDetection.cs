using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonDetection : MonoBehaviour
{
    public Button itemButton;
    public NPCMinigame npcMinigame;
    public HeldItem itemManager;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && itemManager.heldItem == npcMinigame.requiredItem)
        {
            itemButton.interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            itemButton.interactable = false;
        }
    }
}
