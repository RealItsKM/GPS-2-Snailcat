using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCMinigame : MonoBehaviour
{
    public GameObject gamePanel;
    public Item requiredItem;
    public GameObject itemIcon;
    public GameObject minigameButton;
    public HeldItem itemManager;
    private bool itemGiven = false;
    public bool minigameOn = false;
    //public TutorialMode tutorialManager;

    private void Start()
    {
        UpdateIcons();
    }

    private void Update()
    {
        if (PlayerMovement.isStunned)
        {
            CloseMiniGame();
        }
    }

    public void StartMiniGame()
    {
        UpdateIcons();

        if (PlayerMovement.isStunned)
        {
            return;
        }

        if (!itemGiven && itemManager.heldItem != null)
        {
            itemManager.heldItem = null; //use item
            itemGiven = true;

            if (TutorialMode.tutorialOn && TutorialMode.gaveFoodOrTea == false)
            {
                TutorialMode.gaveFoodOrTea = true;
            }
        }
        
        itemManager.UpdateItemUI();
        minigameOn = true;
        gamePanel.SetActive(true);
    }

    public void CloseMiniGame()
    {
        minigameOn = false;
        //Debug.Log("A");
        gamePanel.SetActive(false);
    }

    public void UpdateIcons()
    {
        if (requiredItem != null && minigameButton != null && itemIcon != null && itemManager != null)
        {
            if (itemManager.heldItem == requiredItem || itemGiven)
            {
                itemIcon.SetActive(false);
                minigameButton.SetActive(true);
            }
            else
            {
                itemIcon.SetActive(true);
                minigameButton.SetActive(false);
            }
        }
    }
}
