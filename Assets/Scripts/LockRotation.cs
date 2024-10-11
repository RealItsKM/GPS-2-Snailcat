using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    public float lockedYRotation = 0f;  // Set your desired constant Y rotation value
    public float lockedZRotation = 0f;  // Set your desired constant Z rotation value

    void Update()
    {
        // Get the current rotation of the GameObject
        Quaternion currentRotation = transform.rotation;

        // Lock the Y and Z axis, but allow the X axis to rotate freely
        transform.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, lockedYRotation, lockedZRotation);
    }
}
