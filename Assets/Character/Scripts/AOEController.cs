using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Knossos.Character
{

[RequireComponent(typeof(VisualEffect), typeof(SphereCollider), typeof(Hitbox))]
public class AOEController : MonoBehaviour
{
    [SerializeField] AnimationCurve hitboxEvolution;

    SphereCollider areaOfEffect;
    float lifetime;
    float baseRadius;
    bool hitMinotaur = false;

    Minotaur.StaggerSystem minotaur;

    private void Awake()
    {
        lifetime = GetComponent<VisualEffect>()?.GetFloat("Lifetime") ?? 0f;
        areaOfEffect = GetComponent<SphereCollider>();
    }

    public void ActivateAOE()
    {
        baseRadius = areaOfEffect.radius;
        StartCoroutine(UpdateAOE());
    }

    IEnumerator UpdateAOE()
    {
        var start = Time.time;
        hitMinotaur = false;

        while (Time.time < start + lifetime)
        {
            float progress = hitboxEvolution.Evaluate((Time.time - start) / lifetime);
            areaOfEffect.radius = baseRadius * progress;

            if (!hitMinotaur && minotaur != null)
            {
                hitMinotaur = true;
                // minotaur.Stagger();
            }
            yield return null;
        }
        areaOfEffect.radius = baseRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Minotaur")
        {
            minotaur = other.GetComponent<Minotaur.StaggerSystem>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Minotaur")
        {
            minotaur = null;
        }
    }
}

}