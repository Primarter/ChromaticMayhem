using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Torch : MonoBehaviour
{
    [SerializeField] VisualEffect bonfire;
    [SerializeField] VisualEffect sparks;
    [SerializeField] Light pointLight;
    bool on = false;
    float baseIntensity;

    private void Start()
    {
        baseIntensity = pointLight.intensity;
    }

    private void Update() {
        pointLight.intensity = baseIntensity + SineNoise(Time.time / 60f) * (baseIntensity / 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!on && other.tag == "Player")
        {
            bonfire.Play();
            sparks.Play();
            pointLight.enabled = true;
            on = true;
            StartCoroutine(StopSparks());
        }
    }

    private IEnumerator StopSparks()
    {
        yield return new WaitForSeconds(.1f);
        sparks.Stop();
    }

    float SineNoise(float inp)
    {
        float sinIn = Mathf.Sin(inp);
        float sinInOffset = Mathf.Sin(inp + 1.0f);
        float randomno =  (Mathf.Sin((sinIn - sinInOffset) * (12.9898f + 78.233f)) * 43758.5453f) % 1f;
        float noise = Mathf.Lerp(0, 1, randomno);
        return sinIn + noise;
    }
}
