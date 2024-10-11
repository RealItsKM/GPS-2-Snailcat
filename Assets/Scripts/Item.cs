using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]

public class Item : ScriptableObject
{
    public int numberID;
    public string itemName;
    public Sprite itemArt;
    //public Image itemArt;
}
