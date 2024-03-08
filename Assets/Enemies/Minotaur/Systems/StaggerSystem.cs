using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Knossos.Enemies;

namespace Knossos.Minotaur
{

public class StaggerSystem : MonoBehaviour
{
    [SerializeField] VisualEffect shieldEffect;

    [HideInInspector] public bool stagger = false;

    OnHitEventSystem onHitEventSystem;

    private void Awake()
    {
        onHitEventSystem = GetComponent<OnHitEventSystem>();
        if (onHitEventSystem == null)
        {
            this.enabled = false;
        }
    }

    public void Stagger()
    {
        stagger = true;
    }

    public void ProcessHit(OnHitEventSystem.HitInfo hit)
    {
        if (hit.knockBackStrength > 50)
        {
            stagger = true;
            onHitEventSystem.OnHitDefaultEffect(hit);
        } else
        {
            shieldEffect.Play();
        }
    }
}

}
