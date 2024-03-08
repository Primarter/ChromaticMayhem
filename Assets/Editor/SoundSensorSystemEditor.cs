using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Knossos.Minotaur;

[CustomEditor(typeof(SoundSensorSystem))]
public class SoundSensorSystemEditor : Editor
{
    public void OnSceneGUI()
    {
        var s = (SoundSensorSystem)target;

        Handles.color = Color.green;
        Handles.DrawSolidDisc(s.suspiciousSoundPosition, Vector3.up, 1f);
    }
}


