using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPCMinigame : MonoBehaviour
{
    public GameObject gamePanel;
    public Item requiredItem;
    public GameObject itemIcon;
    public Button itemButton;
    public GameObject minigameButton;
    public HeldItem itemManager;
    public bool itemGiven = false;
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

    public void GiveItem()
    {
        if (!itemGiven && itemManager.heldItem != null)
        {
            //itemManager.heldItem = null; //use item
            itemManager.LoseItem(); //use item
            itemGiven = true;

            if (TutorialMode.tutorialOn && TutorialMode.gaveFoodOrTea == false)
            {
                TutorialMode.gaveFoodOrTea = true;
            }

            itemIcon.SetActive(false);
            minigameButton.SetActive(true);
        }
    }

    public void UpdateIcons()
    {
        if (requiredItem != null && minigameButton != null && itemIcon != null && itemManager != null && itemButton != null)
        {
            if (itemManager.heldItem == requiredItem || itemGiven)
            {
                //itemIcon.SetActive(false);
                //minigameButton.SetActive(true);

                //itemButton.interactable = true;
            }
            else
            {
                //itemIcon.SetActive(true);
                //minigameButton.SetActive(false);

                //itemButton.interactable = false;
            }
        }
    }
}
