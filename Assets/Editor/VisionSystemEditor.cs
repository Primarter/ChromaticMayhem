using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Knossos.Minotaur;

[CustomEditor(typeof(VisionSystem))]
public class VisionSystemEditor : Editor
{
    public void OnSceneGUI()
    {
        var s = (VisionSystem)target;

        Handles.color = Color.white;
        Handles.DrawWireDisc(s.transform.position, Vector3.up, s.viewRange);
        Handles.DrawWireDisc(s.transform.position, Vector3.up, s.minDetectionRange);

        Vector3 forward2D = new Vector3(s.transform.forward.x, 0f, s.transform.forward.z);
        Vector3 viewAngleL = Quaternion.Euler(0f, -s.FOV / 2f, 0f) * forward2D;
        Vector3 viewAngleR = Quaternion.Euler(0f, s.FOV / 2f, 0f) * forward2D;

        Handles.DrawLine(s.transform.position, s.transform.position + viewAngleL.normalized * s.viewRange);
        Handles.DrawLine(s.transform.position, s.transform.position + viewAngleR.normalized * s.viewRange);
    }
}
