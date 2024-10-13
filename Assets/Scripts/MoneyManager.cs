using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney;
    public static int currentBankMoney;
    public static int bankMoney;
    public TextMeshProUGUI currentMoneyText;
    public TextMeshProUGUI bankMoneyText;
    public TextMeshProUGUI startScreenBankMoneyText;
    public SaveData dataManager;
    public TutorialMode tutorialManager;

    void Start()
    {
        currentMoney = 0;
        UpdateMoneyUI();    
        StartScreenMoney();
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
        currentBankMoney += currentMoney;
        currentMoney = 0;
        UpdateMoneyUI();

        Debug.Log("Current Bank Money:" +  currentBankMoney);

        if(TutorialMode.tutorialOn && TutorialMode.usedObstacle == false)
        {
            tutorialManager.UsePiggyBank();
        }
    }

    public void TotalBankMoney()
    {
        bankMoney += currentBankMoney;
        currentBankMoney = 0;
        UpdateMoneyUI();

        if(TutorialMode.tutorialOn == false)
        {
            dataManager.Save(bankMoney);
            StartScreenMoney();

            Debug.Log("Total Bank Money:" + bankMoney);
        }

        //current bank money is redundant
    }

    public void UpdateMoneyUI()
    {
        if(currentMoneyText != null && bankMoneyText != null)
        {
            currentMoneyText.text = currentMoney.ToString();
            bankMoneyText.text = currentBankMoney.ToString();
        }
    }

    public void StartScreenMoney()
    {
        if (startScreenBankMoneyText != null)
        {
            //startScreenBankMoneyText.text = bankMoney.ToString();
            //startScreenBankMoneyText.text = dataManager.bankMoneyData.ToString();
            dataManager.Load();
            startScreenBankMoneyText.text = SaveData.bankMoneyData.ToString();
            //Debug.Log(SaveData.bankMoneyData);
        }
    }
}
