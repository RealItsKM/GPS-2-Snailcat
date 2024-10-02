using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMinigame : MonoBehaviour
{
    public GameObject gamePanel;

    public void StartMiniGame()
    {
        gamePanel.SetActive(true); 
    }

    public void CloseMiniGame()
    {
        gamePanel.SetActive(false);
    }
}
