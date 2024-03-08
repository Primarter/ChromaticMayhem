using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Knossos.Minotaur;

[CustomEditor(typeof(LocomotionSystem))]
public class LocomotionSystemEditor : Editor
{
    public void OnSceneGUI()
    {
        var s = (LocomotionSystem)target;

        Handles.color = Color.red;
        Handles.DrawSolidDisc(s.navMeshAgent.destination, Vector3.up, 1f);
    }
}


