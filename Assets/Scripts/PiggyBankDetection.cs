using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyBankDetection : MonoBehaviour
{
    public GameObject buttonBlocker;


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            buttonBlocker.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            buttonBlocker.SetActive(true);
        }
    }
}
