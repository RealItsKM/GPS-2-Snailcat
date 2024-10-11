using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeldItem : MonoBehaviour
{
    public Item[] itemList;
    public Item heldItem;
    public Image itemImage;
    public TextMeshProUGUI itemNameText;

    private void Start()
    {
        UpdateItemUI();
    }

    public void UpdateItemUI()
    {
        if (heldItem != null)
        {
            itemImage.sprite = heldItem.itemArt;
            itemNameText.text = heldItem.itemName;
        }
        else
        {
            itemNameText.text = "None";
        }
    }

    public void GetItem(int itemID)
    {
        /*
        switch (itemID)
        {
            case 0:
                heldItem = itemList[]
        }
        */

        heldItem = itemList[itemID];
        UpdateItemUI();
    }

    public void LoseItem()
    {
        heldItem = null;
        UpdateItemUI();
    }
}
