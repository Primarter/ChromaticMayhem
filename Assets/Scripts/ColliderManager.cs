using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ColliderManager : MonoBehaviour
{
    [SerializeField] string[] tags;

    [HideInInspector] public List<GameObject> collidingGameObjects = new();

    BoxCollider[] colliders;

    private void Awake()
    {
        colliders = GetComponents<BoxCollider>();
    }

    public GameObject[] GetColliding()
    {
        return collidingGameObjects.ToArray();
    }

    private void FixedUpdate()
    {
        collidingGameObjects.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if (tags.Contains(other.tag))
        {
            collidingGameObjects.Add(other.gameObject);
        }
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     // if (tags.Contains(other.tag))
    //     // {
    //     //     print(other.tag);
    //     //     print("ENTER");
    //     //     collidingGameObjects.Add(other.gameObject);
    //     //     if (OnTriggerIn != null)
    //     //         OnTriggerIn(other);
    //     // }
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     // if (tags.Contains(other.tag))
    //     // {
    //     //     print(other.tag);
    //     //     print("EXIT");
    //     //     collidingGameObjects.Remove(other.gameObject);
    //     //     if (OnTriggerOut != null)
    //     //         OnTriggerOut(other);
    //     // }
    // }
}
