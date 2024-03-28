using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class ForwardAnimParam : MonoBehaviour
{
    public enum ParamType {
        Bool, Float, Int
    }
    [SerializeField] ParamType paramType = ParamType.Float;
    [SerializeField] Animator animator;
    [SerializeField] string animParam;
    [SerializeField] string propertyTarget;

    private VisualEffect visualEffect;

    void Start()
    {
        this.visualEffect = GetComponent<VisualEffect>();

        if (!this.animator) {
            Debug.LogError("Animator not assigned to ForwardAnimParam component " + gameObject.name);
            this.enabled = false;
        }
    }

    void Update()
    {
        switch (paramType) {
            case ParamType.Bool:
                visualEffect.SetBool(propertyTarget, animator.GetBool(animParam));
                break;
            case ParamType.Float:
                visualEffect.SetFloat(propertyTarget, animator.GetFloat(animParam));
                break;
            case ParamType.Int:
                visualEffect.SetInt(propertyTarget, animator.GetInteger(animParam));
                break;
        }
    }
}
