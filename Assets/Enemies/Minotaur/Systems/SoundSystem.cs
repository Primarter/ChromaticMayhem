using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Minotaur
{
    public class SoundSystem : MonoBehaviour
    {
        [SerializeField] AudioClip attackSound;
        [SerializeField] AudioClip[] footstepSounds;

        public void PlayFootstepSound()
        {
            SoundManager.PlaySound(
                transform.position,
                ArrayHelper.PickRandom(footstepSounds),
                spatialBlend: 0.9f,
                volume: 0.4f,
                pitch: Random.Range(0.7f, 1.3f)
            );
        }

        public void PlayAttackSound()
        {
            SoundManager.PlaySound(
                transform.position,
                attackSound,
                spatialBlend: 0.8f,
                volume: 0.45f,
                pitch: Random.Range(0.7f, 1.5f)
            );
        }
    }
}