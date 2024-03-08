using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knossos.Interaction
{

public class Interactable : MonoBehaviour
{
    public UnityEvent<GameObject> onInteractEvent;
    public delegate void OnInteract(GameObject player);
    public OnInteract onInteractCallbacks;

    GameObject player;

    private void Update()
    {
        if (Character.InputManager.inputs.interact && player != null)
        {
            onInteractEvent.Invoke(player);
            if (onInteractCallbacks != null)
            {
                onInteractCallbacks(player);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player")
        {
            player = null;
        }
    }
}

}