using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Knossos
{

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    Coroutine coroutine;

    [SerializeField] Character.Config config;
    [SerializeField] Character.Health healthManager;

    [SerializeField] AnimationCurve[] curves = new AnimationCurve[3];

    [Header("Defaults")]
    [SerializeField] int defaultDuration = 10;
    [SerializeField] AnimationCurve defaultCurve;

    [SerializeField] float shakeAmplitude = .5f;
    [SerializeField] float shakeFrequency = .3f;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void HitShakeScreen(int hit)
    {
        if (coroutine != null || hit < 0 || hit >= config.damageAnimationDurations.Length)
            return;

        coroutine = StartCoroutine(SimpleShakeCoroutine((int)(config.damageAnimationDurations[hit] * .75), shakeAmplitude, shakeFrequency));
    }

    public void DamageShakeScreen(int damage)
    {
        int frames;
        AnimationCurve curve;

        if (healthManager == null)
        {
            frames = defaultDuration;
            curve = defaultCurve;
        }
        else if ((float)healthManager.health / (float)healthManager.startHealth <= .25 || damage >= healthManager.startHealth / 2)
        {
            frames = config.damageAnimationDurations[2];
            curve = curves[2];
        }
        else if ((float)healthManager.health / (float)healthManager.startHealth <= .50 || damage >= healthManager.startHealth / 4)
        {
            frames = config.damageAnimationDurations[1];
            curve = curves[1];
        }
        else
        {
            frames = config.damageAnimationDurations[0];
            curve = curves[0];
        }
        coroutine = StartCoroutine(ScreenShakeCoroutine(frames, curve));
    }

    IEnumerator ScreenShakeCoroutine(int frames, AnimationCurve curve)
    {
        int startFrame = Time.frameCount;

        var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();


        while (Time.frameCount < startFrame + frames)
        {
            noise.m_AmplitudeGain = curve.Evaluate((float)(Time.frameCount - startFrame)/(float)(frames));
            noise.m_FrequencyGain = curve.Evaluate((float)(Time.frameCount - startFrame)/(float)(frames));
            yield return null;
        }

        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
        coroutine = null;
    }

    IEnumerator SimpleShakeCoroutine(int frames, float amplitude, float frequency)
    {
        int startFrame = Time.frameCount;

        var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();


        while (Time.frameCount < startFrame + frames)
        {
            noise.m_AmplitudeGain = amplitude * defaultCurve.Evaluate((float)(Time.frameCount - startFrame)/(float)(frames));
            noise.m_FrequencyGain = frequency * defaultCurve.Evaluate((float)(Time.frameCount - startFrame)/(float)(frames));
            yield return null;
        }

        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
        coroutine = null;
    }
}

}