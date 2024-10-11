using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedVision : MonoBehaviour
{
    public GameObject darkOverlay;

    /*
    public void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            darkOverlay.SetActive(false);
        }
    }
    */

    public void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            darkOverlay.SetActive(false);
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            darkOverlay.SetActive(true);
        }
    }
}
