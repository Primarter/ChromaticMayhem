using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knossos.Enemies
{


public class OnHitEventSystem : MonoBehaviour
{
    public struct HitInfo
    {
        public int damage;
        public float staggerDuration;
        public float knockBackStrength;
        public float knockBackDuration;
        public Vector3 hitDirection;
    }

    public UnityEvent<HitInfo> onHitEvent = new();

    [SerializeField] Renderer meshRenderer;
    public Material damageMaterial;
    [SerializeField] GameObject damageParticle;
    private Material regularMaterial;

    private void Awake()
    {
        regularMaterial = meshRenderer.sharedMaterial;
    }

    public void TakeDamage(Character.MoveInfo moveInfo = new(), Vector3 hitDirection = new())
    {
        HitInfo hitInfo = new HitInfo
        {
            damage=moveInfo.damage,
            staggerDuration=moveInfo.staggerDuration,
            knockBackStrength=moveInfo.knockBackStrength,
            knockBackDuration=moveInfo.knockBackDuration,
            hitDirection=hitDirection,
        };

        onHitEvent.Invoke(hitInfo);
    }

    public void OnHitDefaultEffect(HitInfo info)
    {
        if (info.hitDirection != Vector3.zero)
            Destroy(GameObject.Instantiate(damageParticle, transform.position, Quaternion.LookRotation(info.hitDirection)), .5f);
        if (isActiveAndEnabled)
            StartCoroutine(MaterialChangeCoroutine());
    }

    IEnumerator MaterialChangeCoroutine()
    {
        int frame = Time.frameCount;
        meshRenderer.material = damageMaterial;

        while (Time.frameCount < frame + 10)
        {
            yield return null;
        }
        meshRenderer.material = regularMaterial;
    }
}

}