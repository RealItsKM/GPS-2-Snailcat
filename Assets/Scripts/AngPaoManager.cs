using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngPaoManager : MonoBehaviour
{
    public static int angPaoNumber;
    private int angPaoMoneyAmount;
    public MoneyManager moneyManager;

    private void Start()
    {
        angPaoNumber = 0;  
    }

    public void CollectAngPao()
    {
        angPaoMoneyAmount = Random.Range(5, 51);

        moneyManager.AddMoney(angPaoMoneyAmount);

        Debug.Log("Received " +  angPaoMoneyAmount);

        angPaoNumber++;
    }
}
