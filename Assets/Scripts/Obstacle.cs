using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Obstacle : MonoBehaviour
{
    public GameObject originalObstacle;
    public GameObject usedObstacle;
    public bool isUsed = false;
    public GameObject obstacleButton;
    public NavMeshSurface navMeshManager;
    public TutorialMode tutorialManager;

    public void UseObstacle()
    {
        if (PlayerMovement.isStunned == false)
        {
            originalObstacle.SetActive(false);
            usedObstacle.SetActive(true);
            obstacleButton.SetActive(false);
            isUsed = true;
            navMeshManager.BuildNavMesh(); //bake new nav mesh data

            if(TutorialMode.tutorialOn && TutorialMode.usedObstacle == false)
            {
                tutorialManager.UseObstacle();
            }
        }
    }

    public void ResetObstacle()
    {
        originalObstacle.SetActive(true);
        usedObstacle.SetActive(false);
        obstacleButton.SetActive(true);
        isUsed = false;
        navMeshManager.BuildNavMesh();
    }
}
