using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Knossos.Character
{

public class DamageVisualEffect : MonoBehaviour
{
    [SerializeField] AnimationController animationController;
    [SerializeField] Material damageMaterial;
    [SerializeField] VisualEffect particle;
    [SerializeField] Material screenMaterial;
    [SerializeField] AnimationCurve screenFXOpacity;
    [SerializeField] float screenFXDurationMult = 2;

    Coroutine endEventCoroutine;

    public void StartDamageEffect(int damage)
    {
        if (damage > 0)
        {
            if (endEventCoroutine != null)
            {
                StopAllCoroutines();
            }

            int frames = animationController.config.damageAnimationDurations[0];
            float bound = .25f;
            Health healthManager = GetComponent<Health>();
            if (healthManager == null)
                frames = 10;
            else if ((float)healthManager.health / (float)healthManager.startHealth <= .25 || damage >= healthManager.startHealth / 2)
            {
                bound = 1f;
                frames = animationController.config.damageAnimationDurations[2];
            }
            else if ((float)healthManager.health / (float)healthManager.startHealth <= .50 || damage >= healthManager.startHealth / 4)
            {
                bound = .5f;
                frames = animationController.config.damageAnimationDurations[1];
            }

            particle.Play();
            endEventCoroutine = StartCoroutine(CoroutineHelpers.ExecuteAfter(
                Mathf.Max(frames * screenFXDurationMult, frames),
                () => animationController.onDamageAnimEnd.Invoke(damage)
            ));
            StartCoroutine(CoroutineHelpers.Progress(frames, p => damageMaterial.SetFloat("_Progress", p)));
            StartCoroutine(CoroutineHelpers.Progress(
                (int)(frames * screenFXDurationMult),
                p => screenMaterial.SetFloat("_Opacity", Mathf.Clamp(screenFXOpacity.Evaluate(p), 0, bound))
            ));
        }
    }

    private void OnDestroy()
    {
        damageMaterial.SetFloat("_Progress", 0f);
        screenMaterial.SetFloat("_Opacity", 0f);
    }
}

}