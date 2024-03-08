using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] Transform cutoutObject;

    // [SerializeField] Material cutoutMaterial;
    [SerializeField] GameObject cutoutGameObject;

    void Update()
    {
        Material[] materials = cutoutGameObject.GetComponent<Renderer>().materials;

        foreach (Material mat in materials)
        {
            mat.SetVector("_PlayerPosition", cutoutObject.position);
            // print(cutoutObject.position);
        }
    }
}
