using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Knossos.Minotaur;

[CustomEditor(typeof(PathSystem))]
public class PathSystemEditor : Editor
{
    public void OnSceneGUI()
    {
        var s = (PathSystem)target;

        if (s.waypoints == null || s.waypoints.Length == 0) return;

        Waypoint waypoint = s.getClosestWaypoint();
        Handles.DrawLine(s.transform.position, waypoint.obj.transform.position);

        Handles.color = Color.blue;
        foreach (Waypoint w in s.waypoints)
        {
            foreach (Waypoint n in w.neighbours)
            {
                Handles.DrawLine(w.obj.transform.position, n.obj.transform.position);
            }
        }
    }
}
