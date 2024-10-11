using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TeaMinigame : MonoBehaviour, IPointerDownHandler
{
    public int requiredTapNumber = 0;
    public int currentTapNumber = 0;
    public TextMeshProUGUI requiredTapNumberText;
    public GameObject gamePanel;
    public HeldItem itemManager;
    public NPCMinigame[] npcMinigame;

    void Start()
    {
        RefreshMinigame();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentTapNumber++;
        requiredTapNumberText.text = (requiredTapNumber - currentTapNumber).ToString();

        if (currentTapNumber >= requiredTapNumber)
        {
            itemManager.GetItem(1);
            RefreshMinigame();
            ActivateUpdateIcons();
            gamePanel.SetActive(false);
        }
    }

    public void RefreshMinigame()
    {
        currentTapNumber = 0;
        requiredTapNumberText.text = requiredTapNumber.ToString();
    }

    void ActivateUpdateIcons()
    {
        foreach (NPCMinigame npc in npcMinigame)
        {
            if (npc != null)  
            {
                npc.UpdateIcons();
            }
        }
    }
}
