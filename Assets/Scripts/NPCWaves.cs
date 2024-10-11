using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaves : MonoBehaviour
{
    public List<GameObject> npcsToEnable; 
    public GameObject objectA;             // Object A to enable for the first 10 seconds of each wave
    public GameObject objectB;             // Object B to enable for the last 10 seconds of each wave
    public GameObject objectC;             // Object C for the first 10 seconds of the last wave
    public int numberOfWaves = 5;          
    public float timePerWave = 30f;       

    private int currentWave = 0;          
    private float waveTimer = 0f;          

    private bool objectAEnabled = false;
    private bool objectBEnabled = false;
    private bool objectCEnabled = false;  

    void Start()
    {
        numberOfWaves = Mathf.Min(numberOfWaves, npcsToEnable.Count);

        StartWave();
    }

    void Update()
    {
        if (currentWave < numberOfWaves)
        {
            waveTimer += Time.deltaTime;

            // Special case for the last wave (objectC)
            if (currentWave == numberOfWaves - 1)
            {
                // Enable Object C for the first 10 seconds
                if (waveTimer <= 10f && !objectCEnabled)
                {
                    objectC.SetActive(true);
                    objectCEnabled = true;
                    //Debug.Log("Object C enabled for the first 10 seconds of the last wave.");
                }
                else if (waveTimer > 10f && objectCEnabled)
                {
                    objectC.SetActive(false);
                    objectCEnabled = false;
                    //Debug.Log("Object C disabled after the first 10 seconds of the last wave.");
                }
            }
            else
            {
                // Enable Object A for the first 10 seconds of each wave
                if (waveTimer <= 10f && !objectAEnabled)
                {
                    objectA.SetActive(true);
                    objectAEnabled = true;
                    //Debug.Log("Object A enabled for the first 10 seconds of the wave.");
                }
                else if (waveTimer > 10f && objectAEnabled)
                {
                    objectA.SetActive(false);
                    objectAEnabled = false;
                    //Debug.Log("Object A disabled after the first 10 seconds of the wave.");
                }

                // Enable Object B for the last 10 seconds of each wave
                if (waveTimer >= timePerWave - 10f && waveTimer <= timePerWave && !objectBEnabled)
                {
                    objectB.SetActive(true);
                    objectBEnabled = true;
                    //Debug.Log("Object B enabled for the last 10 seconds of the wave.");
                }
                else if (waveTimer > timePerWave && objectBEnabled)
                {
                    objectB.SetActive(false);
                    objectBEnabled = false;
                    //Debug.Log("Object B disabled after the wave.");
                }
            }

            // Check if the wave time has completed
            if (waveTimer >= timePerWave)
            {
                EndWave();
            }
        }
    }

    void StartWave()
    {
        if (currentWave < numberOfWaves)
        {
            // Enable the NPC for the current wave
            npcsToEnable[currentWave].SetActive(true);
            //Debug.Log("Wave " + (currentWave + 1) + " started, enabling NPC: " + npcsToEnable[currentWave].name);

            // Reset the wave timer
            waveTimer = 0f;

            // Reset object states for the new wave
            objectAEnabled = false;
            objectBEnabled = false;
            objectCEnabled = false;  // Reset Object C for the last wave

            // Ensure Object B is disabled at the beginning of each wave except the last one
            if (currentWave != numberOfWaves - 1)
            {
                objectB.SetActive(false);
            }

            // Ensure Object C is disabled if it's not the last wave
            objectC.SetActive(false);
        }
    }

    void EndWave()
    {
        // Disable the NPC for the current wave
        npcsToEnable[currentWave].SetActive(false);
        Debug.Log("Wave " + (currentWave + 1) + " ended, disabling NPC: " + npcsToEnable[currentWave].name);

        // Move to the next wave
        currentWave++;

        if (currentWave < numberOfWaves)
        {
            StartWave();
        }
        else
        {
            Debug.Log("All waves completed.");
        }
    }
}
