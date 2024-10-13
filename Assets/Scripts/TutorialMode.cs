using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialMode : MonoBehaviour
{
    //pop ups
    public GameObject tutorialUI;
    public GameObject a1;
    public GameObject a2;
    public GameObject b;
    public GameObject c;
    public GameObject d1;
    public GameObject d2;
    public GameObject e1;
    public GameObject e2;
    public GameObject finished;
    //private int currentPopUp = 0;

    //flags
    public static bool tutorialOn;
    public static bool collectedAngpao;
    public static bool gaveFoodOrTea;
    public static bool usedPiggyBank;
    public static bool usedObstacle;
    public static bool shortWaveOver;

    //objects
    public GameObject angpaoNPC;
    public GameObject foodNPC;
    public GameObject foodButton;
    public GameObject piggyBankButton;
    public GameObject obstacleButton;
    public GameObject enemy1;
    public GameObject timer;

    void Start()
    {
        tutorialOn = true;
        collectedAngpao = false;
        gaveFoodOrTea = false;
        usedPiggyBank = false;
        usedObstacle = false;
        shortWaveOver = false;

        //restrict player movement
        Invoke("PopUpA1", 2f);
        //enable player movement 
        angpaoNPC.SetActive(true); //enable npc with angpao
    }

    public void CollectAngpao() //AngPaoManager
    {
        collectedAngpao = true;
        Invoke("PopUpB", 1f);

        foodNPC.SetActive(true); //enable npc with food
        foodButton.SetActive(true); //enable food button
    }

    public void GiveFoodOrTea() //AngPaoManager & NPCMinigame
    {
        Invoke("PopUpC", 1f);

        piggyBankButton.SetActive(true); //enable piggybank button
    }

    public void UsePiggyBank() //MoneyManager
    {
        usedPiggyBank = true;
        Invoke("PopUpD1", 1f);

        obstacleButton.SetActive(true); //enable obstacle
        enemy1.SetActive(true); //enable enemy movement
    }

    public void UseObstacle() //Obstacle
    {
        usedObstacle = true;
        Invoke("PopUpE1", 1f);
    }

    public void StartShortWave()
    {
        //start short wave after closing
        //reset enemy position
        timer.SetActive(true); //enable timer
    }

    public void AfterShortWave() //after 1 minute
    {
        PopUpFinished(); //show finished tutorial pop up
    }


    public void OpenTutorialUI()
    {
        tutorialUI.SetActive(true);
    }

    public void CloseTutorialUI()
    {
        tutorialUI.SetActive(false);
    }

    public void PopUpA1() //goal
    {
        OpenTutorialUI();
        a1.SetActive(true);
        a2.SetActive(false);
        b.SetActive(false);
        c.SetActive(false);
        d1.SetActive(false);
        d2.SetActive(false);
        e1.SetActive(false);
        e2.SetActive(false);
    }

    public void PopUpA2() //movement
    {
        OpenTutorialUI();
        a1.SetActive(false);
        a2.SetActive(true);
        b.SetActive(false);
        c.SetActive(false);
        d1.SetActive(false);
        d2.SetActive(false);
        e1.SetActive(false);
        e2.SetActive(false);
    }

    public void PopUpB() //food and tea
    {
        OpenTutorialUI();
        a1.SetActive(false);
        a2.SetActive(false);
        b.SetActive(true);
        c.SetActive(false);
        d1.SetActive(false);
        d2.SetActive(false);
        e1.SetActive(false);
        e2.SetActive(false);
    }

    public void PopUpC() //piggy bank
    {
        OpenTutorialUI();
        a1.SetActive(false);
        a2.SetActive(false);
        b.SetActive(false);
        c.SetActive(true);
        d1.SetActive(false);
        d2.SetActive(false);
        e1.SetActive(false);
        e2.SetActive(false);
    }

    public void PopUpD1()
    {
        OpenTutorialUI();
        a1.SetActive(false);
        a2.SetActive(false);
        b.SetActive(false);
        c.SetActive(false);
        d1.SetActive(true);
        d2.SetActive(false);
        e1.SetActive(false);
        e2.SetActive(false);
    }

    public void PopUpD2()
    {
        OpenTutorialUI();
        a1.SetActive(false);
        a2.SetActive(false);
        b.SetActive(false);
        c.SetActive(false);
        d1.SetActive(false);
        d2.SetActive(true);
        e1.SetActive(false);
        e2.SetActive(false);
    }

    public void PopUpE1()
    {
        OpenTutorialUI();
        a1.SetActive(false);
        a2.SetActive(false);
        b.SetActive(false);
        c.SetActive(false);
        d1.SetActive(false);
        d2.SetActive(false);
        e1.SetActive(true);
        e2.SetActive(false);
    }

    public void PopUpE2()
    {
        OpenTutorialUI();
        a1.SetActive(false);
        a2.SetActive(false);
        b.SetActive(false);
        c.SetActive(false);
        d1.SetActive(false);
        d2.SetActive(false);
        e1.SetActive(false);
        e2.SetActive(true);
    }

    public void PopUpFinished()
    {
        OpenTutorialUI();
        finished.SetActive(true);
    }
}
