using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Knossos.Character
{

public class Hiding : MonoBehaviour
{
    [SerializeField] string bushTag;

    public bool isHiding;
    public float focusDuration = .5f;
    public float regularVignette = .25f;
    public float focusVignette = .4f;

    private Vignette vignette;
    Coroutine currentEffect;

    void Start()
    {
        isHiding = false;
        Volume postProcessingVolume = FindObjectOfType<Volume>();
        if (!postProcessingVolume)
        {
            Debug.LogError("Couldn't find post-processing voluming for Hiding vignette effect");
            return;
        }
        if (!postProcessingVolume.sharedProfile.TryGet(out vignette))
        {
            vignette = postProcessingVolume.sharedProfile.Add<Vignette>();
            Debug.Log("Didn't find vignette effect, added it to volume profile");
        }
    }

    void OnDestroy()
    {
        vignette.intensity.value = regularVignette;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == bushTag)
        {
            isHiding = true;
            if (vignette)
            {
                if (currentEffect != null) StopCoroutine(currentEffect);
                currentEffect = StartCoroutine(Focus());
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == bushTag)
        {
            isHiding = false;
            if (vignette)
            {
                if (currentEffect != null) StopCoroutine(currentEffect);
                currentEffect = StartCoroutine(Unfocus());
            }
        }
    }

    IEnumerator Focus()
    {
        float progress = (vignette.intensity.value - regularVignette) / (focusVignette - regularVignette);
        float elapsedTime = progress * focusDuration;
        float startTime = Time.time - elapsedTime;
        while (Time.time < startTime + focusDuration)
        {
            progress = (Time.time - startTime) / focusDuration;
            vignette.intensity.value = regularVignette + Easing.easeOutQuint(progress) * (focusVignette - regularVignette);
            yield return null;
        }
    }

    IEnumerator Unfocus()
    {
        float progress = 1 - (vignette.intensity.value - regularVignette) / (focusVignette - regularVignette);
        float elapsedTime = progress * focusDuration;
        float startTime = Time.time - elapsedTime;
        while (Time.time < startTime + focusDuration)
        {
            progress = (Time.time - startTime) / focusDuration;
            vignette.intensity.value = regularVignette + Easing.easeOutQuint(1 - progress) * (focusVignette - regularVignette);
            yield return null;
        }
    }
}

}