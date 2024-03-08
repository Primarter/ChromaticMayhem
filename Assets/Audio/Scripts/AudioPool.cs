using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool
{
    const int maxPoolSize = 32;
    Stack<GameObject> audioSourcePool;
    List<GameObject> audioSourcePlaying;

    public int poolCount => audioSourcePool.Count;
    public int playingCount => audioSourcePlaying.Count;

    public GameObject poolParent;

    public void Initialize()
    {
        audioSourcePool = new(maxPoolSize);
        audioSourcePlaying = new();

        poolParent = new GameObject("AudioSourcePool");

        for (int i = 0 ; i < maxPoolSize ; ++i)
        {
            GameObject obj = new();
            obj.name = "AudioSourcePoolObject";
            obj.transform.SetParent(poolParent.transform, false);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;

            AudioSource audioSource = obj.AddComponent<AudioSource>();

            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1.0f;
            audioSource.minDistance = 1.0f;
            audioSource.maxDistance = 30f; // default max Distance

            audioSourcePool.Push(obj);
        }
    }

    public void updatePool()
    {
        // remove from audioSourcePlaying and add to audioSourcePool if isn't playing
        audioSourcePlaying.RemoveAll(obj => {

            if (obj == null) return true; // if object has been destroyed by scene change, remove it

            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (!audioSource.isPlaying)
            {
                obj.transform.SetParent(poolParent.transform, false);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.rotation = Quaternion.identity;

                audioSourcePool.Push(obj);
                return true;
            }
            return false;
        });
    }

    public GameObject getAvailablePoolObject()
    {
        if (audioSourcePool.Count == 0)
        {
            Debug.LogError("Not enough element in audioSourcePool! " + audioSourcePlaying.Count + " are currently being used.");
            return null;
        }

        GameObject obj = audioSourcePool.Pop();
        audioSourcePlaying.Add(obj);

        return obj;
    }

    public void StopAll()
    {
        audioSourcePlaying.ForEach(obj => obj.GetComponent<AudioSource>().Stop());
        updatePool();
    }
}