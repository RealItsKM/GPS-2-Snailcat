using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    public GameObject player;
    public GameObject joystick;
    private bool isHidden = false;
    public Transform hidingTransform; //for enemy ai

    public void HideButton()
    {
        if (isHidden)
        {
            Unhide();
        }
        else
        {
            Hide(); 
        }
    }

    public void Hide()
    {
        if (PlayerMovement.isStunned == false)
        {
            player.SetActive(false);
            joystick.SetActive(false);
            isHidden = true;
            PlayerHiding.isHiding = true;
        }
    }

    public void Unhide()
    {
        player.SetActive(true);
        joystick.SetActive(true);
        isHidden = false;
        PlayerHiding.isHiding = false;
    }

    public void FoundPlayer()
    {
        player.SetActive(true);
        joystick.SetActive(true);
        isHidden = false;
        PlayerHiding.isHiding = false;
    }
}
