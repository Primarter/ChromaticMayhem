using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudSoundManager
{
    public delegate void LoudSound(Vector3 position);
    public static LoudSound OnLoudSound;

    public static void playLoudSound(Vector3 position)
    {
        if (OnLoudSound != null)
            OnLoudSound(position);
    }
}
