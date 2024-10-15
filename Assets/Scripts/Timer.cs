using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timer;
    public bool timeStarted = false;
    public TextMeshProUGUI timerText;
    public GameObject resultScreen;
    //public static bool gameEnded = false;
    public NPCMinigame[] npcMinigames;

    private void Start()
    {
        //gameEnded = false;
    }

    void Update()
    {
        if (timeStarted == true)
        {
            timer -= Time.deltaTime;
            UpdateTimer();  

            if (timer <= 0)
            {
                //Debug.Log("Game Over");
                timeStarted = false;
                ZeroTimer();
                //gameEnded = true;

                foreach(NPCMinigame npc in npcMinigames) 
                {
                    npc.CloseMiniGame();
                }

                resultScreen.SetActive(true);
            }
        }
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerText.text = niceTime;
    }

    void ZeroTimer()
    {
        int minutes = 0;
        int seconds = 0;

        string zeroTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerText.text = zeroTime;
    }
}
