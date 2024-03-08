using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelpers
{
    public static IEnumerator Progress(int frames, Action<float> action)
    {
        int startFrame = Time.frameCount;

        while (Time.frameCount < startFrame + frames)
        {
            action((float)(Time.frameCount - startFrame)/(float)(frames));
            yield return null;
        }
    }

    public static IEnumerator Progress(float duration, Action<float> action)
    {
        var startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            action((Time.time - startTime) / duration);
            yield return null;
        }
    }

    public static IEnumerator ExecuteAfter(int frames, Action endCallback)
    {
        int startFrame = Time.frameCount;

        while (Time.frameCount < startFrame + frames)
        {
            yield return null;
        }
        endCallback();
    }

    public static IEnumerator ExecuteAfter(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);
        callback();
    }
}