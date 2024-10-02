using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney;
    public static int bankMoney;
    public TextMeshProUGUI currentMoneyText;
    public TextMeshProUGUI bankMoneyText;

    void Start()
    {
        currentMoney = 0;
        UpdateMoneyUI();    
    }

    public void AddMoney(int money)
    {
        currentMoney += money;
        UpdateMoneyUI();
    }

    public void ClearMoney()
    {
        currentMoney = 0;
        UpdateMoneyUI();
    }

    public void BankInMoney()
    {
        bankMoney += currentMoney;
        currentMoney = 0;
        UpdateMoneyUI();

        Debug.Log("Bank Money:" +  bankMoney);
    }

    public void UpdateMoneyUI()
    {
        currentMoneyText.text = currentMoney.ToString();
        //bankMoneyText.text = bankMoney.ToString();
    }
}
