using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Knossos.Character
{

[RequireComponent(typeof(AnimationController))]
public class VFXController : MonoBehaviour
{
    [SerializeField]
    private VisualEffect[] visualEffects;

    public void PlayEffect(int effectIdx)
    {
        if (effectIdx >= 0 && effectIdx < visualEffects.Length)
            visualEffects[effectIdx].Play();
        else
            Debug.Log($"Invalid effectIdx {effectIdx} in PlayEffect");
    }

    public void PauseEffect(int effectIdx)
    {
        if (effectIdx >= 0 && effectIdx < visualEffects.Length)
        {
            visualEffects[effectIdx].playRate = 0f;
        } else
            Debug.Log($"Invalid effectIdx {effectIdx} in PlayEffect");
    }

    public void ResumeEffect(int effectIdx)
    {
        if (effectIdx >= 0 && effectIdx < visualEffects.Length)
        {
            visualEffects[effectIdx].playRate = 1f;
        } else
            Debug.Log($"Invalid effectIdx {effectIdx} in PlayEffect");
    }

}

}