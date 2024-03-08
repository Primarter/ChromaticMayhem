using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

public class VisibilitySystem : MonoBehaviour
{
    int detectionCounter = 0;
    public bool isDetected
    {
        get => detectionCounter > 0;
    }

    public void AlertPlayer()
    {
        detectionCounter += 1;
    }

    public void LosePlayer()
    {
        detectionCounter -= 1;
    }
}

}