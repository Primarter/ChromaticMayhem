using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractShapeKey : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] Material material;

    private void Update()
    {
        material.SetFloat("_Shape", meshRenderer.GetBlendShapeWeight(0) / 100f);
    }

    private void OnDestroy()
    {
        material.SetFloat("_Shape", 0f);
    }
}
