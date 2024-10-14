using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WavesSystem : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public List<GameObject> npcsToEnable;     
        public List<GameObject> enemiesToEnable;  
    }

    public List<Wave> waves;                     // List of waves
    public GameObject objectA;
    public GameObject objectB;
    public GameObject objectC;
    public float timePerWave = 30f;              // Time for each wave
    public float transitionTime = 10f;           // 10-second transition between waves
    public TextMeshProUGUI wavesText;

    private int currentWave = 0;
    private float waveTimer = 0f;
    private float transitionTimer = 0f;
    private bool inTransition = true;

    private bool objectAEnabled = false;
    private bool objectBEnabled = false;
    private bool objectCEnabled = false;

    void Start()
    {
        StartTransition();
    }

    void Update()
    {
        if (inTransition)
        {
            transitionTimer += Time.deltaTime;
            if (transitionTimer >= transitionTime)
            {
                inTransition = false;
                StartWave();
            }
        }
        else if (currentWave < waves.Count)
        {
            waveTimer += Time.deltaTime;

            // Handle wave-specific objects
            HandleWaveObjects();

            // Check if wave time has ended
            if (waveTimer >= timePerWave)
            {
                EndWave();
                if (currentWave < waves.Count)
                {
                    StartTransition();
                }
            }
        }
    }

    void StartTransition()
    {
        inTransition = true;
        transitionTimer = 0f;

        // Disable NPCs and enemies during the transition
        if (currentWave < waves.Count)
        {
            foreach (var npc in waves[currentWave].npcsToEnable)
            {
                npc.SetActive(false);
            }
            foreach (var enemy in waves[currentWave].enemiesToEnable)
            {
                enemy.SetActive(false);
            }
        }

        Debug.Log("Transition started for 10 seconds.");
    }

    void StartWave()
    {
        if (currentWave < waves.Count)
        {
            waveTimer = 0f;
            objectAEnabled = false;
            objectBEnabled = false;
            objectCEnabled = false;

            // Enable NPCs and enemies for the current wave
            foreach (var npc in waves[currentWave].npcsToEnable)
            {
                npc.SetActive(true);
            }
            foreach (var enemy in waves[currentWave].enemiesToEnable)
            {
                enemy.SetActive(true);
            }

            Debug.Log("Wave " + (currentWave + 1) + " started, enabling NPCs and enemies.");

            UpdateWavesUI();    
        }
    }

    void EndWave()
    {
        Debug.Log("Wave " + (currentWave + 1) + " ended.");

        currentWave++;

        if (currentWave >= waves.Count)
        {
            Debug.Log("All waves completed.");
        }
    }

    void HandleWaveObjects()
    {
        if (currentWave == waves.Count - 1)
        {
            // Last wave - handle objectC
            if (waveTimer <= 10f && !objectCEnabled)
            {
                objectC.SetActive(true);
                objectCEnabled = true;
            }
            else if (waveTimer > 10f && objectCEnabled)
            {
                objectC.SetActive(false);
                objectCEnabled = false;
            }
        }
        else
        {
            // Other waves - handle objectA and objectB
            if (waveTimer <= 10f && !objectAEnabled)
            {
                objectA.SetActive(true);
                objectAEnabled = true;
            }
            else if (waveTimer > 10f && objectAEnabled)
            {
                objectA.SetActive(false);
                objectAEnabled = false;
            }

            if (waveTimer >= timePerWave - 10f && !objectBEnabled)
            {
                objectB.SetActive(true);
                objectBEnabled = true;
            }
            else if (waveTimer > timePerWave && objectBEnabled)
            {
                objectB.SetActive(false);
                objectBEnabled = false;
            }
        }
    }

    public void UpdateWavesUI()
    {
        wavesText.text = "Wave " + (currentWave + 1).ToString();
    }
}
