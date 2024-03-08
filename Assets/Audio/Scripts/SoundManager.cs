using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/*
TODO
- connect option slider to audioMixerGroup volume
*/

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerGroup audioMixerGroupMusic;
    [SerializeField] AudioMixerGroup audioMixerGroupEffects;
    [SerializeField] AudioMixerGroup audioMixerGroupGameplay;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource effectsSource;

    [SerializeField] Sound[] sounds;
    IDictionary<string, Sound> soundsMap = new Dictionary<string, Sound>();

    AudioPool audioPool = new AudioPool();

    [SerializeField] int poolSize;
    [SerializeField] int playingSoundsCount;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += ChangedActiveScene;

        foreach (Sound s in sounds)
        {
            soundsMap.Add(s.name, s);
        }
    }

    void Update()
    {
        audioPool.updatePool();

        poolSize = audioPool.poolCount;
        playingSoundsCount = audioPool.playingCount;
    }

    public static void PlayMusic(string name)
    {
        instance.musicSource.clip = instance.soundsMap[name].clip;
        instance.musicSource.Play();
    }

    public static void StopMusic()
    {
        instance.musicSource.Stop();
    }

    public static void PlaySound2D(string name, float volume = 1.0f, float pitch = 1.0f)
    {
        if (!instance.soundsMap.ContainsKey(name))
        {
            Debug.LogError("Sound " + name + " doesn't exist!");
            return;
        }

        Sound s = instance.soundsMap[name];

        GameObject obj = instance.audioPool.getAvailablePoolObject();
        if (obj == null) return;
        AudioSource audioSource = obj.GetComponent<AudioSource>();

        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        // obj.transform.SetParent(null, false);

        audioSource.clip = s.clip;
        audioSource.outputAudioMixerGroup = instance.audioMixerGroupEffects; // effect audioMixer by default
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = 0f;

        audioSource.Play();
    }

    public static void PlaySound(
        Vector3 position,
        AudioClip clip,
        float volume = 1.0f,
        float pitch = 1.0f,
        float spatialBlend = 1.0f,
        float minDistance = 1.0f,
        float maxDistance = 20.0f)
        // AudioType type
    {
        GameObject obj = instance.audioPool.getAvailablePoolObject();
        if (obj == null) return;

        AudioSource audioSource = obj.GetComponent<AudioSource>();

        obj.transform.SetParent(null, false);
        obj.transform.localPosition = position;
        obj.transform.rotation = Quaternion.identity;

        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = instance.audioMixerGroupGameplay; // gameplayer audioMixer by default
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = spatialBlend;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.Play();
    }

    public static void PlaySound(
        Transform parent,
        AudioClip clip,
        float volume = 1.0f,
        float pitch = 1.0f,
        float spatialBlend = 1.0f,
        float minDistance = 1.0f,
        float maxDistance = 30.0f)
    {
        GameObject obj = instance.audioPool.getAvailablePoolObject();
        if (obj == null) return;
        AudioSource audioSource = obj.GetComponent<AudioSource>();

        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.SetParent(parent, false);

        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = instance.audioMixerGroupGameplay; // gameplayer audioMixer by default
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.spatialBlend = spatialBlend;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.Play();
    }

    // used to set the outputAudioMixerGroup of an audio source
    public static void RegisterAudioSource(AudioSource audioSource, AudioType type)
    {
        switch (type)
        {
            case AudioType.Gameplay:
                audioSource.outputAudioMixerGroup = instance.audioMixerGroupGameplay;
                break;
            case AudioType.Music:
                // audioSource.outputAudioMixerGroup = instance.audioMixerGroupMusic;
                // break;
            case AudioType.Effect:
                // audioSource.outputAudioMixerGroup = instance.audioMixerGroupEffects;
                Debug.LogError("Music and effect types can't be registered as audio sources. They have to be played by calling SoundManager.PlaySound()");
                break;
            default:
                Debug.LogError("Audio type " + type + " is not valid!");
                break;
        }
    }

    public static void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;

        // TODO: use master audio mixer instead
        // instance.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public static void setVolume(AudioType type, float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f); // TODO: find if possible to clamp to 0 instead of 0.0001

        switch (type)
        {
            case AudioType.Gameplay:
                instance.audioMixer.SetFloat("GameplayVolume", Mathf.Log10(volume) * 20);
                break;
            case AudioType.Music:
                instance.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
                break;
            case AudioType.Effect:
                instance.audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
                break;
            default:
                Debug.LogError("Audio type " + type + " doesn't exist!");
                break;
        }
    }

    void ChangedActiveScene(Scene current, Scene next) // this is called after a scene has changed
    {
        audioPool.Initialize();
    }
}