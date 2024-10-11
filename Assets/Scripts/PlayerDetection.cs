using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetection : MonoBehaviour
{
    public Button minigameButton;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            minigameButton.interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            minigameButton.interactable = false;
        }
    }
}
