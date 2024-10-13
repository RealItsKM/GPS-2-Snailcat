using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    public MoneyManager moneyManager;
    //public AngPaoManager angPaoManager;
    public TextMeshProUGUI moneyCollectedText;
    public TextMeshProUGUI angPaoCollectedText;
    public TextMeshProUGUI timesCaughtText;

    public Slider gradeSlider;

    public GameObject gradeS;
    public GameObject gradeA;
    public GameObject gradeB;
    public GameObject gradeC;
    public GameObject gradeD;
    public GameObject gradeE;
    public GameObject gradeF;

    void Start()
    {
        moneyCollectedText.text = MoneyManager.currentBankMoney.ToString();
        angPaoCollectedText.text = AngPaoManager.angPaoNumber.ToString();
        timesCaughtText.text = PlayerMovement.timesCaught.ToString();

        gradeS.SetActive(false);
        gradeA.SetActive(false);
        gradeB.SetActive(false);
        gradeC.SetActive(false);
        gradeD.SetActive(false);
        gradeE.SetActive(false);
        gradeF.SetActive(false);

        CalculateGrade();

        moneyManager.TotalBankMoney();
    }

    void CalculateGrade()
    {
        int bankMoney = MoneyManager.currentBankMoney;  // Get the current bank money value
        char grade = 'F';  // Default to F grade
        float sliderValue = 0f;  // Default slider value

        // Determine grade based on bankMoney value
        if (bankMoney >= 250)
        {
            grade = 'S';
            sliderValue = 1f;
            gradeS.SetActive(true);
        }
        else if (bankMoney >= 200)
        {
            grade = 'A';
            sliderValue = 0.83f;
            gradeA.SetActive(true);
        }
        else if (bankMoney >= 150)
        {
            grade = 'B';
            sliderValue = 0.66f;
            gradeB.SetActive(true);
        }
        else if (bankMoney >= 100)
        {
            grade = 'C';
            sliderValue = 0.49f;
            gradeC.SetActive(true);
        }
        else if (bankMoney >= 50)
        {
            grade = 'D';
            sliderValue = 0.325f;
            gradeD.SetActive(true);
        }
        else if (bankMoney >= 20)
        {
            grade = 'E';
            sliderValue = 0.16f;
            gradeE.SetActive(true);
        }
        else
        {
            grade = 'F';
            sliderValue = 0f;
            gradeF.SetActive(true);
        }

        
        gradeSlider.value = sliderValue;

        //Debug.Log("Player Grade: " + grade);
    }
}
