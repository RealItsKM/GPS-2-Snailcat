using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    //public MoneyManager moneyManager;
    //public AngPaoManager angPaoManager;
    public TextMeshProUGUI moneyCollectedText;
    public TextMeshProUGUI angPaoCollectedText;
    //times caught

    public Slider gradeSlider;

    void Start()
    {
        moneyCollectedText.text = MoneyManager.bankMoney.ToString();
        angPaoCollectedText.text = AngPaoManager.angPaoNumber.ToString();

        CalculateGrade();
    }

    void CalculateGrade()
    {
        gradeSlider.value = gradeSlider.maxValue;
    }
}
