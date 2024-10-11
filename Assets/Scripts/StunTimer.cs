using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StunTimer : MonoBehaviour
{
    public Image fillImage; 
    public float stunTime = 5f;
    private float fill;
    //public PlayerMovement playerMovement;
    public TextMeshProUGUI timerText;
    private int timerNumber;

    void Start()
    {
        fillImage.fillAmount = 0f;
        timerNumber = (int)stunTime + 1;
    }

    void Update()
    {
        if (PlayerMovement.isStunned)
        {
            fill += Time.deltaTime;
            fillImage.fillAmount = fill / stunTime;
            timerText.text = ((int)(timerNumber - fill)).ToString() + "s";

            if (fill >= stunTime)
            {
                ResetTimer();
            }
        }
    }

    private void ResetTimer()
    {
        fill = 0;
        fillImage.fillAmount = 0f;
        timerText.text = ((int)(timerNumber - fill)).ToString() + "s";
    }
}
