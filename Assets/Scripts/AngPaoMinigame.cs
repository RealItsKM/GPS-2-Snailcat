using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AngPaoMinigame : MonoBehaviour, IPointerDownHandler
{
    private int requiredTapNumber = 0;
    public int currentTapNumber = 0;
    public TextMeshProUGUI requiredTapNumberText;
    private bool gameFinished;
    public GameObject gamePanel;
    public AngPaoManager angPaoManager;
    public GameObject angPaoButton;

    void Start()
    {
        currentTapNumber = 0;

        requiredTapNumber = Random.Range(5, 51);

        requiredTapNumberText.text = requiredTapNumber.ToString();
    }

    /*
    public void StartMiniGame()
    {
        gamePanel.SetActive(true);
    }
    */

    public void OnPointerDown(PointerEventData eventData)
    {
        currentTapNumber++;
        //Debug.Log(currentTapNumber);
        requiredTapNumberText.text = (requiredTapNumber - currentTapNumber).ToString();

        if (currentTapNumber >= requiredTapNumber)
        {
            gameFinished = true;
            angPaoManager.CollectAngPao(requiredTapNumber);
            angPaoButton.SetActive(false);
            gamePanel.SetActive(false);
        }
    }
}
