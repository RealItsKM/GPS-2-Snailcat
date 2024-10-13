using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngPaoManager : MonoBehaviour
{
    public static int angPaoNumber;
    private int angPaoMoneyAmount;
    public MoneyManager moneyManager;
    public TutorialMode tutorialManager;

    private void Start()
    {
        angPaoNumber = 0;  
    }

    public void CollectAngPao(int num)
    {
        /*
        angPaoMoneyAmount = Random.Range(5, 51);

        moneyManager.AddMoney(angPaoMoneyAmount);
        
        Debug.Log("Received " +  angPaoMoneyAmount);
        */

        /*
        int rng = Random.Range(1, 21);

        if (rng <= 10 ) //50%
        {
            moneyManager.AddMoney(5);
        }
        else if (rng <= 15) //25%
        {
            moneyManager.AddMoney(10);
        }
        else if (rng <= 18) //15%
        {
            moneyManager.AddMoney(20);
        }
        else //10%
        {
            moneyManager.AddMoney(50);
        }
        */

        moneyManager.AddMoney(num);

        Debug.Log("Received " + num);

        angPaoNumber++;

        if(TutorialMode.tutorialOn)
        {
            if(TutorialMode.gaveFoodOrTea == false && TutorialMode.collectedAngpao == false)
            {
                tutorialManager.CollectAngpao();
            }
            else if (TutorialMode.gaveFoodOrTea == true && TutorialMode.collectedAngpao == true)
            {
                tutorialManager.GiveFoodOrTea();
            }
        }
    }
}
