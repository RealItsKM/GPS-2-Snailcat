using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static int bankMoneyData;

    public void Save(int bankMoney)
    {
        bankMoneyData += bankMoney;
        PlayerPrefs.SetInt("BankMoney", bankMoneyData);
        Debug.Log("Saved " + bankMoneyData);
    }

    public void Load()
    {
        bankMoneyData = PlayerPrefs.GetInt("BankMoney");
        Debug.Log("Loaded " + bankMoneyData);
    }
}
