using System;
using System.Collections;
using System.Collections.Generic;
using Knossos.Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class SelectionMenu : MonoBehaviour
{
    [SerializeField] List<Transform> positions = new();
    [SerializeField] UnityEvent actions;
    [SerializeField] int startingIndex;
    [SerializeField] Transform cursor;

    ClampedIntParameter index;
    System.Diagnostics.Stopwatch sw = new();

    private void Start()
    {
        index = new(startingIndex, 0, actions.GetPersistentEventCount() - 1);
        if (positions.Count != actions.GetPersistentEventCount())
        {
            Debug.LogError("Position and action counts do not match");
            this.enabled = false;
        }
        cursor.position = positions[index.value].position;
        sw.Start();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Special"))
        {
            SendMessage(actions.GetPersistentMethodName(index.value), actions.GetPersistentTarget(index.value));
        }

        if (sw.ElapsedMilliseconds > 100)
        {
            if (Input.GetAxisRaw("Vertical") > .2)
            {
                index.value = index.value - 1;
                sw.Restart();
                cursor.position = positions[index.value].position;
            }
            else if (Input.GetAxisRaw("Vertical") < -.2)
            {
                index.value = index.value + 1;
                sw.Restart();
                cursor.position = positions[index.value].position;
            }
        }
    }
}
