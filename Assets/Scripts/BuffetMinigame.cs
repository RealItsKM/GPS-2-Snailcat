using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BuffetMinigame : MonoBehaviour, IPointerDownHandler
{
    public int requiredTapNumber = 0;
    public int currentTapNumber = 0;
    public TextMeshProUGUI requiredTapNumberText;
    public GameObject gamePanel;
    public HeldItem itemManager;
    public NPCMinigame[] npcMinigame;

    // New Variables
    public List<Sprite> imageList;  // List of images for the UI
    public Image uiImage;           // UI Image component to update
    private int currentImageIndex = 0;

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
            itemManager.GetItem(0);
            RefreshMinigame();
            ActivateUpdateIcons();
            gamePanel.SetActive(false);
        }

        UpdateImage();  // Change image after each tap
    }

    public void RefreshMinigame()
    {
        currentTapNumber = 0;
        requiredTapNumberText.text = requiredTapNumber.ToString();
        currentImageIndex = 0;  // Reset to the first image
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

    void UpdateImage()
    {
        if (imageList.Count > 0)
        {
            currentImageIndex = (currentImageIndex + 1) % imageList.Count;  // Move to the next image
            uiImage.sprite = imageList[currentImageIndex];
        }
    }
}

