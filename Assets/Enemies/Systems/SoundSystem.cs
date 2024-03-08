using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class SoundSystem : MonoBehaviour
    {
        [SerializeField] AudioClip takeDamageSound;

        public void PlayTakeDamageSound()
        {
            SoundManager.PlaySound(
                transform.position,
                takeDamageSound,
                spatialBlend: 0.5f,
                volume: Random.Range(0.7f, 0.9f),
                pitch: Random.Range(0.6f, 1.6f)
            );
        }

    }
}